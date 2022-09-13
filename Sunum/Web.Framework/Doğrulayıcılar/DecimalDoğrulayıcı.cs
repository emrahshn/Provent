using Core.Domain.Rehber;
using FluentValidation.Validators;

namespace Web.Framework.Doğrulayıcılar
{
    public class DecimalDoğrulayıcı : PropertyValidator
    {
        private readonly decimal _maxValue;

        protected override bool IsValid(PropertyValidatorContext context)
        {
            decimal value;
            if (decimal.TryParse(context.PropertyValue.ToString(), out value))
            {
                return YuvarlamaYardımcısı.Yuvarla(value,YuvarlamaTipi.Yuvarla05) < _maxValue;
            }
            return false;
        }

        public DecimalDoğrulayıcı(decimal maxValue) :
            base("Decimal değeri sınır dışındaydı")
        {
            this._maxValue = maxValue;
        }
    }
}