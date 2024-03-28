using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;

namespace Paragoniarz.Domain
{
    public static class UriExtensions
    {
        private static readonly ImmutableList<char> AllowedControlChars = ['\u0009', '\u000a', '\u000d', '\u0014'];

        public static async Task<bool> IsBinaryFileAsync(this Uri uri)
        {
            if (Directory.Exists(uri.LocalPath))
                return true;

            using StreamReader stream = new(uri.LocalPath);
            char[] buffer = new char[1024];
            while ((await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                char character = buffer[0];
                if (!AllowedControlChars.Contains(character) && char.IsControl(character))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
