using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

                //var masters = db.Users
                //       .Where(r => r.TypeID == 2)
                //       .Select(r => new { r.FIO  })
                //       .ToList();

                //mastersComboBox.ItemsSource = masters; 
                var masters = db.Users
                  .Where(u => u.TypeID == 2)
                  .Select(u => new { u.UserID, u.FIO })
                  .ToList();
                mastersComboBox.ItemsSource = masters;
                mastersComboBox.DisplayMemberPath = "FIO";
                mastersComboBox.SelectedValuePath = "UserID";
            }
        }
     
        //?
        private void LoadRequestData()
        {
            try
            {
                using (techoEntities db = new techoEntities())
                {
                    var userIdOfMaster = currentReq.ReqClient.First().MasterID;
                    string masterFIO = db.Users.Where(u => u.UserID == userIdOfMaster).Select(u => u.FIO).FirstOrDefault();
                    string status = currentReq.Statuses.Any() ? currentReq.Statuses.First().StatusDescription : "No status available";

                    //MessageBox.Show("master id" + userIdOfMaster + "status" + status);  
                    //var masterFIO = currentReq.requestClientMaster.First().users.fio;
                    //responsiblesComboBox.SelectedItem = masterFIO;

                    mastersComboBox.Text = masterFIO;
                    statusesComboBox.Text = status;

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
                            string status = statusesComboBox.SelectedItem.ToString();
                            //MessageBox.Show("2 var status  " + status);
                           
                            int ms = (int)mastersComboBox.SelectedValue;
                            //MessageBox.Show(mastersComboBox.SelectedItem.ToString());
                    
                              int statusID = db.Statuses
                                    .Where(x => x.StatusDescription == status)
                                    .Select(x => x.StatusID)
                                    .FirstOrDefault();
                                currentReq.StatusID = statusID;
                                //MessageBox.Show("4");

                                var reqClient = db.ReqClient.FirstOrDefault(rc => rc.RequestID == currentReq.RequestID);
                                if (reqClient != null)
                                {
                                    //MessageBox.Show("5");
                                    reqClient.MasterID = ms; 
                                    //MessageBox.Show("6");
                                    db.Requests.AddOrUpdate(currentReq);
                                    //MessageBox.Show("7");
                                    db.ReqClient.AddOrUpdate(reqClient);
                                    //MessageBox.Show("8");
                                    db.SaveChanges();
                        MessageBox.Show("Request updated successfully.");
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
