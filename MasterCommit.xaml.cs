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
    public partial class MasterCommit : Window
    {
        Requests currentReq;
        int curUserID;
        public MasterCommit(int id)
        {
            InitializeComponent();


            int curUserID = id;
            MessageBox.Show("curID" + curUserID);
            LoadRequests();

        }

        public void LoadRequests()
        {
            using (techoEntities db = new techoEntities())
            {
                var requests = db.Requests
                    .Where(r => r.ReqClient.Any(rc => rc.MasterID == 2))
                    .Select(r => new { r.RequestID })
                    .ToList();

                requestsComboBox.ItemsSource = requests;
            }
        }



        private void Button_AddComment_Click(object sender, RoutedEventArgs e)
        {
            if (requestsComboBox.SelectedItem != null)
            {
               
                try
                {
                    using (techoEntities db = new techoEntities())
                    {
                        string commentMessage = Comment.Text;

                    
                        int newCommentID = db.Comments.Any() ? db.Comments.Max(c => c.CommentID) + 1 : 1;
                        var newComment = new Comments
                        {
                            CommentID = newCommentID,
                            Message = commentMessage,
                            RequestID = currentReq.RequestID,
                        };

                        db.Comments.Add(newComment);
                        db.SaveChanges();
                        MessageBox.Show("Комментарий добавлен успешно");

                        this.Close();
                       
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (requestsComboBox.SelectedItem != null)
            {
                int selectedRequestID = (int)requestsComboBox.SelectedValue;
                try
                {
                    using (techoEntities db = new techoEntities())
                    {

                        currentReq = db.Requests.FirstOrDefault(r => r.RequestID == selectedRequestID);
                        string com = currentReq.Comments.Where(c=> c.RequestID == selectedRequestID).Select(c=>c.Message).FirstOrDefault(); 
                                               Comment.Text = com;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите номер заявки");
            }
        }
   
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
