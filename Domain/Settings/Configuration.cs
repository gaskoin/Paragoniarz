using System;
using System.IO;

namespace Paragoniarz.Domain.Settings
{
    public record Configuration(EmailConfiguration EmailConfiguration, SellerConfiguration SellerConfiguration)
    {
        public string AssetsDirectory => GetAssetsDirectory();
        public string DocumentsDirectory => GetDocumentsDirectory();

        public bool IsLocked { get; set; }

        public static Configuration CreateDefault()
        {
            return new Configuration(
                new EmailConfiguration(),
                new SellerConfiguration()
            );
        }

        private static string GetDocumentsDirectory()
        {
            var documentsDirectory = $"{GetProgramDirectory()}/Documents";
            Directory.CreateDirectory(documentsDirectory);
            return documentsDirectory;
        }

        private static string GetAssetsDirectory()
        {
            return $"{GetProgramDirectory()}/Assets";
        }

        internal static string GetProgramDirectory() => AppDomain.CurrentDomain.BaseDirectory;

        public bool ShouldSerializeAssetsDirectory() => false;
        public bool ShouldSerializeDocumentsDirectory() => false;
    }
}
