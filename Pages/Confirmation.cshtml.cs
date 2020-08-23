using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using ResidentMail.Configurations;
using ResidentMail.Models;

namespace ResidentMail.Pages
{
    public class ConfirmationModel : PageModel
    {
        public string PageTitle = "Confirmation";

        private MailSettings MailSettings { get; set; }

        // Bind variable with properties of the Contact model class.
        [BindProperty]
        public Contact Contact { get; set; } 

        public string[] Confirmation = new string[5];

        private IHostEnvironment _env;

        // Injecting configuration to access the values of the configuration file. 
        private IOptionsSnapshot<MailSettings> _settings;

        public ConfirmationModel(IHostEnvironment env, IOptionsSnapshot<MailSettings> settings)
        {
            _settings = settings;
            _env = env;
        }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Bind the content of mailsettings.json to an instance of MailSettings
                    //MailSettings settings = _configuration.GetSection("MailSettings").Get<MailSettings>();

                    // Bind the content of mailsettings.json to an instance of MailSettings
                    MailSettings = _settings.Value;

                    //Instantiate a new MimeMessage
                    var message = new MimeMessage();

                    //Setting the To e-mail address
                    message.To.Add(new MailboxAddress(MailSettings.Name, MailSettings.Address));

                    //Setting the From e-mail address
                    message.From.Add(new MailboxAddress(Contact.Name, Contact.Email));

                    //E-mail subject 
                    message.Subject = Contact.Subject;
                    
                    //E-mail message body
                    message.Body = new TextPart("plain")
                    {
                        Text = Contact.Message + " \n\nMessage was sent by: " + Contact.Name + " E-mail: " + Contact.Email
                    };

                    //Configure the e-mail
                    using (var emailClient = new SmtpClient())
                    {
                        emailClient.Connect(MailSettings.Host, MailSettings.Port, false);
                        emailClient.Authenticate(MailSettings.Username, MailSettings.Password);
                        emailClient.Send(message);
                        emailClient.Disconnect(true);
                    }

                    // Confirmation and message when no error is found
                    Confirmation[0] = "Thank you.";
                    Confirmation[1] = "Email has been sent successfully!";
                    Confirmation[2] = "Dear customer, thanks for reaching out!";
                    Confirmation[3] = "We’re thrilled to hear from you. Our inbox can’t wait to get your messages, so talk to us any time you like.";
                    Confirmation[4] = "Cheers!";
                }
                catch (Exception ex)
                {
                    ModelState.Clear();

                    // Error message if exception is found
                    Confirmation[0] = "Sorry.";

                    if (_env.IsDevelopment())
                    {
                        Confirmation[1] = $"Your email was not sent. {ex.Message}";
                    }
                    else 
                    {
                        Confirmation[1] = $"Your email was not sent.";
                    }

                    Confirmation[2] = "Dear customer, sorry for this inconvenience!";
                    Confirmation[3] = "We’re thrilled to hear from you. You can still reach us by phone: (555)555-5555.";
                    Confirmation[4] = "Cheers!";
                }
            }
        }
    }
}