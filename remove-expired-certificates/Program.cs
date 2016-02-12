using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using CommandLine;

namespace vap.development.remove_expired_certificates
{
    internal class Program
    {
        public static Options Options;

        private static bool IsElevated
            => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        private static void Main(string[] args)
        {
            var commandLineParseResult = Parser.Default.ParseArguments<Options>(args);
            var parsed = commandLineParseResult as Parsed<Options>;
            if (parsed == null)
            {
                return; // not parsed
            }
            Options = parsed.Value;
            Console.WriteLine("Remove Expired Certificates");
            if (!Options.Live)
            {
                Console.WriteLine($"Live argument not set so not removing");
            }

            X509Store store;
            try
            {
                store = new X509Store(Options.Store, StoreLocation.LocalMachine);
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite);
            }
            catch (CryptographicException)
            {
                store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Console.WriteLine($" Opened Certificate Store \"{store.Name}\"");
            try
            {
                var col = store.Certificates;

                foreach (var cert in col)
                {
                    if (Convert.ToDateTime(cert.GetExpirationDateString()) <= DateTime.Now)
                    {
                        Console.WriteLine($" Removing Certificate from Store {cert.FriendlyName}");

                        if (Options.Live)
                        {
                            store.Remove(cert);
                        }
                    }
                }
                Console.WriteLine($" Closing Certificate Store");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing certificate: {ex.Message}");
            }
            store.Close();
        }
    }
}