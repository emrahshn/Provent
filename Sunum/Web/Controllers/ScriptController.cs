using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Domain.Teklif;
using Core.Eklentiler;
using Services.Teklifler;
using Web.Models.Teklif;
using Web.Uzantılar;

namespace Web.Controllers
{

    public class ScriptController : TemelPublicController
    {
        private readonly IEklentiBulucu _eklentiBulucu;
        private readonly ITeklifServisi _teklifServisi;
        private readonly IBagliTeklifOgesiServisi _teklifOgesiServisi;
        public ScriptController(IEklentiBulucu eklentiBulucu, 
            ITeklifServisi teklifServisi,
            IBagliTeklifOgesiServisi teklifOgesiServisi)
        {
            this._eklentiBulucu = eklentiBulucu;
            this._teklifServisi = teklifServisi;
            this._teklifOgesiServisi = teklifOgesiServisi;
        }

        public ActionResult Index()
        {
            return View();
        }
        public virtual void TeklifOrijinalId()
        {
            var teklifler = _teklifServisi.TümTeklifAl();
            foreach(var teklif in teklifler)
            {
                Teklif t = teklif;
                t.OrijinalTeklifId = t.Id;
                if(t.Operasyon==1&&t.Konfirme == 1)
                {
                    t.Durumu = "Konfirme";
                }
                if (t.Operasyon == 1 && t.Konfirme == 1&&t.Biten==1)
                {
                    t.Durumu = "Tamamlandı";
                }
                else
                {
                    t.Durumu = "Operasyon";
                }
                t.Operasyon = 1;
                _teklifServisi.TeklifGüncelle(t);
            }
        }
        public virtual void BagliTeklifOgeisDuzenle(int eskiid,int yeniid)
        {
            var teklifOgeleri = _teklifOgesiServisi.BagliTeklifOgesiAlTeklifId(eskiid);
            foreach (var teklifogesi in teklifOgeleri)
            {
                BagliTeklifOgesi t = teklifogesi;
                t.TeklifId = yeniid;
                _teklifOgesiServisi.BagliTeklifOgesiGüncelle(t);
            }
        }
    }
}
