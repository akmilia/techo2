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
using System.Xml.Linq;

namespace techo
{
    /// <summary>
    /// Логика взаимодействия для MasterCommit.xaml
    /// </summary>
    public partial class MasterCommit : Window
    {
        Requests currentreq;
        private int selectedRequestID;
        private int masterID;
        public MasterCommit()
        {
            InitializeComponent();
            LoadData();
            this.masterID = masterID;

        }
        private void LoadData()
        {
            try
            {
                using (var db = new techoEntities())
                {
                    var requests = db.Requests.ToList();
                    foreach (var request in requests)
                    {
                        RequestComboBox.Items.Add($"Order #{request.RequestID}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


       private void Button_AddComment_Click(object sender, RoutedEventArgs e)
        {
            if (RequestComboBox.SelectedItem != null)
            {
                selectedRequestID = (int)RequestComboBox.SelectedValue;
                try
                {
                    using (techoEntities db = new techoEntities())
                    {
                        currentreq = db.Requests.FirstOrDefault(r => r.RequestID == selectedRequestID);

                        string commentMessage = CommentTextBox.Text;

                        if (string.IsNullOrWhiteSpace(commentMessage))
                        {
                            MessageBox.Show("Please enter a comment message.");
                            return;
                        }

                        int newCommentID = db.Comments.Any() ? db.Comments.Max(c => c.CommentID) + 1 : 1;
                        var newComment = new Comments
                        {
                            CommentID = newCommentID,
                            Message = commentMessage,
                            RequestID = selectedRequestID,
                        };

                        db.Comments.Add(newComment);
                        db.SaveChanges();
                        MessageBox.Show("Комментарий добавлен успешно");

                        // Очистим текстовое поле после добавления комментария
                        CommentTextBox.Text = string.Empty;

                        // Обновим окно с заявками в мастере
                        var masterWindow = Application.Current.Windows.OfType<MasterWindow>().FirstOrDefault();
                        masterWindow?.LoadRequests();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите номер заявки");
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           MasterWindow requestsMaster = new MasterWindow(masterID);
            requestsMaster.Show();
            this.Close();
        }

        private void RequestComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
