using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace XrefViewer.Resources
{
    public static class ResourceHandler
    {
        public static void ExtractIfNecessary(string resourceName, string destination)
        {
            if (!File.Exists(Path.Combine(destination, resourceName)))
            {
                if (!Directory.Exists(destination))
                    Directory.CreateDirectory(destination);
                ExtractResource(resourceName, destination);
            }
            else
            {
                if (!CompareChecksums(resourceName, destination))
                    ExtractResource(resourceName, destination);
            }
        }

        public static void ExtractResource(string resourceName, string destination)
        {
            XrefViewerMod.Logger.Msg("Extracting " + resourceName + "...");
            try
            {
                Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("XrefViewer.Resources." + resourceName);
                FileStream file = new FileStream(destination + "/" + resourceName, FileMode.Create, FileAccess.Write);
                resource.CopyTo(file);
                resource.Close();
                file.Close();
                XrefViewerMod.Logger.Msg("Successfully extracted " + resourceName);
            }
            catch (Exception e)
            {
                XrefViewerMod.Logger.Error(e);
            }
        }

        public static bool CompareChecksums(string resourceName, string externalPath)
        {
            XrefViewerMod.Logger.Msg("Validating checksums of external resource [" + resourceName + "]...");

            Stream internalResource = Assembly.GetExecutingAssembly().GetManifestResourceStream("XrefViewer.Resources." + resourceName);
            Stream externalResource = new FileStream(externalPath + "/" + resourceName, FileMode.Open, FileAccess.Read);

            SHA256 sha256 = SHA256.Create();
            string internalHash = BytesToString(sha256.ComputeHash(internalResource));
            string externalHash = BytesToString(sha256.ComputeHash(externalResource));

            internalResource.Close();
            externalResource.Close();

            XrefViewerMod.Logger.Msg("Internal Hash: " + internalHash);
            XrefViewerMod.Logger.Msg("External Hash: " + externalHash);

            return internalHash.Equals(externalHash);
        }

        private static string BytesToString(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));

            return builder.ToString();
        }
    }
}