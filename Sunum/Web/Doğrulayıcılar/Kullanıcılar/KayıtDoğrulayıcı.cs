using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Core;
using Core.Domain.Kullanıcılar;
using Web.Framework.Doğrulayıcılar;
using Web.Models.Kullanıcılar;

namespace Web.Doğrulayıcılar.Kullanıcılar
{
    public partial class KayıtDoğrulayıcı : TemelDoğrulayıcı<KayıtModel>
    {
        public KayıtDoğrulayıcı(KullanıcıAyarları kullanıcıAyarları)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email gereklidir.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Yanlış e-posta");

            if (kullanıcıAyarları.EmailİkiDefaGir)
            {
                RuleFor(x => x.EmailOnayla).NotEmpty().WithMessage("Email gereklidir.");
                RuleFor(x => x.EmailOnayla).EmailAddress().WithMessage("Yanlış e-posta");
                RuleFor(x => x.EmailOnayla).Equal(x => x.Email).WithMessage("E-posta ve onay e-postası eşleşmiyor.");
            }

            if (kullanıcıAyarları.KullanıcıAdlarıEtkin)
            {
                RuleFor(x => x.KullanıcıAdı).NotEmpty().WithMessage("Kullanıcı Adı Gereklidir.");
            }

            RuleFor(x => x.Adı).NotEmpty().WithMessage("İsim gereklidir");
            RuleFor(x => x.Soyadı).NotEmpty().WithMessage("Soyisim gereklidir");


            RuleFor(x => x.Şifre).NotEmpty().WithMessage("Parola gereklidir");
            RuleFor(x => x.Şifre).Length(kullanıcıAyarları.MinŞifreUzunluğu, 999).WithMessage(string.Format("Parolanın en az {0} karakter olması gerekir.", kullanıcıAyarları.MinŞifreUzunluğu));
            RuleFor(x => x.ŞifreOnayla).NotEmpty().WithMessage("Şifre gereklidir.");
            RuleFor(x => x.ŞifreOnayla).Equal(x => x.Şifre).WithMessage("Parola ve onay parolası uyuşmuyor.");

            //form fields
            if (kullanıcıAyarları.ÜlkeEtkin && kullanıcıAyarları.ÜlkeGerekli)
            {
                RuleFor(x => x.ÜlkeId)
                    .NotEqual(0)
                    .WithMessage("Ülke gereklidir");
            }
            
            if (kullanıcıAyarları.DoğumTarihiEtkin && kullanıcıAyarları.DoğumTarihiGerekli)
            {
                Custom(x =>
                {
                    var dateOfBirth = x.DoğumTarihiniParçala();
                    if (!dateOfBirth.HasValue)
                    {
                        return new ValidationFailure("DoğumTarihi", "Doğum tarihi gereklidir.");
                    }
                    if (kullanıcıAyarları.DoğumTarihiMinimumYaş.HasValue && GenelYardımcı.YıllarArasındakiFarkıAl(dateOfBirth.Value, DateTime.Today) < kullanıcıAyarları.DoğumTarihiMinimumYaş.Value)
                    {
                        return new ValidationFailure("DateOfBirthDay", "Doğum tarihi aralığı geçersiz.");
                    }
                    return null;
                });
            }
            if (kullanıcıAyarları.ŞirketGerekli && kullanıcıAyarları.ŞirketEtkin)
            {
                RuleFor(x => x.Şirket).NotEmpty().WithMessage("Şirket gereklidir");
            }
            if (kullanıcıAyarları.SokakAdresiEtkin && kullanıcıAyarları.SokakAdresiGerekli)
            {
                RuleFor(x => x.SokakAdresi).NotEmpty().WithMessage("Sokak adresi gereklidir");
            }
            if (kullanıcıAyarları.SokakAdresi2Etkin && kullanıcıAyarları.SokakAdresi2Gerekli)
            {
                RuleFor(x => x.SokakAdresi2).NotEmpty().WithMessage("Sokak adresi 2 gereklidir");
            }
            if (kullanıcıAyarları.PostaKoduEtkin && kullanıcıAyarları.PostaKoduGerekli)
            {
                RuleFor(x => x.PostaKodu).NotEmpty().WithMessage("Posta kodu gereklidir");
            }
            if (kullanıcıAyarları.ŞehirEtkin && kullanıcıAyarları.ŞehirGerekli)
            {
                RuleFor(x => x.Şehir).NotEmpty().WithMessage("Şehir gereklidir");
            }
            if (kullanıcıAyarları.TelEtkin && kullanıcıAyarları.TelGerekli)
            {
                RuleFor(x => x.Tel).NotEmpty().WithMessage("Telefon gereklidir");
            }
            if (kullanıcıAyarları.FaksEtkin && kullanıcıAyarları.FaksGerekli)
            {
                RuleFor(x => x.Faks).NotEmpty().WithMessage("Faks gereklidir");
            }
        }
    }
}