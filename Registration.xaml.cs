using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }
        public void RegisterButton_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                using (techoEntities db = new techoEntities())
                {


                    int maxUserId = db.Users.Max(u => (int?)u.UserID) ?? 0;
                    int nextUserId = maxUserId + 1;

                    string caption = "Дата сохранения";
                    if (string.IsNullOrWhiteSpace(PasswordTextBox.Password) ||
                        string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
                        string.IsNullOrWhiteSpace(SecondName.Text) ||
                        string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text))
                    {
                        MessageBox.Show("Все поля обязательны для ввода.");
                        PasswordTextBox.Password = "";
                        FirstName.Text = "";
                        return;
                    }

                    if (!Regex.IsMatch(FirstName.Text, "^[А-Яа-яA-Za-z]{2,20}$"))
                    {
                        MessageBox.Show("Пожалуйста, введите имя правильно!", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                        FirstName.Text = "";
                        return;
                    }

                    if (!Regex.IsMatch(SecondName.Text, "^[А-Яа-яA-Za-z]{2,20}$"))
                    {
                        MessageBox.Show("Пожалуйста, введите фамилию правильно!", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                        SecondName.Text = "";
                        return;
                    }

                    if (!Regex.IsMatch(ThirdName.Text, "^[А-Яа-яA-Za-z]{2,20}$"))
                    {
                        MessageBox.Show("Пожалуйста, введите отчество правильно!", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                        ThirdName.Text = "";
                        return;
                    }

                    if (!Regex.IsMatch(PhoneNumberTextBox.Text, @"^\d{11}$"))
                    {
                        MessageBox.Show("Пожалуйста, введите телефон правильно!", caption, MessageBoxButton.OK, MessageBoxImage.Information);
                        PhoneNumberTextBox.Text = "";
                        return;
                    }

                    // Определяем роль пользователя
                    int typeId = 1; // Default
                    switch (TypeComboBox.Text)
                    {
                        case "Менеджер": typeId = 1; break;
                        case "Мастер": typeId = 2; break;
                        case "Оператор": typeId = 3; break;
                        case "Заказчик": typeId = 4; break;
                    }

                    Users newUser = new Users
                    {
                        UserID = nextUserId,
                        FIO = $"{FirstName.Text} {SecondName.Text} {ThirdName.Text}",
                        Phone = PhoneNumberTextBox.Text,
                        TypeID = typeId,
                        Autorization = new Autorization
                        {
                            Login = LoginTextBox.Text,
                            Password = PasswordTextBox.Password
                        }
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();
                    MessageBox.Show("Успешно зарегистрировано");


                  RequestsList   client = new RequestsList();
                    client.Show();
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Возникла ошибка");
            }
        }
        public void AuthorizeButton_Click(object sender, RoutedEventArgs e)
        {
            AutorizationWin autorization = new AutorizationWin();
            autorization.Show();
            this.Close();
        }
    }
}
