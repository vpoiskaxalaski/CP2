using System;
using System.Windows;
using System.Windows.Controls;

namespace Notest
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //using (UserContext db = new UserContext())
            //{
            //    db.Users.RemoveRange(db.Users);
            //}
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
                        MessageBox.Show("Welcome, " + Class.CurrentUser.user.Login + " !", "", MessageBoxButton.OK, MessageBoxImage.None);

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
                                MessageBox.Show("Welcome, " + Class.CurrentUser.user.Login + " !", "", MessageBoxButton.OK, MessageBoxImage.None);
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
                        if(isUserFind == false)
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
