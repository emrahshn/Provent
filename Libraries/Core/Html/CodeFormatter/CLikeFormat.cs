
namespace Core.Html.CodeFormatter
{
    public abstract partial class CLikeFormat : CodeFormat
    {
        protected override string CommentRegex
        {
            get { return @"/\*.*?\*/|//.*?(?=\r|\n)"; }
        }

        protected override string StringRegex
        {
            get { return @"@?""""|@?"".*?(?!\\).""|''|'.*?(?!\\).'"; }
        }
    }
}
