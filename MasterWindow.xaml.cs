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

namespace techo
{
   
    public partial class MasterWindow : Window
    {
        int masterID;
        public MasterWindow(int id)
        {
            InitializeComponent();
            masterID = id; 

            LoadRequests();
        }


        public void LoadRequests()
        {
            try
            {
                RepairRequestsListBox.Items.Clear();
                using (techoEntities db = new techoEntities())
                {

                    var requests = db.Requests.Where(r => r.ReqClient.Any(rc => rc.MasterID == masterID)).ToList();
                    StringBuilder messageBuilder = new StringBuilder();

                    foreach (var request in requests)
                    {
                        string HomeTechType = request.HomeTechType.Any() ? request.HomeTechType.First().homeTechType1 : "";
                        string homeTechModel = request.HomeTechType.Any() ? request.HomeTechType.First().homeTechModel : "";
                        string statusDescription = request.Statuses.Any() ? request.Statuses.First().StatusDescription : "";
                        string com = request.Comments.Any() ? request.Comments.First().Message : "";

                        messageBuilder.AppendLine($"Request currentID: {request.RequestID}");
                        messageBuilder.AppendLine($"Start Date: {request.StartDate}");
                        messageBuilder.AppendLine($"Home Tech Type: {HomeTechType}");
                        messageBuilder.AppendLine($"Home Tech Model: {homeTechModel}");
                        messageBuilder.AppendLine($"Problem Description: {request.ProblemDescription}");
                        messageBuilder.AppendLine($"Repair Parts: {request.RepairParts}");
                        messageBuilder.AppendLine($"Status Description: {statusDescription}");
                        messageBuilder.AppendLine($"Comment: {com}");
                        messageBuilder.AppendLine();

                        RepairRequestsListBox.Items.Add(
                            $"Request currentID: {request.RequestID}\n" +
                            $"Start Date: {request.StartDate}\n" +
                            $"Home Tech Type: {HomeTechType}\n" +
                            $"Home Tech Model: {homeTechModel}\n" +
                            $"Problem Description: {request.ProblemDescription}\n" +
                            $"Repair Parts: {request.RepairParts}\n" +
                            $"Status Description: {statusDescription}\n" + 
                            $"Comment: {com} \n\n"
                        );


                    }
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка " + ex.Message);
            }
        


    }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MasterCommit requestsMaster = new MasterCommit(masterID);
            requestsMaster.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadRequests();
            RepairRequestsListBox.Items.Refresh();
        }
    }
}
