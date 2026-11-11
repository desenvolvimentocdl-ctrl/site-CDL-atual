using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SiteCDL.Utilities
{
    public static class Remover
    {
        public static string AcentuacaoTexto(string text)
        {
            return new string(text
                .Normalize(NormalizationForm.FormD)
                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }

        public static string HtmlTags(string html)
        {
            return Regex.Replace(html, "<.*?>", "");
        }
    }
}
