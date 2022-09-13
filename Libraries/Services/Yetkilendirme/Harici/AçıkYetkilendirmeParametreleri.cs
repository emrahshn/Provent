using System;
using System.Collections.Generic;

namespace Services.Yetkilendirme.Harici
{
    [Serializable]
    public abstract partial class AçıkYetkilendirmeParametreleri
    {
        public abstract string SağlayıcıSistemAdı { get; }
        public string HariciTanımlayıcı { get; set; }
        public string HariciGörünümTanımlayıcı { get; set; }
        public string OAuthToken { get; set; }
        public string OAuthAccessToken { get; set; }

        public virtual IList<UserClaims> UserClaims
        {
            get { return new List<UserClaims>(0); }
        }
    }
}
