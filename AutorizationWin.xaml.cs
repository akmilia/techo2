using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace techo
{
    public partial class AutorizationWin : Window
    {
        public AutorizationWin()
        {
            InitializeComponent();
        }

        public void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            Registration registrationWindow = new Registration();
            registrationWindow.Show();
            this.Close();
        }
        public  void AuthorizeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (techoEntities db = new techoEntities())
                {
                    string login = LoginTextBox.Text.Trim();
                    string password = PasswordBox.Password.Trim();

                    var user = db.Autorization.FirstOrDefault(a => a.Login == login && a.Password == password);
                    if (user != null)
                    {
                        MessageBox.Show("Успешно вошли в систему");
                        int id = user.UserID;



                        var userType = user.Users.TypeID;
                        Window windowToOpen;
                        //MessageBox.Show(user.UserID.ToString()); 

                        switch (userType)
                        {
                            case 1: // Менеджер
                                windowToOpen = new RequestListAdmin();
                                break;
                            case 2: // Мастер
                                windowToOpen = new MasterWindow();
                                break;
                            case 3: // Оператор
                                windowToOpen = new RequestListAdmin();
                                break;
                            case 4: // Заказчик
                               // MessageBox.Show(id.ToString());
                                windowToOpen = new RequestsList(id);
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
                        LoginTextBox.Text = "";
                        PasswordBox.Password = "";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Возникла ошибка: {ex.Message}");
                return;
            }
        }


    }
}
