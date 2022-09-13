using System.Web.Mvc;
using System;
using Services.Medya;
using System.IO;
using System.Web;
using Core;
using Web.Framework.Güvenlik;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Services.Testler;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using Services.Kongre;
using Services.Notlar;
using Core.Domain.Kongre;
using Services.Tanımlamalar;

namespace Web.Controllers
{
    public partial class XlsController : TemelPublicController
    {
        private readonly IXlsUploadServisi _xlsService;
        private readonly ITestServisi _testServisi;
        private readonly IKatilimciServisi _katilimciServisi;
        private readonly INotServisi _notServisi;
        private readonly IFirmaServisi _musteriServisi;
        private readonly IKayitServisi _kayitServisi;
        private readonly IKonaklamaServisi _konaklamaServisi;

        public XlsController(IXlsUploadServisi xlsService,
            ITestServisi testServisi,
            IKatilimciServisi katilimciServisi,
            INotServisi notServisi,
            IFirmaServisi musteriServisi,
            IKayitServisi kayitServisi,
            IKonaklamaServisi konaklamaServisi)
        {
            this._xlsService = xlsService;
            this._testServisi = testServisi;
            this._katilimciServisi = katilimciServisi;
            this._notServisi = notServisi;
            this._musteriServisi = musteriServisi;
            this._kayitServisi = kayitServisi;
            this._konaklamaServisi = konaklamaServisi;

        }

        [HttpPost]
        [AntiForgery(true)]
        public virtual ActionResult AsyncUpload()
        {
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            List<string> data = new List<string>();
            DataTable dt = new DataTable();
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
                if (contentType == "application/vnd.ms-excel" || contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = httpPostedFile.FileName;
                    string targetpath = Server.MapPath("~/Doc/");
                    httpPostedFile.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    using (SpreadsheetDocument doc = SpreadsheetDocument.Open(pathToExcelFile, false))
                    {
                        Sheet sheet = doc.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                        Worksheet worksheet = (doc.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;
                        IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();
                       // DataTable dt = new DataTable();
                        uint referans = 1;
                        int cnt = 0;
                        foreach (Row row in rows)
                        {
                            foreach (Cell cell in row.Descendants<Cell>())
                            {
                                string val = GetValue(doc, cell);
                                val = val.ToLower();
                                val = val.Replace(" ", String.Empty);
                                if (val != "isim" && val != "ad" && val != "adı")
                                {
                                    continue;
                                }
                                else
                                {
                                    referans = row.RowIndex.Value;
                                }
                            }
                        }

                        foreach (Row row in rows)
                        {
                            if (row.RowIndex.Value == referans)
                            {
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    dt.Columns.Add(GetValue(doc, cell));
                                }
                            }
                            else
                            {
                                //Add rows to DataTable.
                                if (dt.Columns.Count > 0)
                                {
                                    dt.Rows.Add();
                                    int i = 0;
                                    foreach (Cell cell in row.Descendants<Cell>())
                                    {
                                        dt.Rows[dt.Rows.Count - 1][i] = GetValue(doc, cell);
                                        i++;
                                    }
                                }
                            }
                        }
                    }
                    /*
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    return Json("success", JsonRequestBehavior.AllowGet);
                    */
                }
                else
                {
                }
            }
            else
            {
                //Webkit, Mozilla
                stream = Request.InputStream;
                fileName = Request["qqfile"];
            }

            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();
            if (String.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".xlsx":
                        contentType = MimeTipleri.TextXlsx;
                        break;
                    case ".xls":
                    default:
                        break;
                }
            }
            var xls = _xlsService.XlsEkle(fileBinary, contentType, null);

