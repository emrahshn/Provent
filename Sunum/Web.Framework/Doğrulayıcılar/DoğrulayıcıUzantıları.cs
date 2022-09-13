using FluentValidation;

namespace Web.Framework.Doğrulayıcılar
{
    public static class DoğrulayıcıUzantıları
    {
        public static IRuleBuilderOptions<T, string> KrediKartı<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new KrediKartıDoğrulayıcı());
        }
        public static IRuleBuilderOptions<T, decimal> Decimal<T>(this IRuleBuilder<T, decimal> ruleBuilder, decimal maxValue)
        {
            return ruleBuilder.SetValidator(new DecimalDoğrulayıcı(maxValue));
        }
    }
}
