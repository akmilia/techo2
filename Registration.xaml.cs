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
                    {
                    MessageBoxButton btn = MessageBoxButton.OK;
                    MessageBoxImage ico = MessageBoxImage.Information;
                    string caption = "Дата сохранения";
                    if (string.IsNullOrWhiteSpace(PasswordTextBox.Text) || string.IsNullOrWhiteSpace(LoginTextBox.Text)
                            || string.IsNullOrWhiteSpace(Patronymic.Text) || string.IsNullOrWhiteSpace(PhoneNumber.Text))
                    {
                        MessageBox.Show("Все поля обязательны для ввода.");
                        PasswordTextBox.Password = "";
                        Surname.Text = "";
                       
                        return;
                    }
                    if (!Regex.IsMatch(FirstName.Text, "^[А-Яа-яA-Za-z]{2,20}$"))
                    {
                        MessageBox.Show("Пожалуйста,введите имя повторно!", caption, btn, ico);
                        FirstName.Text = "";
                        return;
                    }

                    if (!Regex.IsMatch(Surname.Text, "^[А-Яа-яA-Za-z]{2,20}$"))
                    {
                        MessageBox.Show("Пожалуйста, введите фамилию правильно!", caption, btn, ico);
                        Surname.Text = "";
                        return;
                    }

                    if (!Regex.IsMatch(Patronymic.Text, "^[А-Яа-яA-Za-z]{5,20}$"))
                    {
                        MessageBox.Show("Пожалуйста,введите отчество повторно!", caption, btn, ico);
                        Patronymic.Text = "";
                        return;
                    }

                    if (!Regex.IsMatch(PhoneNumber.Text, @"^\d{11}$"))
                    {
                        MessageBox.Show("Пожалуйста, введите телефон правильно!", caption, btn, ico);
                        PhoneNumber.Text = "";
                        return;
                    }

                    Clients clients = new Clients()
                    {
                        SName = Surname.Text,
                        FName = FirstName.Text,
                        TName = Patronymic.Text,
                        Phone = PhoneNumber.Text
                    };
                    db.Clients.Add(clients);
                    db.SaveChanges();
                    MessageBox.Show("Успешно добавлено");

                    Client client = new Client();
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
            Autorization autorization = new Autorization();
            autorization.Show();
            this.Close();
        }
    }
}
