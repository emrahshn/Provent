using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Html.CodeFormatter
{
    public partial class HtmlFormat : SourceFormat
    {
        private readonly CSharpFormat csf; //to format embedded C# code
        private readonly JavaScriptFormat jsf; //to format client-side JavaScript code
        private readonly Regex attribRegex;

        public HtmlFormat()
        {
            const string regJavaScript = @"(?<=&lt;script(?:\s.*?)?&gt;).+?(?=&lt;/script&gt;)";
            const string regComment = @"&lt;!--.*?--&gt;";
            const string regAspTag = @"&lt;%@.*?%&gt;|&lt;%|%&gt;";
            const string regAspCode = @"(?<=&lt;%).*?(?=%&gt;)";
            const string regTagDelimiter = @"(?:&lt;/?!?\??(?!%)|(?<!%)/?&gt;)+";
            const string regTagName = @"(?<=&lt;/?!?\??(?!%))[\w\.:-]+(?=.*&gt;)";
            const string regAttributes = @"(?<=&lt;(?!%)/?!?\??[\w:-]+).*?(?=(?<!%)/?&gt;)";
            const string regEntity = @"&amp;\w+;";
            const string regAttributeMatch = @"(=?"".*?""|=?'.*?')|([\w:-]+)";

            //the regex object will handle all the replacements in one pass
            string regAll = "(" + regJavaScript + ")|(" + regComment + ")|("
                + regAspTag + ")|(" + regAspCode + ")|("
                + regTagDelimiter + ")|(" + regTagName + ")|("
                + regAttributes + ")|(" + regEntity + ")";

            CodeRegex = new Regex(regAll, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            attribRegex = new Regex(regAttributeMatch, RegexOptions.Singleline);

            csf = new CSharpFormat();
            jsf = new JavaScriptFormat();
        }
        private string AttributeMatchEval(Match match)
        {
            if (match.Groups[1].Success) //attribute value
                return "<span class=\"kwrd\">" + match.ToString() + "</span>";

            if (match.Groups[2].Success) //attribute name
                return "<span class=\"attr\">" + match.ToString() + "</span>";

            return match.ToString();
        }
        protected override string MatchEval(Match match)
        {
            if (match.Groups[1].Success) //JavaScript code
            {
                //string s = match.ToString();
                return jsf.FormatSubCode(match.ToString());
            }
            if (match.Groups[2].Success) //comment
            {
                var reader = new StringReader(match.ToString());
                string line;
                var sb = new StringBuilder();
                while ((line = reader.ReadLine()) != null)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("\n");
                    }
                    sb.Append("<span class=\"rem\">");
                    sb.Append(line);
                    sb.Append("</span>");
                }
                return sb.ToString();
            }
            if (match.Groups[3].Success) //asp tag
            {
                return "<span class=\"asp\">" + match.ToString() + "</span>";
            }
            if (match.Groups[4].Success) //asp C# code
            {
                return csf.FormatSubCode(match.ToString());
            }
            if (match.Groups[5].Success) //tag delimiter
            {
                return "<span class=\"kwrd\">" + match.ToString() + "</span>";
            }
            if (match.Groups[6].Success) //html tagname
            {
                return "<span class=\"html\">" + match.ToString() + "</span>";
            }
            if (match.Groups[7].Success) //attributes
            {
                return attribRegex.Replace(match.ToString(),
                    new MatchEvaluator(this.AttributeMatchEval));
            }
            if (match.Groups[8].Success) //entity
            {
                return "<span class=\"attr\">" + match.ToString() + "</span>";
            }
            return match.ToString();
        }
    }
}
