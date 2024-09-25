using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Net;
using System.Net.Mail;

namespace TraahvIndividual.Models
{
    [Table("Traahv")]
    public class Traahv
    {
        [Key]
        public int Id { get; set; }
        [StringLength(7, MinimumLength = 7, ErrorMessage = "Vale SoidukeNumber")]
        public string SoidukeNumber { get; set; }
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\s]{1,30}$", ErrorMessage = "Vale OmanikuNimi")]
        public string OmanikuNimi { get; set; }
        [RegularExpression(@".+\@.+\..+", ErrorMessage = "Valesti sisestatud email")]
        public string OmanikuEpost { get; set; }
        public DateTime Rikkumisekuupaev { get; set; }
        [Range(3, 300, ErrorMessage = "Valesti KiiruseUletamine")]
        public int KiiruseUletamine { get; set; }

        public int TrahviSuurus { get; set; }
        public virtual ICollection<Login> Logins { get; set; }


        public void CalculateFine()
        {
            if (KiiruseUletamine <= 10)
            {
                TrahviSuurus = 25;
            }
            else if (KiiruseUletamine <= 20)
            {
                TrahviSuurus = 45;
            }
            else if (KiiruseUletamine <= 45)
            {
                TrahviSuurus = 65;
            }
            else if (KiiruseUletamine <= 60)
            {
                TrahviSuurus = 120;
            }
            else if (KiiruseUletamine <= 80)
            {
                TrahviSuurus = 275;
            }
            else
            {
                TrahviSuurus = 400;
            }
        }
        public void SendMessage()
        {

            try
            {

                var fromAddress = new MailAddress("finesender2000@gmail.com", "Police");
                var toAddress = new MailAddress(OmanikuEpost, OmanikuNimi);
                const string fromPassword = "tgwt xjrr yrea xida";
                string message1 = "Trahv infromatsion " + SoidukeNumber;
                string subject = message1;
                string body = $"Tere, kell {Rikkumisekuupaev} rikute kiirusepiirangut ({KiiruseUletamine}), nii et te trahv {TrahviSuurus}. Maksmiseks on teil 2 kuud";


                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                // Create the email message
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    // Send the email
                    smtp.Send(message);
                }

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email. Error: " + ex.Message);
            }


        }
    }
}