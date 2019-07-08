using MailKit.Security;

namespace Geev.MailKit
{
    public class GeevMailKitConfiguration : IGeevMailKitConfiguration
    {
        public SecureSocketOptions? SecureSocketOption { get; set; }
    }
}
