using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailAuthorization.Data.Interfaces
{
    public class SendYandexMailForAuth : ISendMail
    {
        public static readonly string mailBox = "test.csharp@yandex.ru";


        public int Code { get; set; }
        public bool SendMail()
        {
            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.yandex.ru", 465, true);
                    smtp.Authenticate(Properties.Settings.Default.YandexLogin, Properties.Settings.Default.YandexPassword);

                    var rnd = new Random();
                    Code = rnd.Next(1000, 9999);

                    var bodyBilder = new BodyBuilder();
                    bodyBilder.TextBody = $"Ваш код для восстановления {Code}";
                    bodyBilder.HtmlBody = $"Ваш код для восстановления {Code}";

                    var msg = new MimeMessage()
                    {
                        Subject = "Восстановление пароля",
                        Body = bodyBilder.ToMessageBody()
                    };

                    msg.To.Add(new MailboxAddress(address: mailBox, name: "csharp authorization mail"));
                    msg.From.Add(new MailboxAddress(address: mailBox, name: "csharp authorization mail"));

                    smtp.Send(msg);
                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
