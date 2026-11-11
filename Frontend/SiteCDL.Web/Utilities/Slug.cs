using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SiteCDL.Utilities
{
    public static class Slug
    {
        public static string StringToSlug(string input = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    return string.Empty;
                }

                string normalizado = input.Normalize(NormalizationForm.FormD);
                StringBuilder stringBuilder = new StringBuilder();

                foreach (char c in normalizado)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(c);
                    }
                }

                string semAcentos = stringBuilder.ToString().Normalize(NormalizationForm.FormC);
                string resultado = Regex.Replace(semAcentos, @"[^a-zA-Z0-9\s-]", "").ToLower();

                resultado = Regex.Replace(resultado, @"\s+", "-");
                resultado = Regex.Replace(resultado, @"-+", "-");

                return resultado.Trim('-');
            }
            catch(Exception)
            {
                return string.Empty;
            }
        }
    }
}
