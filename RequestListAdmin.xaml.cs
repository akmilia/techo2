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
    /// <summary>
    /// Логика взаимодействия для RequestListAdmin.xaml
    /// </summary>
    public partial class RequestListAdmin : Window
    {
        public RequestListAdmin()
        {
            InitializeComponent();
            LoadRequests();
        }
        public void LoadRequests()
        {
            try
            {
                using (techoEntities db = new techoEntities())
                {
                  

                    var requests = db.Requests.ToList();

                    StringBuilder messageBuilder = new StringBuilder();

                    foreach (var request in requests)
                    {
                        string HomeTechType = request.HomeTechType.Any() ? request.HomeTechType.First().homeTechType1 : "";
                        string homeTechModel = request.HomeTechType.Any() ? request.HomeTechType.First().homeTechModel : "";
                        string statusDescription = request.Statuses.Any() ? request.Statuses.First().StatusDescription : ""; 

                        messageBuilder.AppendLine($"Request currentID: {request.RequestID}");
                        messageBuilder.AppendLine($"Start Date: {request.StartDate}");
                        messageBuilder.AppendLine($"Home Tech Type: {HomeTechType}");
                        messageBuilder.AppendLine($"Home Tech Model: {homeTechModel}");
                        messageBuilder.AppendLine($"Problem Description: {request.ProblemDescription}");
                        messageBuilder.AppendLine($"Repair Parts: {request.RepairParts}");
                        messageBuilder.AppendLine($"Status Description: {statusDescription}");
                        messageBuilder.AppendLine(); // Add a line break between requests

                        // Add formatted data to the ListBox
                        RepairRequestsListBox.Items.Add(
                            $"Request currentID: {request.RequestID}\n" +
                            $"Start Date: {request.StartDate}\n" +
                            $"Home Tech Type: {HomeTechType}\n" +
                            $"Home Tech Model: {homeTechModel}\n" +
                            $"Problem Description: {request.ProblemDescription}\n" +
                            $"Repair Parts: {request.RepairParts}\n" +
                         $"Status Description: {statusDescription}\n\n"
                        );


                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EditRequestAdmin addRequest = new EditRequestAdmin();
            addRequest.Show();
            this.Close();
        }
    }
}
