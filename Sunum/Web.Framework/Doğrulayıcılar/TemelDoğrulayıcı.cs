using System.Linq;
using System.Linq.Dynamic;
using FluentValidation;
using Core.Altyapı;
using Data;
//using Services.Localization;

namespace Web.Framework.Doğrulayıcılar
{
    public abstract class TemelDoğrulayıcı<T> : AbstractValidator<T> where T : class
    {
        protected TemelDoğrulayıcı()
        {
            PostInitialize();
        }
        protected virtual void PostInitialize()
        {

        }
        protected virtual void VeritabanıDoğrulamaKuralıAyarla<TObject>(IDbContext dbContext, params string[] filterStringPropertyNames)
        {
            MaksimumÖzellikDeğeriAyarla<TObject>(dbContext, filterStringPropertyNames);
            MaksimumDecimalDeğeriAyarla<TObject>(dbContext);
        }
        protected virtual void MaksimumÖzellikDeğeriAyarla<TObject>(IDbContext dbContext, params string[] filterPropertyNames)
        {
            if (dbContext == null)
                return;

            var dbObjectType = typeof(TObject);

            var names = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string) && !filterPropertyNames.Contains(p.Name))
                .Select(p => p.Name).ToArray();

            var maxLength = dbContext.GetColumnsMaxLength(dbObjectType.Name, names);
            var expression = maxLength.Keys.ToDictionary(name => name, name => DynamicExpression.ParseLambda<T, string>(name, null));

            foreach (var expr in expression)
            {
                RuleFor(expr.Value).Length(0, maxLength[expr.Key]);
            }
        }
        protected virtual void MaksimumDecimalDeğeriAyarla<TObject>(IDbContext dbContext)
        {

            if (dbContext == null)
                return;

            var dbObjectType = typeof(TObject);

            var names = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(decimal))
                .Select(p => p.Name).ToArray();

            var maxValues = dbContext.GetDecimalMaxValue(dbObjectType.Name, names);
            var expression = maxValues.Keys.ToDictionary(name => name, name => DynamicExpression.ParseLambda<T, decimal>(name, null));

            foreach (var expr in expression)
            {
                RuleFor(expr.Value).Decimal(maxValues[expr.Key]).WithMessage(string.Format("Değer aralık dışında. Maksimum değer {0} .99'dır.", maxValues[expr.Key] - 1));
            }
        }
    }
}
