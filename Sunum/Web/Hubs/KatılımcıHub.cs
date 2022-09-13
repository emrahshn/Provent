using Core;
using Core.Domain.Kongre;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Services.Kongre;
using Services.Notlar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Web.Framework.Kendoui;
using Web.Models;
using Web.Models.Kongre;
using Web.Uzantılar;

namespace Web.Hubs
{
    [HubName("katılımcıHub")]
    public class KatılımcıHub : Hub
    {
        private static readonly Dictionary<string, SignalRClient> HubClients = new Dictionary<string, SignalRClient>();
        private readonly IKatilimciServisi _katilimciServisi;
        private readonly IWorkContext _workContext;
        private readonly INotServisi _notServisi;
        
        public KatılımcıHub(IKatilimciServisi katilimciServisi,
            IWorkContext workContext,
            INotServisi notServisi)
        {
            this._katilimciServisi = katilimciServisi;
            this._workContext = workContext;
            this._notServisi = notServisi;
        }
       
        public IEnumerable<Katilimci> Read(KatilimciModel model)
        {
            var tümListe = _katilimciServisi.TümKatilimciAl(true);
            if (model.KongreAra.Count > 0)
            {
                foreach(var k in model.KongreAra)
                    tümListe = tümListe.Where(x => x.KongreId == k).ToList();
            }
            if (!String.IsNullOrEmpty(model.AdAra))
                tümListe = tümListe.Where(x => x.Adı.Contains(model.AdAra)).ToList();
            if (!String.IsNullOrEmpty(model.SoyadAra))
                tümListe = tümListe.Where(x => x.Soyadı.Contains(model.SoyadAra)).ToList();
            tümListe = tümListe.OrderByDescending(x => x.Id).ToList();
            /*
            int mevcutKullanıcıId = _workContext.MevcutKullanıcı.Id;
            var data = tümListe.Select(x =>
            {
                var n = x.ToModel();
                if (_notServisi.NotAlId(mevcutKullanıcıId, "Katılımcı", x.Id).Count > 0)
                {
                    foreach (var m in _notServisi.NotAlId(mevcutKullanıcıId, "Katılımcı", x.Id))
                    {
                        n.Notlar.Add(m.ToModel());
                    }
                }
                return n;
            });
            */
            return tümListe.ToList();
        }

        public void Update(KatilimciModel model)
        {
            var Katılımcı = _katilimciServisi.KatilimciAlId(model.Id);
            Katılımcı = model.ToEntity(Katılımcı);
            _katilimciServisi.KatilimciGüncelle(Katılımcı);
            Clients.Others.update(Katılımcı);
        }

        public void Destroy(Katilimci katilimci)
        {
            _katilimciServisi.KatilimciSil(katilimci);
        }

        public void Create(KatilimciModel model)
        {
            model = new KatilimciModel();
            var Katılımcı = model.ToEntity();
            _katilimciServisi.KatilimciEkle(Katılımcı);
            Clients.Others.update(Katılımcı);
        }
        
    }
}