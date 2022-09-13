using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Teklif
{
    public partial class BagliTeklifOgesiModel : TemelTSEntityModel
    {
        public BagliTeklifOgesiModel()
        {
            //Kurum = new List<FirmaModel>();
        }
        [DisplayName("TeklifId")]
        [AllowHtml]
        public int TeklifId { get; set; }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Aciklama")]
        [AllowHtml]
        public string Aciklama { get; set; }
        [DisplayName("AlisBirimFiyat")]
        [AllowHtml]
        public decimal AlisBirimFiyat { get; set; }
        [DisplayName("AlisBirimFiyatDolar")]
        [AllowHtml]
        public decimal AlisBirimFiyatDolar { get; set; }
        [DisplayName("AlisBirimFiyatEuro")]
        [AllowHtml]
        public decimal AlisBirimFiyatEuro { get; set; }
        [DisplayName("SatisBirimFiyat")]
        [AllowHtml]
        public decimal SatisBirimFiyat { get; set; }
        [DisplayName("SatisBirimFiyatDolar")]
        [AllowHtml]
        public decimal SatisBirimFiyatDolar { get; set; }
        [DisplayName("SatisBirimFiyatEuro")]
        [AllowHtml]
        public decimal SatisBirimFiyatEuro { get; set; }
        [DisplayName("AlisFiyat")]
        [AllowHtml]
        public decimal AlisFiyat { get; set; }
        [DisplayName("AlisFiyatDolar")]
        [AllowHtml]
        public decimal AlisFiyatDolar { get; set; }
        [DisplayName("AlisFiyatEuro")]
        [AllowHtml]
        public decimal AlisFiyatEuro { get; set; }
        [DisplayName("SatisFiyat")]
        [AllowHtml]
        public decimal SatisFiyat { get; set; }
        [DisplayName("SatisFiyatDolar")]
        [AllowHtml]
        public decimal SatisFiyatDolar { get; set; }
        [DisplayName("SatisFiyatEuro")]
        [AllowHtml]
        public decimal SatisFiyatEuro { get; set; }
        [DisplayName("ToplamFiyat")]
        [AllowHtml]
        public decimal ToplamFiyat { get; set; }
        [DisplayName("ToplamFiyatDolar")]
        [AllowHtml]
        public decimal ToplamFiyatDolar { get; set; }
        [DisplayName("ToplamFiyatEuro")]
        [AllowHtml]
        public decimal ToplamFiyatEuro { get; set; }
        [DisplayName("Kar")]
        [AllowHtml]
        public decimal Kar { get; set; }
        [DisplayName("KarDolar")]
        [AllowHtml]
        public decimal KarDolar { get; set; }
        [DisplayName("KarEuro")]
        [AllowHtml]
        public decimal KarEuro { get; set; }
        [DisplayName("Gelir")]
        [AllowHtml]
        public string Gelir { get; set; }
        [DisplayName("Satış Adet")]
        [AllowHtml]
        public int Adet { get; set; }
        [DisplayName("Alış Adet")]
        [AllowHtml]
        public int AlisAdet { get; set; }
        [DisplayName("Gun")]
        [AllowHtml]
        public int Gun { get; set; }
        [DisplayName("Kdv")]
        [AllowHtml]
        public int Kdv { get; set; }
        [DisplayName("Parabirimi")]
        [AllowHtml]
        public int Parabirimi { get; set; }
        public string ParabirimiDeger { get; set; }
        [DisplayName("Vparent")]
        [AllowHtml]
        public int Vparent { get; set; }
        [DisplayName("Tparent")]
        [AllowHtml]
        public string Tparent { get; set; }
        [DisplayName("Kurum")]
        [AllowHtml]
        public string Kurum { get; set; }
        public int treeItemId { get; set; }
        
        //public IList<FirmaModel> Kurum { get; set; }
    }
}