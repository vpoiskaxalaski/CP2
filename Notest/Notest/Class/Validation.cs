using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity.Validation;


namespace Notest
{
    public static class Validation
    {
      

        //ПРОВЕРКА на повторение логина
        public static bool NoRepeatLogin(string s)
        {
            bool isNoRepeat = true;          

            using (Context db = new Context())
            {
                try
                {                 
                    // получаем объекты из бд и выводим на консоль
                    var users = db.Users;
                    foreach (User u in users)
                    {
                        if(s == u.Login )
                        {
                            isNoRepeat = false;                           
                        }
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        MessageBox.Show(String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            MessageBox.Show(String.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }           
            return isNoRepeat;
        }

        //хэширование пароля
        public static Guid GetHashString(string s)
        {
            //переводим строку в байт-массим  
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return new Guid(hash);
        }

        //проверка на существование теста
        public static bool IsTestExist (string topic, string header)
        {
            bool isExist = false;

            using (Context db = new Context())
            {
                var tests = db.Tests;

                foreach(Test t in tests)
                {
                    if(t.Topic == topic && t.Header == header)
                    {
                        isExist = true;
                    }
                }
            }

            return isExist;
        }
    }

        public partial class MainWindow : Window
        {
        //корректность логина
        private bool CheckLogin(TextBox login, Image image)
        {
            string regex = @"[0-9a-zA-Z]";
            bool valid = true;
            ToolTip toolTip = new ToolTip();

            for (int i = 0; i < login.Text.Length; i++)
            {
                if (Regex.IsMatch(login.Text[i].ToString(), regex) == false)
                {
                    valid = false;
                    toolTip.Content = "Wrong symbol";
                }
            }

            if (login.Text.Length == 0)
            {
                valid = false;
                toolTip.Content = "Can not be empty";
            }

            if (login.Name == "NewLogin")
            {
                if (Validation.NoRepeatLogin(login.Text) == false)
                {
                    valid = false;
                    toolTip.Content = "Данный логин уже существует";
                }
            }
           
            if (valid == false)
            {
                login.BorderBrush = new SolidColorBrush(Colors.IndianRed);
                image.Visibility = Visibility.Visible;
                image.ToolTip = toolTip;
            }

            return valid;
        }

        //корректность пароля
        private bool CheckPassword(PasswordBox passwordBox, Image image)
        {
            string regex = @"[0-9a-zA-Z]";
            bool valid = true;
            ToolTip toolTip = new ToolTip();

            for (int i = 0; i < passwordBox.Password.Length; i++)
            {
                if (Regex.IsMatch(passwordBox.Password[i].ToString(), regex) == false)
                {
                    valid = false;
                    toolTip.Content = "Wrong symbol";
                }
            }

            if (passwordBox.Password.Length == 0)
            {
                valid = false;
                toolTip.Content = "Can not be empty";
            }

            if (valid == false)
            {
                passwordBox.BorderBrush = new SolidColorBrush(Colors.IndianRed);
                image.Visibility = Visibility.Visible;
                image.ToolTip = toolTip;
            }
            return valid;
        }

       
    }
}
