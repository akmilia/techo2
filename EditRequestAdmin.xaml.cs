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
 
    public partial class EditRequestAdmin : Window
    {
        Requests currentReq;
        public EditRequestAdmin()
        {
            InitializeComponent();

            LoadEverything();
          
        }

        public void LoadEverything()
        {
            using (techoEntities db = new techoEntities())
            {
             
                var requests = db.Requests
                        .Select(r => new { r.RequestID }) 
                        .ToList();

                requestsComboBox.ItemsSource = requests; 

                var statuses = db.Statuses
                       .Select(r => new { r.StatusDescription }) 

                       .ToList();

                statusesComboBox.ItemsSource = statuses;

                var masters = db.Users
                       .Where(r => r.TypeID == 2)
                       .Select(r => new { r.FIO })
                       .ToList();

                mastersComboBox.ItemsSource = masters;
            }
        }
     
        //?
        private void LoadRequestData()
        {
            try
            {
                using (techoEntities db = new techoEntities())
                {
                    var fioOfMaster = currentReq.ReqClient
                    .Join(db.Users, rc => rc.MasterID, u => u.UserID, (rc, u) => new { ReqClient = rc, User = u })
                    .Where(x => x.User.TypeID == 2)
                    .Select(x => x.User.UserID);

                    string status = currentReq.Statuses.Any() ? currentReq.Statuses.First().StatusDescription : "";

                    //если тут что-то и прописывать, то только через += к label и связки
                    statusesComboBox.Text = status; 
                    mastersComboBox.Text = fioOfMaster.ToString();

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
                    using (techoEntities db = new techoEntities())
                    {    

                        string status = statusesComboBox.Text;
                        string masterID = mastersComboBox.Text;  

                        //currentReq.
                        //db.Requests.AddOrUpdate(currentReq);
                        //db.SaveChanges();

                        this.Close();
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
