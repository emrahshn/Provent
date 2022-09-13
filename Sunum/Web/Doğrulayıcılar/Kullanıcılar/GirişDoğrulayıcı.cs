using Web.Framework.Doğrulayıcılar;
using Web.Models.Kullanıcılar;
using FluentValidation;
using Core.Domain.Kullanıcılar;

namespace Web.Doğrulayıcılar.Kullanıcılar
{
    public partial class GirişDoğrulayıcı : TemelDoğrulayıcı<GirişModel>
    {
        public GirişDoğrulayıcı(KullanıcıAyarları kullanıcıAyarları)
        {
            if (!kullanıcıAyarları.KullanıcıAdlarıEtkin)
            {
                //email ile giriş yap
                RuleFor(x => x.Email).NotEmpty().WithMessage("Lütfen E-postanızı girin");
                RuleFor(x => x.Email).EmailAddress().WithMessage("Yanlış e-posta");
            }
        }
    }
}