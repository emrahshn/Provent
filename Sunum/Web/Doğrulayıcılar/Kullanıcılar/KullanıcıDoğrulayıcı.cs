using Core.Domain.Kullanıcılar;
using Data;
using FluentValidation;
using Services.Kullanıcılar;
using System.Collections.Generic;
using System.Linq;
using Web.Framework.Doğrulayıcılar;
using Web.Models.Kullanıcılar;

namespace Web.Doğrulayıcılar.Kullanıcılar
{
    public partial class KullanıcıDoğrulayıcı : TemelDoğrulayıcı<KullanıcıModel>
    {
        public KullanıcıDoğrulayıcı(IKullanıcıServisi kullanıcıServisi,
            KullanıcıAyarları kullanıcıAyarları,
            IDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Hatalı E-Mail")
                .When(x => IsRegisteredCustomerRoleChecked(x, kullanıcıServisi));

            //form alanı
            if (kullanıcıAyarları.ÜlkeEtkin && kullanıcıAyarları.ÜlkeGerekli)
            {
                RuleFor(x => x.ÜlkeId)
                    .NotEqual(0)
                    .WithMessage("Ülke gerekli")
                    .When(x => IsRegisteredCustomerRoleChecked(x, kullanıcıServisi));
            }
            if (kullanıcıAyarları.ŞirketGerekli && kullanıcıAyarları.ŞirketEtkin)
            {
                RuleFor(x => x.Şirket)
                    .NotEmpty()
                    .WithMessage("Şirket gerekli")
                    .When(x => IsRegisteredCustomerRoleChecked(x, kullanıcıServisi));
            }
            if (kullanıcıAyarları.SokakAdresiEtkin && kullanıcıAyarları.SokakAdresiEtkin)
            {
                RuleFor(x => x.SokakAdresi)
                    .NotEmpty()
                    .WithMessage("Sokak adresi gerekli")
                    .When(x => IsRegisteredCustomerRoleChecked(x, kullanıcıServisi));
            }
            if (kullanıcıAyarları.SokakAdresi2Etkin && kullanıcıAyarları.SokakAdresi2Gerekli)
            {
                RuleFor(x => x.SokakAdresi2)
                    .NotEmpty()
                    .WithMessage("Sokak adresi 2 gerekli")
                    .When(x => IsRegisteredCustomerRoleChecked(x, kullanıcıServisi));
            }
            if (kullanıcıAyarları.PostaKoduEtkin && kullanıcıAyarları.PostaKoduGerekli)
            {
                RuleFor(x => x.PostaKodu)
                    .NotEmpty()
                    .WithMessage("Posta kodu gerekli")
                    .When(x => IsRegisteredCustomerRoleChecked(x, kullanıcıServisi));
            }
            if (kullanıcıAyarları.ŞehirEtkin && kullanıcıAyarları.ŞehirGerekli)
            {
                RuleFor(x => x.Şehir)
                    .NotEmpty()
                    .WithMessage("Şehir gerekli")
                    .When(x => IsRegisteredCustomerRoleChecked(x, kullanıcıServisi));
            }
            if (kullanıcıAyarları.TelEtkin && kullanıcıAyarları.TelGerekli)
            {
                RuleFor(x => x.Tel)
                    .NotEmpty()
                    .WithMessage("Telefon gerekli")
                    .When(x => IsRegisteredCustomerRoleChecked(x, kullanıcıServisi));
            }
            if (kullanıcıAyarları.FaksEtkin && kullanıcıAyarları.FaksGerekli)
            {
                RuleFor(x => x.Faks)
                    .NotEmpty()
                    .WithMessage("Faks gerekli")
                    .When(x => IsRegisteredCustomerRoleChecked(x, kullanıcıServisi));
            }

            VeritabanıDoğrulamaKuralıAyarla<Kullanıcı>(dbContext);
        }

        private bool IsRegisteredCustomerRoleChecked(KullanıcıModel model, IKullanıcıServisi kullanıcıServisi)
        {
            var allCustomerRoles = kullanıcıServisi.TümKullanıcıRolleriniAl(true);
            var newCustomerRoles = new List<KullanıcıRolü>();
            foreach (var customerRole in allCustomerRoles)
                if (model.SeçiliKullanıcıRolIdleri.Contains(customerRole.Id))
                    newCustomerRoles.Add(customerRole);

            bool isInRegisteredRole = newCustomerRoles.FirstOrDefault(cr => cr.SistemAdı == SistemKullanıcıRolAdları.Kayıtlı) != null;
            return isInRegisteredRole;
        }
    }
}