using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AD.Shared.Extensions
{
    /// <summary>
    /// GUIDs are very useful tool for your web and application developpers but a little bit verbose. 
    /// The little functions below let you shave 30% off the guid string size while keeping the entire data.  
    /// This not rocket science, I use the standard: Base64 with URL and Filename Safe Alphabet (RFC 4648 'base64url' encoding) without the end padding.  
    /// </summary>
    public static class GuidExtensions
    {
        public static string ToShortGuid(this Guid newGuid)
        {
            string modifiedBase64 = Convert.ToBase64String(newGuid.ToByteArray())
                .Replace('+', '-').Replace('/', '_') // avoid invalid URL characters
                .Substring(0, 22);
            return modifiedBase64;
        }

        public static Guid ParseShortGuid(string shortGuid)
        {
            string base64 = shortGuid.Replace('-', '+').Replace('_', '/') + "==";
            Byte[] bytes = Convert.FromBase64String(base64);
            return new Guid(bytes);
        }
    }
}
