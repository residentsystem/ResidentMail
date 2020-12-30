using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Http;
using ResidentMail.Configurations;
using ResidentMail.Models;

namespace ResidentMail.Pages
{
    public class ConfirmationModel : PageModel
    {
        public string PageTitle = "Mail Confirmation";

        // Injecting the configuration to access the settings in the configuration file. 
        private IOptionsSnapshot<MailSettings> _settings;

        // Injecting the hosting environment the application is running in.
        private IHostEnvironment _env;

        private readonly IHttpContextAccessor _httpContextAccessor;

        // Injecting the session state to store user data while browsing the application.
        private ISession _session;

        private MailSettings MailSettings { get; set; }

        public string[] ConfirmationMessage = new string[3];

        // Bind variable with properties of the Contact model class.
        [BindProperty]
        public Contact Contact { get; set; }

        //Honeypot 
        public string SpamMessage { get; set; }

        public string SessionResult { get; set; }

        public ConfirmationModel(IOptionsSnapshot<MailSettings> settings, IHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _settings = settings;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public IActionResult OnGet()
        {
            return RedirectToPage("Index");
        }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    if (!String.IsNullOrEmpty(Contact.Honey))
                    {
                        SpamMessage = $"Suspecting a Spam: {Contact.Honey}";

                        throw new Exception();
                    }
                    else if (String.IsNullOrEmpty(Contact.Sum))
                    {
                        SpamMessage = $"Please provide the sum of those two numbers!";

                        throw new Exception();
                    }
                    else if (!String.IsNullOrEmpty(Contact.Sum))
                    {
                        SessionResult = _session.GetString("SessionResult");

                        if (Contact.Sum != SessionResult)
                        {
                            SpamMessage = $"Sorry, your anwser contain an error!";

                            throw new Exception();
                        }
                    }

                    // Bind the content of mailsettings.json to an instance of MailSettings
                    MailSettings = _settings.Value;

                    //Instantiate a new MimeMessage
                    var message = new MimeMessage();

                    //Set the primary recipient of the message
                    message.To.Add(new MailboxAddress(MailSettings.Name, MailSettings.Address));

                    //Set the primary sender of the message
                    message.From.Add(new MailboxAddress(Contact.Name, Contact.Email));

                    //Set the subject of the message 
                    message.Subject = Contact.Subject;

                    //Set the body of the message
                    message.Body = new TextPart("plain")
                    {
                        Text = Contact.Message + " \n\nMessage was sent by " + Contact.Name + ": " + Contact.Email
                    };

                    //Connect to the online mail service and send the message
                    using (var emailClient = new SmtpClient())
                    {
                        emailClient.Connect(MailSettings.Host, MailSettings.Port, false);
                        emailClient.Authenticate(MailSettings.Username, MailSettings.Password);
                        emailClient.Send(message);
                        emailClient.Disconnect(true);
                    }

                    // Confirm the message delivery when no error is found
                    ConfirmationMessage[0] = "Thank You!";
                    ConfirmationMessage[1] = "Your email was sent.";
                    ConfirmationMessage[2] = "Thank you for contacting me.";

                }
                catch (Exception ex)
                {
                    ModelState.Clear();

                    ConfirmationMessage[0] = "Sorry!";
                    ConfirmationMessage[1] = $"Something went wrong.";

                    if (_env.IsDevelopment())
                    {
                        ConfirmationMessage[2] = $"Your email was not sent. {ex.Message}";
                    }
                    else
                    {
                        ConfirmationMessage[2] = $"Your email was not sent.";
                    }
                }
            }
        }
    }
}