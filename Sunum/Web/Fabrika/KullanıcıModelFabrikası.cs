using Core;
using Core.Domain.Kullanıcılar;
using System;
using System.Web.Mvc;
using Web.Models.Kullanıcılar;

namespace Web.Fabrika
{
    public partial class KullanıcıModelFabrikası : IKullanıcıModelFabrikası
    {
        private readonly KullanıcıAyarları _kullanıcıAyarları;
        private readonly IWorkContext _workContext;
        public KullanıcıModelFabrikası(KullanıcıAyarları kullanıcıAyarları,
            IWorkContext workContext)
        {
            this._kullanıcıAyarları = kullanıcıAyarları;
            this._workContext = workContext;
        }

        public virtual GirişModel GirişModelHazırla()
        {
            var model = new GirişModel();
            model.KullanıcıAdlarıEtkin = _kullanıcıAyarları.KullanıcıAdlarıEtkin;
            return model;
        }

        public virtual KayıtModel KayıtModelHazırla(KayıtModel model, bool excludeProperties,
            string overrideCustomCustomerAttributesXml = "", bool setDefaultValues = false)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            /*
            model.AllowCustomersToSetTimeZone = _dateTimeSettings.AllowCustomersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem { Text = tzi.DisplayName, Value = tzi.Id, Selected = (excludeProperties ? tzi.Id == model.TimeZoneId : tzi.Id == _dateTimeHelper.CurrentTimeZone.Id) });
*/
            //form fields
            model.CinsiyetEtkin = _kullanıcıAyarları.CinsiyetEtkin;
            model.DoğumTarihiEtkin = _kullanıcıAyarları.DoğumTarihiEtkin;
            model.DoğumTarihiGerekli = _kullanıcıAyarları.DoğumTarihiGerekli;
            model.ŞirketEtkin = _kullanıcıAyarları.ŞirketEtkin;
            model.ŞirketGerekli = _kullanıcıAyarları.ŞirketGerekli;
            model.SokakAdresiEtkin = _kullanıcıAyarları.SokakAdresiEtkin;
            model.SokakAdresiGerekli = _kullanıcıAyarları.SokakAdresiGerekli;
            model.SokakAdresiEtkin2 = _kullanıcıAyarları.SokakAdresi2Etkin;
            model.SokakAdresiGerekli2 = _kullanıcıAyarları.SokakAdresi2Gerekli;
            model.PostaKoduEtkin = _kullanıcıAyarları.PostaKoduEtkin;
            model.PostaKoduGerekli = _kullanıcıAyarları.PostaKoduGerekli;
            model.ŞehirEtkin = _kullanıcıAyarları.ŞehirEtkin;
            model.ŞehirGerekli = _kullanıcıAyarları.ŞehirGerekli;
            model.ÜlkeEtkin = _kullanıcıAyarları.ÜlkeEtkin;
            model.ÜlkeGerekli = _kullanıcıAyarları.ÜlkeGerekli;
            model.TelEtkin = _kullanıcıAyarları.TelEtkin;
            model.TelGerekli = _kullanıcıAyarları.TelGerekli;
            model.FaksEtkin = _kullanıcıAyarları.FaksEtkin;
            model.FaksGerekli = _kullanıcıAyarları.FaksGerekli;
            model.BültenEtkin = _kullanıcıAyarları.BültenEtkin;
            model.GizlilikSözleşmesiEtkin = _kullanıcıAyarları.GizlilikPolitikasıEtkin;
            model.KullanıcıAdlarıEtkin = _kullanıcıAyarları.KullanıcıAdlarıEtkin;
            model.KullanıcıAdıUygunluğu = _kullanıcıAyarları.KullanıcıAdıUygunluğunuKontrolEt;
            //model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnRegistrationPage;
            model.EmailİkiDefa = _kullanıcıAyarları.EmailİkiDefaGir;
            if (setDefaultValues)
            {
                //varsayılan olarak bülten etkin
                model.Bülten = _kullanıcıAyarları.BültenVarsayılanOlarakTikli;
            }

            //ülke
            if (_kullanıcıAyarları.ÜlkeEtkin)
            {
                model.KullanılabilirÜlkeler.Add(new SelectListItem { Text = "Ülke seçiniz", Value = "0" });
                /*
                foreach (var c in _countryService.GetAllCountries(_workContext.WorkingLanguage.Id))
                {
                    model.AvailableCountries.Add(new SelectListItem
                    {
                        Text = c.GetLocalized(x => x.Name),
                        Value = c.Id.ToString(),
                        Selected = c.Id == model.CountryId
                    });
                }
                */
            }

