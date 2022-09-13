using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.EkTanımlamalar;
using Web.Models.Kullanıcılar;
using Web.Models.Notlar;

namespace Web.Models.Finans
{
    public partial class OdemeFormuModel : TemelTSEntityModel
    {
        public OdemeFormuModel()
        {
            BolumRol = new List<KullanıcıRolModel>();
            Bankalar = new List<BankaModel>();
            Notlar = new List<NotModel>();
            Agac = new List<TeklifKalemiModel>();
            AgacIdleri = new List<int>();
            KalemTutarAdları = new List<string>();
            KalemTutarPOları = new List<string>();
            KalemTutarları = new List<string>();
            KalemTutarParabirimi = new List<string>();
            Tutarlar = new List<string>();
            TutarParabirimi = new List<string>();
            BelgeTurleri = new List<HariciSektorModel>();
            //TutarParabirimleri = new Parabirimi();
        }
        [DisplayName("Bolum")]
        [AllowHtml]
        public int Bolum { get; set; }
        [DisplayName("Bolüm No")]
        [AllowHtml]
        public int BolumNo { get; set; }
        [DisplayName("Firma")]
        [AllowHtml]
        public string Firma { get; set; }
        [DisplayName("Banka")]
        [AllowHtml]
        public int Banka { get; set; }
        public string BankaAdı { get; set; }
        [DisplayName("Şube Kodu")]
        [AllowHtml]
        public string SubeKodu { get; set; }
        [DisplayName("Hesap No")]
        [AllowHtml]
        public string HesapNo { get; set; }
        [DisplayName("IBAN")]
        [AllowHtml]
        public string IBAN { get; set; }
        [DisplayName("Tutar")]
        [AllowHtml]
        public string Tutar { get; set; }
        [DisplayName("Parabirimi")]
        [AllowHtml]
        public int ParaBirimi { get; set; }
        [DisplayName("Ödeme Şekli")]
        [AllowHtml]
        public int OdemeSekli { get; set; }
        [DisplayName("Ödeme Türü")]
        [AllowHtml]
        public int OdemeTuru { get; set; }
        [DisplayName("Ödeme Tarihi")]
        [AllowHtml]
        public DateTime OdemeTarihi { get; set; }
        [DisplayName("Açıklama")]
        [AllowHtml]
        public string Aciklama { get; set; }
        [DisplayName("İlgili")]
        [AllowHtml]
        public int Ilgili { get; set; }
        [DisplayName("Onay1")]
        [AllowHtml]
        public int Onay1 { get; set; }
        [DisplayName("Onay2")]
        [AllowHtml]
        public int Onay2 { get; set; }
        [DisplayName("Onay3")]
        [AllowHtml]
        public int Onay3 { get; set; }
        [DisplayName("PO")]
        [AllowHtml]
        public string PO { get; set; }
        [DisplayName("Kongre Tarihi")]
        [AllowHtml]
        public DateTime KongreTarihi { get; set; }
        [DisplayName("Kongre Adı")]
        [AllowHtml]
        public string KongreAdı { get; set; }
        public List<KullanıcıRolModel> BolumRol { get; set; }
        public List<BankaModel> Bankalar { get; set; }
        public List<NotModel> Notlar { get; set; }
        public IList<TeklifKalemiModel> Agac { get; set; }
        [DisplayName("Kayıt Kalemi")]
        [AllowHtml]
        [UIHint("MultiSelect")]
        public IList<int> AgacIdleri { get; set; }
        [DisplayName("Kayıt Kalemi Tutarı")]
        [AllowHtml]
        public string KalemTutar { get; set; }
        public string FaturaNo { get; set; }
        public string SatisFaturaNo { get; set; }
        public string TutarGrup { get; set; }
        public string KalemGrup { get; set; }
        public IList<string> KalemTutarAdları { get; set; }
        public IList<string> KalemTutarPOları { get; set; }
        public IList<string> KalemTutarları{ get; set; }
        [DisplayName("Parabirimi")]
        [AllowHtml]
        public IList<string> KalemTutarParabirimi { get; set; }
        public IList<string> Tutarlar { get; set; }
        public IList<string> TutarParabirimi { get; set; }
        public string KalemTutarS { get; set; }
        [DisplayName("Id Ara")]
        public int IdAra { get; set; }

        [DisplayName("Firma Ara")]
        public string FirmaAra { get; set; }
        [DisplayName("Kongre Tarihi Ara")]
        public string KongreGünüAra { get; set; }
        public string KongreAyıAra { get; set; }
        [DisplayName("Ödeme Tarihi Ara")]
        public string OdemeGünüAra { get; set; }
        public string OdemeAyıAra { get; set; }
        [DisplayName("Alış Fatura Ara")]
        public string AlisFaturaAra { get; set; }
        [DisplayName("Satış Fatura Ara")]
        public string SatisFaturaAra { get; set; }
        [DisplayName("Açıklama Fatura Ara")]
        public string AciklamaAra { get; set; }
        [DisplayName("Onay")]
        [AllowHtml]
        public bool Onay { get; set; }
        public int BelgeTuru { get; set; }
        public string BelgeTuruAdı { get; set; }
        public IList<HariciSektorModel> BelgeTurleri { get; set; }
    }

}