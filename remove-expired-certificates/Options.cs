using CommandLine;

namespace vap.development.remove_expired_certificates
{
    internal class Options
    {
        [Option(HelpText = "Removes certificates instead of telling you what it would use")]
        public bool Live { get; set; }

        [Option(Default = "WebHosting", HelpText = "Certificate store to use")]
        public string Store { get; set; }
    }
}