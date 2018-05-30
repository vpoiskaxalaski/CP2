using System;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Notest
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            foreach (MenuItem item in Languages.Items)
            {
                string name = item.Name;
                item.Height = 25;
                item.Width = 50;
                item.ToolTip = name;
                item.Margin = new Thickness(10, 0, 0, 0);
                item.Background = new ImageBrush
                {
                    ImageSource = BitmapFrame.Create(new Uri(GetLanguageDirectory() + $"\\{name}.png", UriKind.Relative)),
                    Opacity = 0.7
                };
            }

            //using (UserContext db = new UserContext())
            //{
            //    db.Users.RemoveRange(db.Users);
            //}
        }

        private void OnLanguageChange(object sender, RoutedEventArgs e)
        {
            MenuItem selectedLang = sender as MenuItem;
            string lang = selectedLang.Name;
            DirectoryInfo directory = new DirectoryInfo(GetLanguageDirectory() + "/" + lang);
            try
            {
                FileInfo[] dictionaries = directory.GetFiles();
                Application.Current.Resources.Clear();
                foreach (FileInfo dictionary in dictionaries)
                {
                    int index = dictionary.FullName.IndexOf($"Languages\\{lang}");
                    string resourcePath = dictionary.FullName.Substring(index);
                    var uri = new Uri(resourcePath, UriKind.Relative);
                    ResourceDictionary resource = Application.LoadComponent(uri) as ResourceDictionary;
                    Application.Current.Resources.MergedDictionaries.Add(resource);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private string GetLanguageDirectory()
        {
            DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            return directory.Parent.Parent.FullName + "\\Languages";
        }

        //РЕГИСТРАЦИЯ
        private void OnRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            bool isLoginCorrect = CheckLogin(NewLogin, Alert_NewLogin);
            bool isPasswordCorrect = CheckPassword(NewPassword, Alert_NewPassword);

            if (isLoginCorrect && isPasswordCorrect)
            {
                try
                {
                    User user = new User
                    {
                        Login = NewLogin.Text,
                        Password = Validation.GetHashString(NewPassword.Password)
                    };
                    ComboBoxItem selectedItem = (ComboBoxItem)UserType.SelectedItem;
                    user.UserType = selectedItem.Content.ToString();
                    Class.CurrentUser.user = user;

                    // Создание подключения                  
                    using (Context db = new Context())
                    {
                        // добавляем их в бд
                        db.Users.Add(user);
                        db.SaveChanges();                        

                        switch (UserType.SelectedIndex)
                        {
                            case 0:
                                UserWindow userWindow = new UserWindow();
                                userWindow.Show();
                                this.Close();
                                break;
                            case 1:
                                CreaterWindow createrWindow = new CreaterWindow();
                                createrWindow.Show();
                                this.Close();
                                break;
                        }
                    }
                }
                catch
                {

                }
            }
        }

        //ВХОД
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool isLoginCorrect = CheckLogin(Login, Alert_Login);
            bool isPasswordCorrect = CheckPassword(Password, Alert_Password);

            if (isLoginCorrect && isPasswordCorrect)
            {
                try
                {
                    User user = new User
                    {
                        Login = Login.Text,
                        Password = Validation.GetHashString(Password.Password)
                    };
                    Class.CurrentUser.user = user;

                    using (Context db = new Context())
                    {
                        var users = db.Users;
                        bool isUserFind = false;

                        foreach (User u in users)
                        {
                            if (user.Password == u.Password && user.Login == u.Login)
                            {
                                isUserFind = true;
                               
                                if (u.UserType == "User")
                                {
                                    UserWindow userWindow = new UserWindow();
                                    userWindow.Show();
                                    this.Close();
                                }
                                else
                                {
                                    CreaterWindow createrWindow = new CreaterWindow();
                                    createrWindow.Show();
                                    this.Close();
                                }
                            }
                        }
                        if (isUserFind == false)
                        {
                            MessageBox.Show("Check the entered data", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }

                }
                catch
                {
                }
            }
        }

        #region кнопки для окна
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void HideWindow_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        #endregion


    }


}
