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
    public partial class AutorizationWindow : Window
    {
        public AutorizationWindow()
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
            try
            {
                using (techoEntities db = new techoEntities())
                {
                    string login = LoginTextBox.Text;
                    string password = PasswordBox.Password;

                    var user = db.Autorization.FirstOrDefault(a => a.Login == login && a.Password == password);
                    if (user != null)
                    {
                        MessageBox.Show("Успешно вошли в систему");

                        // Определяем роль пользователя
                        var userType = user.Users.TypeID;
                        Window windowToOpen;

                        switch (userType)
                        {
                            case 1: // Менеджер
                                windowToOpen = new AddRequest();
                                break;
                            case 2: // Мастер
                                windowToOpen = new MasterWindow();
                                break;
                            case 3: // Оператор
                                windowToOpen = new RequestListAdmin();
                                break;
                            case 4: // Заказчик
                                windowToOpen = new RequestsList();
                                break;
                            default:
                                throw new InvalidOperationException("Неизвестная роль пользователя");
                        }

                        windowToOpen.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Возникла ошибка: {ex.Message}");
            }
            //RequestsList1 registrationWindow = new RequestsList1();
            //registrationWindow.Show();
            //this.Close();
        }

       

    }
}
