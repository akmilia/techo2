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


        public void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (techoEntities db = new techoEntities())
                {
                    if (!ValidateFields())
                        return;

                    if (db.Autorization.Any(a => a.Login == LoginTextBox.Text))
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует.");
                        LoginTextBox.Text = "";
                        return;
                    }

                    int typeId = GetTypeId(TypeComboBox.Text);

                    int maxUserId = db.Users.Max(u => u.UserID);
                    int nextUserId = maxUserId + 1;

                    Users newUser = new Users
                    {
                        UserID = nextUserId,
                        FIO = $"{FirstName.Text.Trim()} {SecondName.Text.Trim()} {ThirdName.Text.Trim()}",
                        Phone = PhoneNumberTextBox.Text,
                        TypeID = typeId,
                        Autorization = new Autorization
                        {
                            Login = LoginTextBox.Text.Trim(),
                            Password = PasswordTextBox.Password.Trim()
                        }
                    };

                    db.Users.Add(newUser);
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

        public  bool ValidateFields()
        {
            return ValidateRequiredField(SecondName, "Фамилию") &&
                   ValidateRequiredField(FirstName, "Имя") &&
                   ValidateRequiredField(ThirdName, "Отчество") &&
                   ValidateRequiredField(LoginTextBox, "Логин") &&
                   ValidateRequiredField(PhoneNumberTextBox, "номер") &&
                   ValidateRequiredPassword(PasswordTextBox) &&
                   ValidateNameFormat(SecondName, "фамилию") &&
                   ValidateNameFormat(FirstName, "имя") &&
                   ValidateNameFormat(ThirdName, "отчество") &&
                   ValidateLoginFormat(LoginTextBox) &&
                   ValidatePhoneNumberFormat(PhoneNumberTextBox) &&
                   ValidatePasswordFormat(PasswordTextBox.Password);
        }

        public bool ValidateRequiredField(TextBox textBox, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                MessageBox.Show($"Пожалуйста, введите {fieldName}. Поле не должно содержать пробелы");
                textBox.Text = string.Empty;
                return false;
            }
            return true;
        }

        public bool ValidateRequiredPassword(PasswordBox passwordBox)
        {
            if (string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                MessageBox.Show("Пожалуйста, введите пароль. Поле не должно содержать пробелы");
                passwordBox.Password = string.Empty;
                return false;
            }
            return true;
        }

        public bool ValidateNameFormat(TextBox textBox, string fieldName)
        {
            if (!Regex.IsMatch(textBox.Text, "^[А-Яа-яA-Za-z]{2,20}$"))
            {
                MessageBox.Show($"Пожалуйста, введите {fieldName} правильно! Только буквы, до 20", "Дата сохранения", MessageBoxButton.OK, MessageBoxImage.Information);
                textBox.Text = string.Empty;
                return false;
            }
            return true;
        }

        public bool ValidateLoginFormat(TextBox textBox)
        {
            if (textBox.Text.Length > 20)
            {
                MessageBox.Show("Логин должен быть до 20 символов");
                textBox.Text = string.Empty;
                return false;
            }
            return true;
        }

        public bool ValidatePhoneNumberFormat(TextBox textBox)
        {
            if (!Regex.IsMatch(textBox.Text, @"^\d{11}$"))
            {
                MessageBox.Show("Пожалуйста, введите телефон правильно! (11 цифр)", "Дата сохранения", MessageBoxButton.OK, MessageBoxImage.Information);
                textBox.Text = string.Empty;
                return false;
            }
            return true;
        }

        public bool ValidatePasswordFormat(string password)
        {
            bool hasDigit = password.Any(char.IsDigit);
            bool hasLower = password.Any(char.IsLower);
            bool hasUpper = password.Any(char.IsUpper);
            bool hasSpecialChar = password.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c));
            int length = password.Length;

            if (!hasDigit)
            {
                MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать цифры");
                return false;
            }
            if (!hasLower)
            {
                MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать строчные буквы");
                return false;
            }
            if (!hasUpper)
            {
                MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать заглавные буквы");
                return false;
            }
            if (!hasSpecialChar)
            {
                MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать специальные символы");
                return false;
            }
            if (length < 8 || length > 20)
            {
                MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать от 8 до 20 символов");
                return false;
            }

            return true;
        }

        public int GetTypeId(string type)
        {
            switch (type)
            {
                case "Менеджер":
                    return 1;
                case "Мастер":
                    return 2;
                case "Оператор":
                    return 3;
                case "Заказчик":
                    return 4;
                default:
                    return 0;
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
