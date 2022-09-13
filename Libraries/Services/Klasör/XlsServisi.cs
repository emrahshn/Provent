using System;
using System.Linq;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using Core.Domain.Teklif;
using Services.Tanımlamalar;
using Services.Teklifler;
using Services.Kullanıcılar;
using System.Text;
using Services.Medya;
using Services.Yapılandırma;
using Core.Domain.Genel;

namespace Services.Klasör
{
    public partial class XlsServisi : IXlsServisi
    {
        CultureInfo ci = new CultureInfo("tr-TR");
        private readonly IFirmaServisi _musteriServisi;
        private readonly IBagliTeklifOgesiServisi _bagliTeklifOgesi;
        private readonly IKullanıcıServisi _kullanıcıServisi;
        private readonly IYetkililerServisi _yetkiliServisi;
        private readonly ITeklifServisi _teklifServisi;
        private readonly IResimServisi _resimServisi;
        private readonly IAyarlarServisi _ayarlarServisi;
        public XlsServisi(IFirmaServisi musteriServisi,
            IResimServisi resimServisi,
            IBagliTeklifOgesiServisi bagliTeklifOgesi,
            IKullanıcıServisi kullanıcıServisi,
            IYetkililerServisi yetkiliServisi,
            IAyarlarServisi ayarlarServisi,
            ITeklifServisi teklifServisi)
        {
            this._musteriServisi = musteriServisi;
            this._bagliTeklifOgesi = bagliTeklifOgesi;
            this._kullanıcıServisi = kullanıcıServisi;
            this._yetkiliServisi = yetkiliServisi;
            this._teklifServisi = teklifServisi;
            this._resimServisi = resimServisi;
            this._ayarlarServisi = ayarlarServisi;
        }
        
