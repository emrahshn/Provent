using System;
using System.Xml;

namespace Core
{
    public static class Uzantılar
    {
        public static bool BoşVeyaVarsayılan<T>(this T? value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }

        public static string ElText(this XmlNode node, string elName)
        {
            return node.SelectSingleNode(elName).InnerText;
        }

        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> değerlendirici, TResult hataDeğeri)
            where TInput : class
        {
            return o == null ? hataDeğeri : değerlendirici(o);
        }
    }
}
