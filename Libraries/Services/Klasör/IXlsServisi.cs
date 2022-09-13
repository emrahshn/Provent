using Core.Domain.Teklif;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Services.Klasör
{
    public partial interface IXlsServisi
    {
        bool CreateExcelDocument(DataSet ds, string excelFilename, bool includeAutoFilter);
        StringBuilder TeklifRaporOlustur(IList<Teklif> teklifler);
    }
}
