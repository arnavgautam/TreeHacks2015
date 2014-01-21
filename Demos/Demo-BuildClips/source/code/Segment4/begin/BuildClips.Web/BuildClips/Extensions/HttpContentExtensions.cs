namespace BuildClips.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    public static class HttpContentExtensions
    {
        public static bool TryGetFormFieldStringValue(this IEnumerable<HttpContent> contents, string dispositionName, out string formFieldValue)
        {
            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }
            
            if (string.IsNullOrWhiteSpace(dispositionName))
            {
                throw new ArgumentNullException("dispositionName");
            }

            HttpContent content = contents.FirstOrDefault<HttpContent>(delegate(HttpContent item)
            {
                return FirstDispositionName(item, dispositionName);
            });

            if (content != null)
            {
                formFieldValue = content.ReadAsStringAsync().Result;
                return true;
            }

            formFieldValue = null;

            return false;
        }

        public static bool TryGetFormFieldHttpContentValue(this IEnumerable<HttpContent> contents, string dispositionName, out HttpContent formFieldValue)
        {
            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            if (string.IsNullOrWhiteSpace(dispositionName))
            {
                throw new ArgumentNullException("dispositionName");
            }

            HttpContent content = contents.FirstOrDefault<HttpContent>(delegate(HttpContent item)
            {
                return FirstDispositionName(item, dispositionName);
            });

            formFieldValue = content;

            return formFieldValue != null;
        }

        private static bool FirstDispositionName(HttpContent content, string dispositionName)
        {
            return content.Headers != null && content.Headers.ContentDisposition != null && string.Equals(UnquoteToken(content.Headers.ContentDisposition.Name), UnquoteToken(dispositionName), StringComparison.OrdinalIgnoreCase);
        }

        private static string UnquoteToken(string token)
        {
            if (!string.IsNullOrWhiteSpace(token) && ((token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal)) && (token.Length > 1)))
            {
                return token.Substring(1, token.Length - 2);
            }

            return token;
        }
    }
}