using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Html.CodeFormatter
{

    public abstract partial class SourceFormat
    {
        protected SourceFormat()
        {
            _tabSpaces = 4;
            _lineNumbers = false;
            _alternate = false;
            _embedStyleSheet = false;
        }

        private byte _tabSpaces;

        public byte TabSpaces
        {
            get { return _tabSpaces; }
            set { _tabSpaces = value; }
        }

        private bool _lineNumbers;

        public bool LineNumbers
        {
            get { return _lineNumbers; }
            set { _lineNumbers = value; }
        }

        private bool _alternate;

        public bool Alternate
        {
            get { return _alternate; }
            set { _alternate = value; }
        }

        private bool _embedStyleSheet;

        public bool EmbedStyleSheet
        {
            get { return _embedStyleSheet; }
            set { _embedStyleSheet = value; }
        }

        public string FormatCode(Stream source)
        {
            using (var reader = new StreamReader(source))
            {
                string s = reader.ReadToEnd();
                return FormatCode(s, _lineNumbers, _alternate, _embedStyleSheet, false);
            }
        }

        public string FormatCode(string source)
        {
            return FormatCode(source, _lineNumbers, _alternate, _embedStyleSheet, false);
        }

        public string FormatSubCode(string source)
        {
            return FormatCode(source, false, false, false, true);
        }

        public static Stream GetCssStream()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "Manoli.Utils.CSharpFormat.csharp.css");
        }

        public static string GetCssString()
        {
            using (var reader = new StreamReader(GetCssStream()))
            {
                return reader.ReadToEnd();
            }
        }

        private Regex codeRegex;

        protected Regex CodeRegex
        {
            get { return codeRegex; }
            set { codeRegex = value; }
        }

        protected abstract string MatchEval(Match match);

        private string FormatCode(string source, bool lineNumbers,
            bool alternate, bool embedStyleSheet, bool subCode)
        {
            //replace special characters
            var sb = new StringBuilder(source);

            if (!subCode)
            {
                sb.Replace("&", "&amp;");
                sb.Replace("<", "&lt;");
                sb.Replace(">", "&gt;");
                sb.Replace("\t", string.Empty.PadRight(_tabSpaces));
            }

            //color the code
            source = codeRegex.Replace(sb.ToString(), new MatchEvaluator(this.MatchEval));

            sb = new StringBuilder();

            if (embedStyleSheet)
            {
                sb.AppendFormat("<style type=\"{0}\">\n", MimeTipleri.TextCss);
                sb.Append(GetCssString());
                sb.Append("</style>\n");
            }

            if (lineNumbers || alternate) //we have to process the code line by line
            {
                if (!subCode)
                    sb.Append("<div class=\"csharpcode\">\n");
                var reader = new StringReader(source);
                int i = 0;
                const string spaces = "    ";
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    i++;
                    if (alternate && ((i % 2) == 1))
                    {
                        sb.Append("<pre class=\"alt\">");
                    }
                    else
                    {
                        sb.Append("<pre>");
                    }

                    if (lineNumbers)
                    {
                        var order = (int)Math.Log10(i);
                        sb.Append("<span class=\"lnum\">"
                            + spaces.Substring(0, 3 - order) + i.ToString()
                            + ":  </span>");
                    }

                    if (line.Length == 0)
                        sb.Append("&nbsp;");
                    else
                        sb.Append(line);
                    sb.Append("</pre>\n");
                }
                reader.Close();
                if (!subCode)
                    sb.Append("</div>");
            }
            else
            {
                //have to use a <pre> because IE below ver 6 does not understand 
                //the "white-space: pre" CSS value
                if (!subCode)
                    sb.Append("<pre class=\"csharpcode\">\n");
                sb.Append(source);
                if (!subCode)
                    sb.Append("</pre>");
            }

            return sb.ToString();
        }

    }
}
