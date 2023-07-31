using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FashionForward.Helpers;

public class EmailHelper : IEmailSender
{
    private string EmailFrom;

    public string SendGridSecret { get; set; }

    public EmailHelper(IConfiguration config)
    {
        SendGridSecret = config.GetValue<string>("SendGrid:SecretKey");
        EmailFrom = config.GetValue<string>("SendGrid:Email");
    }

    public string GetVerificationEmailHtml(string verificationLink)
    {
        string html = @"
        <html>
        <head>
            <style>
                .button {
                    display: inline-block;
                    background-color: #4CAF50;
                    border: none;
                    color: white;
                    text-align: center;
                    font-size: 16px;
                    padding: 10px 24px;
                    cursor: pointer;
                    text-decoration: none;
                }
            </style>
        </head>
        <body>
            <h2>Email Verification</h2>
            <p>Please verify your email address by clicking the button below:</p>
            <a class=""button"" href=""" + verificationLink + @""">Verify Email</a>
        </body>
        </html>
        ";

        return html;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var client = new SendGridClient(SendGridSecret);
        var from = new EmailAddress(EmailFrom);
        var to = new EmailAddress(email);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
        return client.SendEmailAsync(msg);
    }
}

