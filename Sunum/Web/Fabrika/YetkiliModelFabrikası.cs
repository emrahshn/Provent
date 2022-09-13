using Core.Domain.Tanımlamalar;
using Services.Tanımlamalar;
using System;
using System.Collections.Generic;
using Web.Models.Tanımlamalar;

namespace Web.Fabrika
{
    public class YetkiliModelFabrikası:IYetkiliModelFabrikası
    {
        private readonly IYetkililerServisi _yetkiliServisi;

        public YetkiliModelFabrikası(IYetkililerServisi yetkiliServisi)
        {
            this._yetkiliServisi = yetkiliServisi;
        }
        public virtual IEnumerable<YetkililerModel> YetkiliModeliHazırla(IEnumerable<Yetkililer> yetkililer)
        {
            if (yetkililer == null)
                throw new ArgumentNullException("yetkililer");

            var models = new List<YetkililerModel>();
            foreach (var yetkili in yetkililer)
            {
                var model = new YetkililerModel
                {
                    Id = yetkili.Id,
                    Adı = yetkili.Adı,
                    Soyadı = yetkili.Soyadı,
                    KategoriId = yetkili.KategoriId,
                    CepTel1 = yetkili.CepTel1,
                    CepTel2 = yetkili.CepTel2,
                    Email1 = yetkili.Email1,
                    Email2 = yetkili.Email2,
                    DoğumTarihi = yetkili.DoğumTarihi,
                    Adres = yetkili.Adres,
                    PostaKodu = yetkili.PostaKodu,
                    YSehirId = yetkili.YSehirId,
                    YIlceId = yetkili.YIlceId,
                    UnvanId = yetkili.UnvanId,
                };

                models.Add(model);
            }
            return models;
        }
    }
}