            //custom customer attributes
            //var customAttributes = PrepareCustomCustomerAttributes(_workContext.CurrentCustomer, overrideCustomCustomerAttributesXml);
            //customAttributes.ForEach(model.CustomerAttributes.Add);

            return model;
        }
        public virtual KullanıcıBilgiModel KullanıcıBilgiModelHazırla(KullanıcıBilgiModel model, Kullanıcı kullanıcı,
            bool excludeProperties, string overrideCustomCustomerAttributesXml = "")
        {
            /*
            if (model == null)
                throw new ArgumentNullException("model");

            if (customer == null)
                throw new ArgumentNullException("customer");

            model.AllowCustomersToSetTimeZone = _dateTimeSettings.AllowCustomersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem { Text = tzi.DisplayName, Value = tzi.Id, Selected = (excludeProperties ? tzi.Id == model.TimeZoneId : tzi.Id == _dateTimeHelper.CurrentTimeZone.Id) });

            if (!excludeProperties)
            {
                model.VatNumber = customer.GetAttribute<string>(SystemCustomerAttributeNames.VatNumber);
                model.FirstName = customer.GetAttribute<string>(SystemCustomerAttributeNames.FirstName);
                model.LastName = customer.GetAttribute<string>(SystemCustomerAttributeNames.LastName);
                model.Gender = customer.GetAttribute<string>(SystemCustomerAttributeNames.Gender);
                var dateOfBirth = customer.GetAttribute<DateTime?>(SystemCustomerAttributeNames.DateOfBirth);
                if (dateOfBirth.HasValue)
                {
                    model.DateOfBirthDay = dateOfBirth.Value.Day;
                    model.DateOfBirthMonth = dateOfBirth.Value.Month;
                    model.DateOfBirthYear = dateOfBirth.Value.Year;
                }
                model.Company = customer.GetAttribute<string>(SystemCustomerAttributeNames.Company);
                model.StreetAddress = customer.GetAttribute<string>(SystemCustomerAttributeNames.StreetAddress);
                model.StreetAddress2 = customer.GetAttribute<string>(SystemCustomerAttributeNames.StreetAddress2);
                model.ZipPostalCode = customer.GetAttribute<string>(SystemCustomerAttributeNames.ZipPostalCode);
                model.City = customer.GetAttribute<string>(SystemCustomerAttributeNames.City);
                model.CountryId = customer.GetAttribute<int>(SystemCustomerAttributeNames.CountryId);
                model.StateProvinceId = customer.GetAttribute<int>(SystemCustomerAttributeNames.StateProvinceId);
                model.Phone = customer.GetAttribute<string>(SystemCustomerAttributeNames.Phone);
                model.Fax = customer.GetAttribute<string>(SystemCustomerAttributeNames.Fax);

                //newsletter
                var newsletter = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, _storeContext.CurrentStore.Id);
                model.Newsletter = newsletter != null && newsletter.Active;

                model.Signature = customer.GetAttribute<string>(SystemCustomerAttributeNames.Signature);

                model.Email = customer.Email;
                model.Username = customer.Username;
            }
            else
            {
                if (_customerSettings.UsernamesEnabled && !_customerSettings.AllowUsersToChangeUsernames)
                    model.Username = customer.Username;
            }

            if (_customerSettings.UserRegistrationType == UserRegistrationType.EmailValidation)
                model.EmailToRevalidate = customer.EmailToRevalidate;

            //countries and states
            if (_customerSettings.CountryEnabled)
            {
                model.AvailableCountries.Add(new SelectListItem { Text = _localizationService.GetResource("Address.SelectCountry"), Value = "0" });
                foreach (var c in _countryService.GetAllCountries(_workContext.WorkingLanguage.Id))
                {
                    model.AvailableCountries.Add(new SelectListItem
                    {
                        Text = c.GetLocalized(x => x.Name),
                        Value = c.Id.ToString(),
                        Selected = c.Id == model.CountryId
                    });
                }

                if (_customerSettings.StateProvinceEnabled)
                {
                    //states
                    var states = _stateProvinceService.GetStateProvincesByCountryId(model.CountryId, _workContext.WorkingLanguage.Id).ToList();
                    if (states.Any())
                    {
                        model.AvailableStates.Add(new SelectListItem { Text = _localizationService.GetResource("Address.SelectState"), Value = "0" });

                        foreach (var s in states)
                        {
                            model.AvailableStates.Add(new SelectListItem { Text = s.GetLocalized(x => x.Name), Value = s.Id.ToString(), Selected = (s.Id == model.StateProvinceId) });
                        }
                    }
                    else
                    {
                        bool anyCountrySelected = model.AvailableCountries.Any(x => x.Selected);

                        model.AvailableStates.Add(new SelectListItem
                        {
                            Text = _localizationService.GetResource(anyCountrySelected ? "Address.OtherNonUS" : "Address.SelectState"),
                            Value = "0"
                        });
                    }

                }
            }
            model.DisplayVatNumber = _taxSettings.EuVatEnabled;
            model.VatNumberStatusNote = ((VatNumberStatus)customer.GetAttribute<int>(SystemCustomerAttributeNames.VatNumberStatusId))
                .GetLocalizedEnum(_localizationService, _workContext);
            model.GenderEnabled = _customerSettings.GenderEnabled;
            model.DateOfBirthEnabled = _customerSettings.DateOfBirthEnabled;
            model.DateOfBirthRequired = _customerSettings.DateOfBirthRequired;
            model.CompanyEnabled = _customerSettings.CompanyEnabled;
            model.CompanyRequired = _customerSettings.CompanyRequired;
            model.StreetAddressEnabled = _customerSettings.StreetAddressEnabled;
            model.StreetAddressRequired = _customerSettings.StreetAddressRequired;
            model.StreetAddress2Enabled = _customerSettings.StreetAddress2Enabled;
            model.StreetAddress2Required = _customerSettings.StreetAddress2Required;
            model.ZipPostalCodeEnabled = _customerSettings.ZipPostalCodeEnabled;
            model.ZipPostalCodeRequired = _customerSettings.ZipPostalCodeRequired;
            model.CityEnabled = _customerSettings.CityEnabled;
            model.CityRequired = _customerSettings.CityRequired;
            model.CountryEnabled = _customerSettings.CountryEnabled;
            model.CountryRequired = _customerSettings.CountryRequired;
            model.StateProvinceEnabled = _customerSettings.StateProvinceEnabled;
            model.StateProvinceRequired = _customerSettings.StateProvinceRequired;
            model.PhoneEnabled = _customerSettings.PhoneEnabled;
            model.PhoneRequired = _customerSettings.PhoneRequired;
            model.FaxEnabled = _customerSettings.FaxEnabled;
            model.FaxRequired = _customerSettings.FaxRequired;
            model.NewsletterEnabled = _customerSettings.NewsletterEnabled;
            model.UsernamesEnabled = _customerSettings.UsernamesEnabled;
            model.AllowUsersToChangeUsernames = _customerSettings.AllowUsersToChangeUsernames;
            model.CheckUsernameAvailabilityEnabled = _customerSettings.CheckUsernameAvailabilityEnabled;
            model.SignatureEnabled = _forumSettings.ForumsEnabled && _forumSettings.SignaturesEnabled;

            //external authentication
            model.NumberOfExternalAuthenticationProviders = _openAuthenticationService
                .LoadActiveExternalAuthenticationMethods(_workContext.CurrentCustomer, _storeContext.CurrentStore.Id).Count;
            foreach (var ear in _openAuthenticationService.GetExternalIdentifiersFor(customer))
            {
                var authMethod = _openAuthenticationService.LoadExternalAuthenticationMethodBySystemName(ear.ProviderSystemName);
                if (authMethod == null || !authMethod.IsMethodActive(_externalAuthenticationSettings))
                    continue;

                model.AssociatedExternalAuthRecords.Add(new CustomerInfoModel.AssociatedExternalAuthModel
                {
                    Id = ear.Id,
                    Email = ear.Email,
                    ExternalIdentifier = ear.ExternalIdentifier,
                    AuthMethodName = authMethod.GetLocalizedFriendlyName(_localizationService, _workContext.WorkingLanguage.Id)
                });
            }

            //custom customer attributes
            var customAttributes = PrepareCustomCustomerAttributes(customer, overrideCustomCustomerAttributesXml);
            customAttributes.ForEach(model.CustomerAttributes.Add);
            */
            return model;
        }
    }

}