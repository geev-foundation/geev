using System;
using MailKit.Net.Smtp;

namespace Geev.MailKit
{
    public interface IMailKitSmtpBuilder
    {
        SmtpClient Build();
    }
}