            for(int i = 0; i < dt.Rows.Count; i++)
            {

                #region sponsorlar
                string kayitspo = dt.Rows[i][12].ToString();
                string koankspo = dt.Rows[i][27].ToString();
                string trasnspo = dt.Rows[i][41].ToString();
                int kayitspoId = 0;
                int konakspoId = 0;
                int transspoId = 0;
                if (kayitspo == "DERNEK")
                    kayitspoId = -1;
                if (kayitspo == "MÜNFERİT")
                    kayitspoId = -2;
                foreach (var m in _musteriServisi.TümFirmaAl())
                {
                    if (m.Adı == kayitspo)
                        kayitspoId = m.Id;
                    if (m.Adı == koankspo)
                        konakspoId = m.Id;
                    if (m.Adı == kayitspo)
                        transspoId = m.Id;
                }
                #endregion
                #region kayit
                int kayittipiId = 0;
                string kayittipi = dt.Rows[i][14].ToString();
                foreach (var m in _kayitServisi.TümKayitAl())
                {
                    if (m.KayıtTipi == kayittipi.Trim() && dt.Rows[i][13].ToString().Trim() == m.KayıtUcreti)
                        kayittipiId = m.Id;
                    if (kayitspoId == -1 && (dt.Rows[i][13].ToString().Trim() == "0" || dt.Rows[i][13].ToString().Trim() == ""))
                        kayittipiId = 27;
                }
                #endregion
                #region konaklama
                int refakatci = 0;
                int konaklamatipiId = 0;
                string konaklamatipi= dt.Rows[i][31].ToString();
                string konaklamabedeli= dt.Rows[i][32].ToString();
                int kontenjan = 1;
                if (konaklamatipi.Trim() == "DBL")
                    kontenjan = 2;
                if (konaklamatipi.Trim() == "1")
                {
                    kontenjan = 0;
                    refakatci = 1;
                }
                    
                foreach (var m in _konaklamaServisi.TümKonaklamaAl())
                {
                    if (m.OtelKontenjanı == kontenjan && konaklamabedeli.Trim() == m.OtelUcreti)
                        konaklamatipiId = m.Id;
                }
                string gTarihi = String.IsNullOrEmpty(dt.Rows[i][29].ToString()) ? DateTime.Now.ToShortTimeString() : DateTime.FromOADate(Convert.ToDouble(dt.Rows[i][29])).ToShortDateString(); 
                string cTarihi = String.IsNullOrEmpty(dt.Rows[i][30].ToString()) ? DateTime.Now.ToShortTimeString() : DateTime.FromOADate(Convert.ToDouble(dt.Rows[i][30])).ToShortDateString();
                #endregion

                Katilimci k = new Katilimci
                {
                    KongreId = 19,
                    Email = dt.Rows[i][6].ToString(),
                    Tel = dt.Rows[i][7].ToString(),
                    TCKN = dt.Rows[i][8].ToString(),
                    Adı = dt.Rows[i][9].ToString(),
                    Soyadı = dt.Rows[i][10].ToString(),
                    OtelGiris = Convert.ToDateTime(gTarihi),
                    OtelCikis = Convert.ToDateTime(cTarihi),
                    UlasimVarisTarihi = Convert.ToDateTime(gTarihi),
                    UlasimKalkisTarihi = Convert.ToDateTime(gTarihi),
                    TransferTarihi = Convert.ToDateTime(gTarihi),
                    KayıtSponsorId = kayitspoId,
                    KonaklamaSponsorId = konakspoId,
                    TransferSponsorId = transspoId,
                    KayıtId = kayittipiId,
                    KonaklamaId = konaklamatipiId,
                    Refakatci = refakatci

                };
                
                 _katilimciServisi.KatilimciEkle(k);
            }
            return Json(new { success = true, data = JsonConvert.SerializeObject(dt) }, MimeTipleri.TextPlain);
        }

        private string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = "";
            if (cell.DataType == null) // number & dates
            {
                int styleIndex = (int)cell.StyleIndex.Value;
                CellFormat cellFormat = (CellFormat)doc.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats.ElementAt(styleIndex);
                uint formatId = cellFormat.NumberFormatId.Value;

                if (formatId == (uint)Formats.DateShort || formatId == (uint)Formats.DateLong)
                {
                    double oaDate;
                    if (double.TryParse(cell.InnerText, out oaDate))
                    {
                        value = DateTime.FromOADate(oaDate).ToShortDateString();
                    }
                }
                else
                {
                    value = cell.InnerText;
                }
            }
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                value = cell.CellValue.InnerText;
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }
        private enum Formats
        {
            General = 0,
            Number = 1,
            Decimal = 2,
            Currency = 164,
            Accounting = 44,
            DateShort = 168,
            DateLong = 165,
            Time = 166,
            Percentage = 10,
            Fraction = 12,
            Scientific = 11,
            Text = 49
        }
    }
}