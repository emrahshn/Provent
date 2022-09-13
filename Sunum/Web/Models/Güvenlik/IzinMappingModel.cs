using System.Collections.Generic;
using Web.Framework.Mvc;
using Web.Models.Kullanıcılar;

namespace Web.Models.Güvenlik
{
    public partial class IzinMappingModel : TemelTSModel
    {
        public IzinMappingModel()
        {
            MevcutIzinler = new List<IzinKaydıModel>();
            MevcutKullanıcıRolleri = new List<KullanıcıRolModel>();
            Izinli = new Dictionary<string, IDictionary<int, bool>>();
        }
        public IList<IzinKaydıModel> MevcutIzinler { get; set; }
        public IList<KullanıcıRolModel> MevcutKullanıcıRolleri { get; set; }
        public IDictionary<string, IDictionary<int, bool>> Izinli { get; set; }
    }
}