        #region CreateExcelDocument
        public bool CreateExcelDocument(DataSet ds, string excelFilename, bool includeAutoFilter)
        {
            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(excelFilename, SpreadsheetDocumentType.Workbook))
                {
                    WriteExcelFile(ds, document, includeAutoFilter);
                }
                Trace.WriteLine("Successfully created: " + excelFilename);
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed, exception thrown: " + ex.Message);
                return false;
            }
        }
        #endregion
        #region WriteExcelFile
        private static void WriteExcelFile(DataSet ds, SpreadsheetDocument spreadsheet, bool includeAutoFilter)
        {
            // Reset rows cache
            _rowListCache = new List<Row>();

            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = spreadsheet.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet();

            // add styles to sheet
            WorkbookStylesPart wbsp = workbookpart.AddNewPart<WorkbookStylesPart>();
            wbsp.Stylesheet = CreateStylesheet();
            wbsp.Stylesheet.Save();

            DataTable dt = ds.Tables[0];
            string worksheetName = dt.TableName;

            // Create columns calculating size of biggest text for the database column
            int numberOfColumns = dt.Columns.Count;
            Columns columns = new Columns();
            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                DataColumn col = dt.Columns[colInx];

                string maxText = col.ColumnName;
                foreach (DataRow dr in dt.Rows)
                {
                    string value = string.Empty;
                    if (col.DataType.FullName == "System.DateTime")
                    {
                        DateTime dtValue;
                        if (DateTime.TryParse(dr[col].ToString(), out dtValue))
                            value = dtValue.ToShortDateString();
                    }
                    else
                    {
                        value = dr[col].ToString();
                    }

                    if (value.Length > maxText.Length)
                    {
                        maxText = value;
                    }
                }
                double width = GetWidth("Calibri", 11, maxText);
                columns.Append(CreateColumnData((uint)colInx + 1, (uint)colInx + 1, width + 2));
            }
            worksheetPart.Worksheet.Append(columns);

            // Create SheetData and assign to worksheetpart
            SheetData sd = new SheetData();
            worksheetPart.Worksheet.Append(sd);

            // Add Sheets to the Workbook.
            Sheets sheets = spreadsheet.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            // Append a new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet()
            {
                Id = spreadsheet.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = worksheetName
            };
            sheets.Append(sheet);

            // Append this worksheet's data to our Workbook, using OpenXmlWriter, to prevent memory problems
            WriteDataTableToExcelWorksheet(dt, ref worksheetPart, includeAutoFilter);

            // Save it and close it
            spreadsheet.WorkbookPart.Workbook.Save();
            spreadsheet.Close();
        }
        #endregion
        #region WriteDataTableToExcelWorksheet
        private static void WriteDataTableToExcelWorksheet(DataTable dt, ref WorksheetPart worksheetPart, bool includeAutoFilter)
        {
            string cellValue = "";

            //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
            //
            //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
            //  cells of data, we'll know if to write Text values or Numeric cell values.
            int numberOfColumns = dt.Columns.Count;
            bool[] IsNumericColumn = new bool[numberOfColumns];
            bool[] IsDateColumn = new bool[numberOfColumns];

            string[] excelColumnNames = new string[numberOfColumns];
            for (int n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = GetExcelColumnName(n);

            //
            //  Create the Header row in our Excel Worksheet
            //
            uint rowIndex = 1;

            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                DataColumn col = dt.Columns[colInx];

                // Save the cell data in spreadsheet
                var cell = CreateSpreadsheetCellIfNotExist(worksheetPart.Worksheet, excelColumnNames[colInx] + rowIndex.ToString());
                cell.CellValue = new CellValue(col.ColumnName);
                cell.DataType = CellValues.String;
                cell.StyleIndex = (UInt32Value)1U;

                IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Int32") || (col.DataType.FullName == "System.Int64") || (col.DataType.FullName == "System.Double") || (col.DataType.FullName == "System.Single");
                IsDateColumn[colInx] = (col.DataType.FullName == "System.DateTime");
            }

            // Set the AutoFilter property to a range that is the size of the data
            // within the worksheet
            if (includeAutoFilter)
            {
                AutoFilter autoFilter1 = new AutoFilter()
                { Reference = "A1:" + excelColumnNames[numberOfColumns - 1] + "1" };
                worksheetPart.Worksheet.Append(autoFilter1);
            }

            //
            //  Now, step through each row of data in our DataTable...
            //
            double cellNumericValue = 0;
            foreach (DataRow dr in dt.Rows)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;

                // Add Data
                Cell cell;

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = dr.ItemArray[colInx].ToString();
                    cellValue = ReplaceHexadecimalSymbols(cellValue);

                    // Create cell with data
                    if (IsNumericColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        //  If this numeric value is NULL, then don't write anything to the Excel file.
                        cellNumericValue = 0;
                        if (double.TryParse(cellValue, out cellNumericValue))
                        {
                            cellValue = cellNumericValue.ToString();
                        }
                        else
                        {
                            cellValue = null;
                        }

                        cell = CreateSpreadsheetCellIfNotExist(worksheetPart.Worksheet, excelColumnNames[colInx] + rowIndex.ToString());
                        cell.CellValue = new CellValue(cellValue);
                        cell.DataType = CellValues.Number;
                        cell.StyleIndex = (rowIndex % 2 == 0) ? (UInt32Value)0U : (UInt32Value)3U;
                    }
                    else if (IsDateColumn[colInx])
                    {
                        //  This is a date value.
                        DateTime dtValue;
                        string strValue = "";
                        if (DateTime.TryParse(cellValue, out dtValue))
                            strValue = dtValue.ToOADate().ToString(CultureInfo.InvariantCulture);

                        cell = CreateSpreadsheetCellIfNotExist(worksheetPart.Worksheet, excelColumnNames[colInx] + rowIndex.ToString());
                        cell.CellValue = new CellValue(strValue);
                        cell.DataType = new EnumValue<CellValues>(CellValues.Number);  //Date is only available in Office 2010
                        cell.StyleIndex = (rowIndex % 2 == 0) ? (UInt32Value)2U : (UInt32Value)4U;
                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        cell = CreateSpreadsheetCellIfNotExist(worksheetPart.Worksheet, excelColumnNames[colInx] + rowIndex.ToString());
                        cell.CellValue = new CellValue(cellValue);
                        cell.DataType = CellValues.String;
                        cell.StyleIndex = (rowIndex % 2 == 0) ? (UInt32Value)0U : (UInt32Value)3U;
                    }
                }
            }

            worksheetPart.Worksheet.Save();
        }
        #endregion
        #region ReplaceHexadecimalSymbols
        private static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }
        #endregion
        #region GetExcelColumnName
        //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
        public static string GetExcelColumnName(int columnIndex)
        {
            //  eg  (0) should return "A"
            //      (1) should return "B"
            //      (25) should return "Z"
            //      (26) should return "AA"
            //      (27) should return "AB"
            //      ..etc..
            char firstChar;
            char secondChar;
            char thirdChar;

            if (columnIndex < 26)
            {
                return ((char)('A' + columnIndex)).ToString();
            }

            if (columnIndex < 702)
            {
                firstChar = (char)('A' + (columnIndex / 26) - 1);
                secondChar = (char)('A' + (columnIndex % 26));

                return string.Format("{0}{1}", firstChar, secondChar);
            }

            int firstInt = columnIndex / 26 / 26;
            int secondInt = (columnIndex - firstInt * 26 * 26) / 26;
            if (secondInt == 0)
            {
                secondInt = 26;
                firstInt = firstInt - 1;
            }
            int thirdInt = (columnIndex - firstInt * 26 * 26 - secondInt * 26);

            firstChar = (char)('A' + firstInt - 1);
            secondChar = (char)('A' + secondInt - 1);
            thirdChar = (char)('A' + thirdInt);

            return string.Format("{0}{1}{2}", firstChar, secondChar, thirdChar);
        }
        #endregion
        #region GetWidth
        private static double GetWidth(string font, int fontSize, string text)
        {
            System.Drawing.Font stringFont = new System.Drawing.Font(font, fontSize);
            return GetWidth(stringFont, text);
        }
        #endregion
        #region GetWidth
        private static double GetWidth(System.Drawing.Font stringFont, string text)
        {
            // This formula is based on this article plus a nudge ( + 0.2M )
            // http://msdn.microsoft.com/en-us/library/documentformat.openxml.spreadsheet.column.width.aspx
            // Truncate(((256 * Solve_For_This + Truncate(128 / 7)) / 256) * 7) = DeterminePixelsOfString

            System.Drawing.Size textSize = System.Windows.Forms.TextRenderer.MeasureText(text, stringFont);
            double width = (double)(((textSize.Width / (double)7) * 256) - (128 / 7)) / 256;
            width = (double)decimal.Round((decimal)width + 0.2M, 2);

            return width;
        }
        #endregion
        #region CreateColumnData
        private static Column CreateColumnData(UInt32 StartColumnIndex, UInt32 EndColumnIndex, double ColumnWidth)
        {
            Column column;
            column = new Column
            {
                Min = StartColumnIndex,
                Max = EndColumnIndex,
                Width = ColumnWidth,
                CustomWidth = true
            };
            return column;
        }
        #endregion
        #region GetColumnName
        // Given a cell name, parses the specified cell to get the column name.
        private static string GetColumnName(string cellName)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellName);

            return match.Value;
        }
        #endregion
        #region  GetRowIndex
        // Given a cell name, parses the specified cell to get the row index.
        private static uint GetRowIndex(string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(cellName);

            return uint.Parse(match.Value);
        }
        #endregion
        private static IList<Row> _rowListCache = new List<Row>();

        // Given a Worksheet and a cell name, verifies that the specified cell exists.
        // If it does not exist, creates a new cell. 
        #region CreateSpreadsheetCellIfNotExist
        private static Cell CreateSpreadsheetCellIfNotExist(Worksheet worksheet, string cellName)
        {
            string columnName = GetColumnName(cellName);
            uint rowIndex = GetRowIndex(cellName);

            Row rowWorkSheet = rowIndex < _rowListCache.Count ? _rowListCache[(int)rowIndex] : null;
            Cell cell;

            // If the Worksheet does not contain the specified row, create the specified row.
            // Create the specified cell in that row, and insert the row into the Worksheet.
            if (rowWorkSheet == null)
            {
                Row row = new Row() { RowIndex = new UInt32Value(rowIndex) };
                cell = new Cell() { CellReference = new StringValue(cellName) };
                row.Append(cell);
                worksheet.Descendants<SheetData>().First().Append(row);
                _rowListCache.Add(row);
            }
            else
            {
                Row row = rowWorkSheet;
                cell = new Cell() { CellReference = new StringValue(cellName) };
                row.Append(cell);
            }

            return cell;
        }
        #endregion
        #region CreateStylesheet
        private static Stylesheet CreateStylesheet()
        {
            // Stylesheet declarion and namespace
            Stylesheet stylesheet = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            stylesheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            // List of fonts
            Fonts fontsList = new Fonts() { Count = (UInt32Value)2U, KnownFonts = true };

            // FontId=0 - Regular Excel font
            Font font0 = new Font();
            FontSize fontSize0 = new FontSize() { Val = 11D };
            Color color0 = new Color() { Theme = (UInt32Value)1U };
            FontName fontName0 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering0 = new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme0 = new FontScheme() { Val = FontSchemeValues.Minor };
            font0.Append(fontSize0);
            font0.Append(color0);
            font0.Append(fontName0);
            font0.Append(fontFamilyNumbering0);
            font0.Append(fontScheme0);

            // FontId=1 - Bold font for header
            Font font1 = new Font();
            Bold bold = new Bold();
            font1.Append(bold);

            fontsList.Append(font0);
            fontsList.Append(font1);

            // List of fills
            Fills fillList = new Fills() { Count = (UInt32Value)3U };

            // FillId = 0, Normal background
            Fill fill0 = new Fill();
            PatternFill patternFill0 = new PatternFill() { PatternType = PatternValues.None };
            fill0.Append(patternFill0);

            // FillId = 00, Normal background
            Fill fill00 = new Fill();
            PatternFill patternFill00 = new PatternFill() { PatternType = PatternValues.None };
            fill00.Append(patternFill00);

            // FillId = 1, Light Blue for alternating cells
            Fill fill1 = new Fill();
            PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.Solid };
            ForegroundColor foregroundColor1 = new ForegroundColor() { Rgb = "FFDEEFF7" };
            patternFill1.Append(foregroundColor1);
            fill1.Append(patternFill1);

            fillList.Append(fill0);
            fillList.Append(fill00);
            fillList.Append(fill1);

            // Borders style
            Borders bordersList = new Borders() { Count = (UInt32Value)1U };
            Border border1 = new Border();
            LeftBorder leftBorder1 = new LeftBorder();
            RightBorder rightBorder1 = new RightBorder();
            TopBorder topBorder1 = new TopBorder();
            BottomBorder bottomBorder1 = new BottomBorder();
            DiagonalBorder diagonalBorder1 = new DiagonalBorder();
            border1.Append(leftBorder1);
            border1.Append(rightBorder1);
            border1.Append(topBorder1);
            border1.Append(bottomBorder1);
            border1.Append(diagonalBorder1);
            bordersList.Append(border1);

            // List of cell styles formats
            CellStyleFormats cellStyleFormatsList = new CellStyleFormats() { Count = (UInt32Value)1U };
            CellFormat cellFormat = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };
            cellStyleFormatsList.Append(cellFormat);

            // Cells formats 
            CellFormats cellFormatsList = new CellFormats() { Count = (UInt32Value)5U };

            // StyleIndex = 0 - Regular font
            CellFormat cellFormat0 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };

            // StyleIndex = 1 - Bold font
            CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };

            // StyleIndex = 2 - Date (Short Date)
            CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)14U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };

            // StyleIndex = 3 - Light blue background (Text and Numbers)
            CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)2U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFill = true };

            // StyleIndex = 4 - Light blue background (Date)
            CellFormat cellFormat4 = new CellFormat() { NumberFormatId = (UInt32Value)14U, FontId = (UInt32Value)0U, FillId = (UInt32Value)2U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFill = true };

            cellFormatsList.Append(cellFormat0);
            cellFormatsList.Append(cellFormat1);
            cellFormatsList.Append(cellFormat2);
            cellFormatsList.Append(cellFormat3);
            cellFormatsList.Append(cellFormat4);

            // Cells styles
            CellStyles cellStyleList = new CellStyles() { Count = (UInt32Value)1U };
            CellStyle cellStyle0 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };
            cellStyleList.Append(cellStyle0);

            DifferentialFormats differentialFormats0 = new DifferentialFormats() { Count = (UInt32Value)0U };
            TableStyles tableStyles0 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleMedium9" };
            StylesheetExtensionList stylesheetExtensionList = new StylesheetExtensionList();
            StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
            stylesheetExtensionList.Append(stylesheetExtension1);

            stylesheet.Append(fontsList);
            stylesheet.Append(fillList);
            stylesheet.Append(bordersList);
            stylesheet.Append(cellStyleFormatsList);
            stylesheet.Append(cellFormatsList);
            stylesheet.Append(cellStyleList);
            stylesheet.Append(differentialFormats0);
            stylesheet.Append(tableStyles0);
            stylesheet.Append(stylesheetExtensionList);

            return stylesheet;
        }
        #endregion

        public virtual StringBuilder TeklifRaporOlustur(IList<Teklif> teklifler)
        {

            if (teklifler == null)
                throw new ArgumentNullException("teklifler");
            /*
            var sayfaBüyüklüğü = PageSize.A4;

            if (_pdfAyarları.HarfSayfaBüyüklüğüEtkin)
            {
                sayfaBüyüklüğü = PageSize.LETTER;
            }*/
            StringBuilder sb = new StringBuilder();//
            foreach (var teklif in teklifler)
            {
                var pdfAyarları = _ayarlarServisi.AyarYükle<PdfAyarları>();
                string kurumadi = "", teklifno = teklif.Id.ToString(), ilgilikisi = "", kurumaciklama = "", hazırlayan = "", aciklama = "", konum = "", kodno = "", sparabirimi = "";
                decimal kurEuro, kurDolar, tlTutar = 0, tlAlis = 0, euroTutar = 0, dolarTutar = 0, tlToplam = 0
                    , dolarToplam = 0, euroToplam = 0, tlToplamK = 0, dolarToplamK = 0, euroToplamK = 0
                    , euroToplamG = 0, tlToplamG = 0, euroHizmet = 0, tlHizmet = 0, euroHaftasonu = 0
                    , tlHaftasonu = 0, tlKDV8 = 0, euroKDV8 = 0, tlKDV18 = 0, euroKDV18 = 0, tlKDVG = 0
                    , euroKDVG = 0, hb = 0, dolarHaftasonu = 0, dolarHizmet = 0, dolarToplamG = 0, dolarKDV8 = 0
                    , dolarKDV18 = 0, dolarKDVG = 0, tlGelir = 0, tlKar = 0;

                kurumadi = teklif.FirmaId > 0 ? _musteriServisi.FirmaAlId(teklif.FirmaId).Adı : "";

                kurEuro = teklif.KurEuro > 0 ? teklif.KurEuro : 1;
                kurDolar = teklif.KurDolar > 0 ? teklif.KurDolar : 1;
                DateTime kurTarih = DateTime.Now;
                var teklif2 = _teklifServisi.TeklifAlId(teklif.OrijinalTeklifId);

                bool yurtdisi = false;
                ilgilikisi = teklif.YetkiliId > 0 ? _yetkiliServisi.YetkiliAlId(teklif.YetkiliId).Adı + " " + _yetkiliServisi.YetkiliAlId(teklif.YetkiliId).Soyadı : "";
                //kurumaciklama = _musteriServisi.MusteriAlId(teklif.)
                konum = teklif.Konum;
                kodno = teklif.Kod;
                if (teklif.UlkeId != 1)
                    yurtdisi = true;

                hazırlayan = teklif.HazırlayanId > 0 ? _kullanıcıServisi.KullanıcıAlId(teklif.HazırlayanId).TamAdAl() : "";

                var logoResmi = _resimServisi.ResimAlId(pdfAyarları.LogoResimId);
                var logoPath = _resimServisi.ThumbYoluAl(logoResmi, 0, false);
                var logoMevcut = logoResmi != null;
                string resim = "<img src=\"" + logoPath + "\" />", parabirimi;

                
                //var doc = new Document(sayfaBüyüklüğü);

                /* StyleSheet styles = new StyleSheet();
                FontFactory.Register(GenelYardımcı.MapPath("~/App_Data/Pdf/arial.ttf"), "Garamond");   // just give a path of arial.ttf 
                styles.LoadTagStyle("body", "face", "Garamond");
                styles.LoadTagStyle("body", "encoding", "Identity-H");*/
                int teklifCount = teklifler.Count;

                sb.Append("<table border = '1' bgcolor=\"#3E6BE5\" style=\"font-family:Arial; font-size:12px; color:#fff;\"><tr><th colspan=\"4\">Teklif</th><th colspan=\"2\">Alış Fiyatı</th><th colspan=\"2\">Satış Fiyat</th><th colspan=\"2\">Kar/Yüzde Oranı</th><th colspan=\"3\"></th></tr><tr><th>Aciklama</th><th>KURUM</th><th>KİŞİ/ADET</th><th>GÜN</th><th>BİRİM FİYAT</th><th>TUTAR</th><th>BİRİM FİYAT</th><th>TUTAR</th><th>KAR</th><th>GELIR</th><th>EURO TOPLAM</th><th>DOLAR TOPLAM</th><th>TL TOPLAM</th></tr></table>");
                sb.Append("<table border = '1' style=\"font-family:Arial; font-size:12px;\">");
                decimal alist = 0, satist = 0, kart = 0, gelirt = 0, geliryuzde = 0, alistk = 0, satistk = 0, kartk = 0, gelirtk = 0, geliryuzdek = 0;

                int count = _bagliTeklifOgesi.BagliTeklifOgesiAlTeklifId(teklif.Id).Count;
                //int hb = 0;
                if (teklif.UlkeId != 1)
                    yurtdisi = true;
                List<int> parabirimleri = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    var ogeler = _bagliTeklifOgesi.BagliTeklifOgesiAlTeklifId(teklif.Id);
                    var oge = ogeler[i];
                    BagliTeklifOgesi oge2;
                    if (count > i + 1)
                        oge2 = ogeler[i + 1];
                    else
                        oge2 = ogeler[i];

                    decimal tutar = oge.SatisFiyat;
                    decimal alis = oge.AlisFiyat;
                    decimal gelir = Convert.ToDecimal(oge.Gelir);
                    decimal kar = oge.Kar;
                    int parabirimiDeger = oge.Parabirimi;
                    parabirimi = parabirimiDeger == 1 ? "TL" : parabirimiDeger == 2 ? "$" : "€";
                    int konaklama = 0;

                    if (parabirimleri.Count == 0)
                        parabirimleri.Add(Convert.ToInt32(parabirimiDeger));

                    for (int j = 0; j < parabirimleri.Count; j++)
                    {
                        int p = Convert.ToInt32(parabirimiDeger);
                        if (parabirimleri.Contains(p))
                            break;
                        else
                            parabirimleri.Add(p);
                    }

                    if (oge.Kdv == 8)
                        konaklama = 1;
                    else
                        konaklama = 0;
                    if (parabirimiDeger == 1)
                    {
                        sparabirimi = "TL";
                        tlTutar = tutar;
                        euroTutar = Convert.ToDecimal((tutar / kurEuro));
                        dolarTutar = Convert.ToDecimal((tutar / kurDolar));
                        tlAlis = alis;
                        tlGelir = gelir;
                        tlKar = kar;
                    }
                    if (parabirimiDeger == 2)
                    {
                        sparabirimi = "$";
                        tlTutar = Convert.ToDecimal((tutar * kurDolar));
                        euroTutar = Convert.ToDecimal((tlTutar / kurEuro));
                        dolarTutar = tutar;
                        tlAlis = Convert.ToDecimal((alis * kurDolar));
                        tlGelir = Convert.ToDecimal((gelir * kurDolar));
                        tlKar = Convert.ToDecimal((kar * kurDolar));
                    }
                    if (parabirimiDeger == 3)
                    {
                        sparabirimi = "€";
                        tlTutar = Convert.ToDecimal((tutar * kurEuro));
                        euroTutar = tutar;
                        dolarTutar = Convert.ToDecimal((tutar / kurDolar));
                        tlAlis = Convert.ToDecimal((alis * kurEuro));
                        tlGelir = Convert.ToDecimal((gelir * kurEuro));
                        tlKar = Convert.ToDecimal((kar * kurEuro));
                    }
                    if (konaklama == 1)
                    {
                        alistk += tlAlis;
                        satistk += tlTutar;
                        gelirtk += tlGelir;
                        kartk += tlKar;
                        geliryuzdek = satistk > 0 ? kartk / satistk * 100 : 0;
                        geliryuzdek = Math.Round(geliryuzdek, 2);
                        tlToplamK += tlTutar;
                        dolarToplamK += dolarTutar;
                        euroToplamK += euroTutar;
                    }
                    else
                    {
                        alist += tlAlis;
                        satist += tlTutar;
                        gelirt += tlGelir;
                        kart += tlKar;
                        geliryuzde = satist > 0 ? kart / satist * 100 : 0;
                        geliryuzde = Math.Round(geliryuzde, 2);
                        tlToplam += tlTutar;
                        dolarToplam += dolarTutar;
                        euroToplam += euroTutar;
                    }
                    if (oge.Aciklama != string.Empty || oge.Aciklama != "")
                        aciklama = " (" + oge.Aciklama + ") ";
                    else
                        aciklama = "";
                    if (i == 0)
                    {
                        sb.Append("<tr><th colspan=\"13\"  bgcolor=\"#3E6BE5\" style=\"color:#fff;\">" + oge.Tparent + aciklama + "</th></tr>");
                    }
                    tlTutar = Math.Round(tlTutar, 2);
                    dolarTutar = Math.Round(dolarTutar, 2);
                    euroTutar = Math.Round(euroTutar, 2);

                    int alisadet = oge.AlisAdet;
                    int satisadet = oge.Adet;
                    int adet = alisadet > 0 ? alisadet : satisadet;

                    sb.Append("<tr><td>" + oge.Adı + "</td><td>" + oge.Kurum + "</td><td>" + adet + "</td><td>" + oge.Gun + "</td><td>" + oge.AlisBirimFiyat + sparabirimi + "</td><td>" + oge.AlisFiyat + sparabirimi + "</td><td>" + oge.SatisBirimFiyat + sparabirimi + "</td><td>" + oge.SatisFiyat + sparabirimi + "</td><td>" + oge.Kar + sparabirimi + "</td><td>" + oge.Gelir + "%" + "</td><td>" + euroTutar.ToString("0,0.00", ci) + "€" + "</td><td>" + dolarTutar.ToString("0,0.00", ci) + "$" + "</td><td>" + tlTutar.ToString("0,0.00", ci) + "TL" + "</td></tr>");
                    if (oge.Vparent != oge2.Vparent)
                    {
                        sb.Append("<tr><th colspan=\"13\"  bgcolor=\"#3E6BE5\" style=\"color:#fff;\">" + oge2.Tparent + aciklama + "</th></tr>");
                    }

                }
                if (teklif.HizmetBedeli != 0 || teklif.HizmetBedeli.ToString() != "")
                    hb = Convert.ToDecimal(teklif.HizmetBedeli);

                euroHaftasonu = euroToplamK + euroToplam;
                dolarHaftasonu = dolarToplamK + dolarToplam;
                tlHaftasonu = tlToplamK + tlToplam;
                euroHizmet = hb > 0 ? (euroToplamK + euroToplam) / 100 * hb : (euroToplamK + euroToplam) / 100;
                dolarHizmet = hb > 0 ? (dolarToplamK + dolarToplam) / 100 * hb : (dolarToplamK + dolarToplam) / 100;
                tlHizmet = hb > 0 ? (tlToplamK + tlToplam) / 100 * hb : (tlToplamK + tlToplam) / 100;
                euroToplamG = euroHaftasonu + euroHizmet;
                dolarToplamG = dolarHaftasonu + dolarHizmet;
                tlToplamG = tlHaftasonu + tlHizmet;
                tlKDV8 = yurtdisi ? 0 : tlToplamK * 8 / 100;
                euroKDV8 = yurtdisi ? 0 : euroToplamK * 8 / 100;
                dolarKDV8 = yurtdisi ? 0 : dolarToplamK * 8 / 100;
                tlKDV18 = yurtdisi ? 0 : tlToplam * 18 / 100 + (tlHizmet * 18 / 100);
                euroKDV18 = yurtdisi ? 0 : euroToplam * 18 / 100 + (euroHizmet * 18 / 100);
                dolarKDV18 = yurtdisi ? 0 : dolarToplam * 18 / 100 + (dolarHizmet * 18 / 100);
                tlKDVG = yurtdisi ? 0 : tlToplamG + tlKDV8 + tlKDV18;
                euroKDVG = yurtdisi ? 0 : euroToplamG + euroKDV8 + euroKDV18;
                dolarKDVG = yurtdisi ? 0 : dolarToplamG + dolarKDV8 + dolarKDV18;

                tlToplam = Math.Round(tlToplam, 2);
                dolarToplam = Math.Round(dolarToplam, 2);
                euroToplam = Math.Round(euroToplam, 2);
                tlToplamK = Math.Round(tlToplamK, 2);
                dolarToplamK = Math.Round(dolarToplamK, 2);
                euroToplamK = Math.Round(euroToplamK, 2);
                euroToplamG = Math.Round(euroToplamG, 2);
                dolarToplamG = Math.Round(dolarToplamG, 2);
                tlToplamG = Math.Round(tlToplamG, 2);
                euroHizmet = Math.Round(euroHizmet, 2);
                dolarHizmet = Math.Round(dolarHizmet, 2);
                tlHizmet = Math.Round(tlHizmet, 2);
                euroHaftasonu = Math.Round(euroHaftasonu, 2);
                dolarHaftasonu = Math.Round(dolarHaftasonu, 2);
                tlHaftasonu = Math.Round(tlHaftasonu, 2);
                tlKDV8 = Math.Round(tlKDV8, 2);
                euroKDV8 = Math.Round(euroKDV8, 2);
                dolarKDV8 = Math.Round(dolarKDV8, 2);
                tlKDV18 = Math.Round(tlKDV18, 2);
                euroKDV18 = Math.Round(euroKDV18, 2);
                dolarKDV18 = Math.Round(dolarKDV18, 2);
                tlKDVG = Math.Round(tlKDVG, 2);
                euroKDVG = Math.Round(euroKDVG, 2);
                dolarKDVG = Math.Round(dolarKDVG, 2);

                sb.Append("</table>");
                sb.Append("<table border = '1' style=\"font-family:Arial; font-size:12px;\"><tr><th colspan=\"13\"> </th></tr><tr><td colspan=\"2\">Konaklama Toplamı</td><td colspan=\"3\"><td>" + alistk.ToString("0,0.00", ci) + sparabirimi + "</td><td></td><td>" + satistk.ToString("0,0.00", ci) + sparabirimi + "</td><td>" + kartk.ToString("0,0.00", ci) + sparabirimi + "</td><td>" + geliryuzdek + "%" + "</td><td>" + euroToplamK.ToString("0,0.00", ci) + "€" + "</td><td>" + dolarToplamK.ToString("0,0.00", ci) + "$" + "</td><td>" + tlToplamK.ToString("0,0.00", ci) + "TL" + "</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Diğer Toplam</td><td colspan=\"3\"><td>" + alist.ToString("0,0.00", ci) + sparabirimi + "</td><td></td><td>" + satist.ToString("0,0.00", ci) + sparabirimi + "</td><td>" + kart.ToString("0,0.00", ci) + sparabirimi + "</td><td>" + geliryuzde + "%" + "</td><td>" + euroToplam.ToString("0,0.00", ci) + "€" + "</td><td>" + dolarToplam.ToString("0,0.00", ci) + "$" + "</td><td>" + tlToplam.ToString("0,0.00", ci) + "TL" + "</td></tr>");
                sb.Append("<tr><td colspan=\"2\">Toplam (Hizmet Dahil)</td><td colspan=\"3\"><td>" + (alist + alistk).ToString("0,0.00", ci) + "TL" + "</td><td></td><td>" + (satist + satistk + tlHizmet).ToString("0,0.00", ci) + "TL" + "</td><td>" + (kart + kartk + tlHizmet).ToString("0,0.00", ci) + "TL" + "</td><td>" + ((satist + satistk + tlHizmet) > 0 ? (kart + kartk + tlHizmet) / (satist + satistk + tlHizmet) * 100 : 0).ToString("0,0.00", ci) + "%" + "</td><td>" + euroHaftasonu.ToString("0,0.00", ci) + "€" + "</td><td>" + dolarHaftasonu.ToString("0,0.00", ci) + "$" + "</td><td>" + tlHaftasonu.ToString("0,0.00", ci) + "TL" + "</td></tr></table>");
                sb.Append("<table border = '1' bgcolor=\"#3E6BE5\"  style=\"font-family:Arial; font-size:12px; color:#fff;\"><tr><th colspan=\"13\"></th></tr><tr><td colspan=\"13\" ></td></tr><tr><td colspan=\"2\">Toplam</td><td colspan=\"8\" ></td><td>" + euroHaftasonu.ToString("0,0.00", ci) + "€" + "</td><td>" + dolarHaftasonu.ToString("0,0.00", ci) + "$" + "</td><td>" + tlHaftasonu.ToString("0,0.00", ci) + "TL" + "</td></tr><tr><td colspan=\"2\">Hizmet Bedeli " + hb.ToString() + "%</td><td colspan=\"8\" ></td><td>" + euroHizmet.ToString("0,0.00", ci) + "€" + "</td><td>" + dolarHizmet.ToString("0,0.00", ci) + "$" + "</td><td>" + tlHizmet.ToString("0,0.00", ci) + "TL" + "</td></tr><tr><td colspan=\"2\">Genel Toplam</td><td colspan=\"8\" ></td><td>" + euroToplamG.ToString("0,0.00", ci) + "€" + "</td><td>" + dolarToplamG.ToString("0,0.00", ci) + "$" + "</td><td>" + tlToplamG.ToString("0,0.00", ci) + "TL" + "</td></tr><tr><td colspan=\"2\">KDV %8</td><td colspan=\"8\" ></td><td>" + euroKDV8.ToString("0,0.00", ci) + "€" + "</td><td>" + dolarKDV8.ToString("0,0.00", ci) + "$" + "</td><td>" + tlKDV8.ToString("0,0.00", ci) + "TL" + "</td></tr><tr><td colspan=\"2\">KDV %18</td><td colspan=\"8\" ></td><td>" + euroKDV18.ToString("0,0.00", ci) + "€" + "</td><td>" + dolarKDV18.ToString("0,0.00", ci) + "$" + "</td><td>" + tlKDV18.ToString("0,0.00", ci) + "TL" + "</td></tr><tr><td colspan=\"2\">KDV'li Genel Toplam</td><td colspan=\"8\" ></td><td>" + euroKDVG.ToString("0,0.00", ci) + "€" + "</td><td>" + dolarKDVG.ToString("0,0.00", ci) + "$" + "</td><td>" + tlKDVG.ToString("0,0.00", ci) + "TL" + "</td></tr></table>");
                sb.Append("<table border = '1'  bgcolor=\"#30e5d3\" style=\"font-family:Arial; font-size:12px;\"><tr><th align=\"center\">" + kurTarih.Date.ToShortDateString() + " tarihindeki TCMB efektif satış döviz kurları baz alınarak hesaplanmıştır.<br>1 Euro/" + (kurEuro).ToString("N04", ci) + " TL, 1 Dolar/" + (kurDolar).ToString("N04", ci) + " TL <br></th></tr></table>");
                sb.Append("<br />");
                sb.Append("<br />");
                alist = 0; satist = 0; kart = 0; gelirt = 0; geliryuzde = 0;
                alistk = 0; satistk = 0; kartk = 0; gelirtk = 0; geliryuzdek = 0;
                tlTutar = 0; tlAlis = 0; euroTutar = 0; dolarTutar = 0; tlToplam = 0;
                dolarToplam = 0; euroToplam = 0; tlToplamK = 0; dolarToplamK = 0;
                euroToplamK = 0; euroToplamG = 0; tlToplamG = 0; euroHizmet = 0;
                tlHizmet = 0; euroHaftasonu = 0; tlHaftasonu = 0; tlKDV8 = 0;
                euroKDV8 = 0; tlKDV18 = 0; euroKDV18 = 0; tlKDVG = 0;
                euroKDVG = 0; hb = 0; dolarHaftasonu = 0; dolarHizmet = 0;
                dolarToplamG = 0; dolarKDV8 = 0; dolarKDV18 = 0; dolarKDVG = 0;
                tlKar = 0; tlGelir = 0; tlAlis = 0;

                StringReader sr = new StringReader(sb.ToString());

                
            }
            return sb;
        }
    }
}
