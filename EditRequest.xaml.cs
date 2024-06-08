using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    public partial class EditRequest : Window
    {
        Requests currentReq;
        int curUserID; 
        public EditRequest(int UserID)
        {
            InitializeComponent();
            LoadRequests(); 

            curUserID = UserID;
        }

        public void LoadRequests()
        {
            using (techoEntities db = new techoEntities())
            {
                //var requests1 = db.Requests.Where(r => r.ReqClient.Any(rc => rc.ClientID == curUserID)).ToList();
                //var requests = requests1.Select(r => new { r.RequestID })
                //        .ToList(); 

                var requests = db.Requests
                        .Where(r => r.ReqClient.Any(rc => rc.ClientID == curUserID))
                        .Select(r => new { r.RequestID }) // Select only RequestID for the ComboBox
                        .ToList();

                requestsComboBox.ItemsSource = requests;
            }
        } 


        private void LoadRequestData()
        {
            try
            {
                using (techoEntities db = new techoEntities())
                {

                    string HomeTechType = currentReq.HomeTechType.Any() ? currentReq.HomeTechType.First().homeTechType1 : "";
                    string homeTechModel = currentReq.HomeTechType.Any() ? currentReq.HomeTechType.First().homeTechModel : "";
                    string problemDescription = currentReq.ProblemDescription;

                    Type.Content = HomeTechType; 
                    Model.Content = homeTechModel;
                    Problem.Text = problemDescription;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {   
                if (Problem.Text.Length > 150)
                {
                    MessageBox.Show("Слишком длинное сообщение, до 150 символов"); 
                    return;
                } 
                else
                {
                    using (techoEntities db = new techoEntities())
                    {
                        currentReq.ProblemDescription = Problem.Text;
                        db.Requests.AddOrUpdate(currentReq);
                        db.SaveChanges();

                        this.Close();
                    }
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (requestsComboBox.SelectedItem != null)
            {
                int selectedRequestID = (int)requestsComboBox.SelectedValue;
                try
                {
                    using (techoEntities db = new techoEntities())
                    {

                        currentReq = db.Requests.FirstOrDefault(r => r.RequestID == selectedRequestID);
                        LoadRequestData();

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
    } 

   
}
