using MailKit.Security;

namespace Geev.MailKit
{
    public interface IGeevMailKitConfiguration
    {
        SecureSocketOptions? SecureSocketOption { get; set; }
    }
}