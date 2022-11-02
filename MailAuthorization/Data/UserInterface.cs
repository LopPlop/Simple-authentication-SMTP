using MailAuthorization.Data.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailAuthorization.Data
{
    public class UserInterface
    {
        public static readonly string userLogin = "admin";
        public static readonly string userPassword = "admin";

        public UserInterface()
        {
            Console.WriteLine("\t\tКонсольная авторизация без СУБД\n\n");

            while (true)
            {
                Console.Write("Введите логин: ");
                string login = Console.ReadLine();
                Console.Write("Введите пароль: ");
                string password = EnterPassword();


                if (login == userLogin && 
                    password == userPassword)
                    break;
                else
                {
                    Console.WriteLine("\n\t\tНеверные данные. Хотите восстановить пароль(введите yes/no)?");
                    string answer = Console.ReadLine();
                    if (answer.Contains("yes") && AuthenticationByMailCode())
                    {
                        break;
                    }
                }
            }
            Console.WriteLine("\nВы вошли!");
            Console.ReadKey();
        }

        private static bool AuthenticationByMailCode()
        {
            var sendAuthCode = new SendYandexMailForAuth();
            if (sendAuthCode.SendMail())
            {
                Console.WriteLine($"Код был выслан на почту {SendYandexMailForAuth.mailBox}.\nВведите код подтверждения для входа");
                int code;
                while (true)
                {
                    try
                    {
                        code = int.Parse(Console.ReadLine());
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Неверные данные. Попробуйте заново или нажмите 'Enter' для выхода из программы");
                        if(Console.ReadKey().Key == ConsoleKey.Enter)
                        {
                            return false;
                        }
                    }
                }

                if(sendAuthCode.Code == code)
                return true;
            }
            else Console.WriteLine("Ошибка");
            return false;
        }

        private string EnterPassword()
        {
            string text = "";
            ConsoleKeyInfo keyInfo;

            while (true)
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (text.Length == 0)
                        continue;
                    text = text.Remove(text.Length - 1);
                    Console.Write("\b \b");
                }
                else
                {
                    text += keyInfo.KeyChar;
                    Console.Write("*");
                }
            }
            return text;
        }
    }
}
