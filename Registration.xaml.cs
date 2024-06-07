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
                    if (string.IsNullOrWhiteSpace(PasswordTextBox.Password) ||
                        string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
                        string.IsNullOrWhiteSpace(SecondName.Text) ||
                        string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text))
                    {
                        MessageBox.Show("Все поля обязательны для ввода и пробелы запрещены.");
                        return;
                    }

                    if (!Regex.IsMatch(SecondName.Text, "^[А-Яа-яA-Za-z]{2,20}$"))
                    {
                        MessageBox.Show("Пожалуйста, введите фамилию правильно!", "Дата сохранения", MessageBoxButton.OK, MessageBoxImage.Information);
                        SecondName.Text = "";
                        return;
                    }

                    if (!Regex.IsMatch(FirstName.Text, "^[А-Яа-яA-Za-z]{2,20}$"))
                    {
                        MessageBox.Show("Пожалуйста, введите имя правильно!", "Дата сохранения", MessageBoxButton.OK, MessageBoxImage.Information);
                        FirstName.Text = "";
                        return;
                    }

                    if (!Regex.IsMatch(ThirdName.Text, "^[А-Яа-яA-Za-z]{2,20}$"))
                    {
                        MessageBox.Show("Пожалуйста, введите отчество правильно!", "Дата сохранения", MessageBoxButton.OK, MessageBoxImage.Information);
                        ThirdName.Text = "";
                        return;
                    }

                    // нужна проверка уникальности логина if not exist 
                    if (db.Autorization.Any(a => a.Login == LoginTextBox.Text))
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует.");
                        LoginTextBox.Text = "";
                        return;
                    }

                    if (!Regex.IsMatch(PhoneNumberTextBox.Text, @"^\d{11}$"))
                    {
                        MessageBox.Show("Пожалуйста, введите телефон правильно!(11 цифр)", "Дата сохранения", MessageBoxButton.OK, MessageBoxImage.Information);
                        PhoneNumberTextBox.Text = "";
                        return;
                    }

                    if (!ValidatePassword(PasswordTextBox.Password))
                    {
                        PasswordTextBox.Password = "";
                        return;
                    }

                    int typeId;
                    switch (TypeComboBox.Text)
                    {
                        case "Менеджер":
                            typeId = 1;
                            break;
                        case "Мастер":
                            typeId = 2;
                            break;
                        case "Оператор":
                            typeId = 3;
                            break;
                        case "Заказчик":
                            typeId = 4;
                            break;
                    }

                    int maxUserId = db.Users.Max(u => u.UserID);
                    int nextUserId = maxUserId + 1; 

                    MessageBox.Show("1");
                    Users newUser = new Users
                    {
                       
                        UserID = nextUserId,
                        FIO = $"{FirstName.Text.Trim()} {SecondName.Text.Trim()} {ThirdName.Text.Trim()}",
                        Phone = PhoneNumberTextBox.Text,
                        TypeID = 1,
                        Autorization = new Autorization
                        {
                            Login = LoginTextBox.Text.Trim(),
                            Password = PasswordTextBox.Password.Trim()
                        }
                    };
                    //db.Autorization.Add(newUser.Autorization);
                    db.Users.Add(newUser);
                    //newUser.Autorization = null;
                    db.SaveChanges();

                    AutorizationWin autorization = new AutorizationWin();
                    autorization.Show();
                    this.Close();

                    MessageBox.Show("Пользователь успешно создан.");
                }
            }
            catch
            {
                MessageBox.Show("Возникла ошибка ");
            }
        }
        public void AuthorizeButton_Click(object sender, RoutedEventArgs e)
        {
            AutorizationWin autorization = new AutorizationWin();
            autorization.Show();
            this.Close();
        }



        public bool ValidatePassword(string x)
        {
            bool hasDigit = false;
            bool hasLower = false;
            bool hasUpper = false;
            bool hasSpecialChar = false;
            int kol = x.Length;

            foreach (char c in x)
            {
                if (char.IsDigit(c))
                {
                    hasDigit = true;
                }
                else if (char.IsLower(c))
                {
                    hasLower = true;
                }
                else if (char.IsUpper(c))
                {
                    hasUpper = true;
                }
                else if (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c))
                {
                    hasSpecialChar = true;
                }
            }

            if (!hasDigit)
            {
                MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать цифры");
                return false;
            }
            else if (!hasLower)
            {
                MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать строчные буквы");
                return false;
            }
            else if (!hasUpper)
            {
                MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать заглавные буквы");
                return false;
            }
            else if (!hasSpecialChar)
            {
                MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать специальные символы");
                return false;
            }
            else if (kol < 8 || kol > 20)
            {
                MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать от 8 до 20 символов");
                return false;
            }

            return true;
        }
    }
}
