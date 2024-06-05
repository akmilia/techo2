using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace techo
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        public Autorization()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }
        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            Registration registrationWindow = new Registration();
            registrationWindow.Show();
            this.Close();
        }
        private void AuthorizeButton_Click(object sender, RoutedEventArgs e)
        {    
            /////код для входа  
            ////если клиент
            RequestsList registrationWindow = new RequestsList();
            registrationWindow.Show();
            this.Close();
            string login = LoginTextBox.Text; // Получаем введенный логин
            string password = PasswordBox.Password; // Получаем введенный пароль

            // Проверяем логин и пароль в базе данных
            if (CheckCredentials(login, password))
            {
                // Fetch user data from the database
                using (techoEntities db = new techoEntities())
                {
                    CurrentUser.UserData = db.Users.FirstOrDefault(t => t.Login == login);
                }

                // Open the ClientWEdit window
              

              
                MessageBox.Show("Вход успешен!");
            }
            else
            {
                // Show an error message if credentials are incorrect
                MessageBox.Show("Неверные логин или пароль.");
            }
            ////если клиент
            //RequestsList1 registrationWindow = new RequestsList1();
            //registrationWindow.Show();
            //this.Close();
        }

        private bool AuthenticateUser(string password)
        {
            using (techoEntities db = new techoEntities())
            {
                var user = db.Users.FirstOrDefault(u => u.Pass == password);

                return user != null;
            }
        }
        private bool CheckCredentials(string login, string password)
        {
            using (techoEntities db = new techoEntities())
            {
                // Check if a user with the provided login and password exists in the database
                return db.Users.Any(t => t.Login == login && t.Pass == password);
            }
        }

    }
}
