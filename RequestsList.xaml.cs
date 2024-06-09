using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace techo
{
    public partial class RequestsList : Window
    {
        int currentID; 
        
        public RequestsList(int id)
        {
            InitializeComponent(); 


            currentID = id;
            LoadRequests(); 
        }

        public void LoadRequests()
        {
            try
            {
                using (techoEntities db = new techoEntities())
                {

                    var requests = db.Requests.Where(r => r.ReqClient.Any(rc => rc.ClientID == currentID)).ToList();
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
                        messageBuilder.AppendLine(); 

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
        public void AddRequestButton_Click(object sender, RoutedEventArgs e)
        {
            AddRequest addRequest = new AddRequest(currentID);
            addRequest.Show();
            
        }
        public void EditRequestButton_Click(object sender, RoutedEventArgs e)
        {
            EditRequest addRequest = new EditRequest(currentID);
            addRequest.Show();
        }
        public void ReviewReequestButton_Click(object sender, RoutedEventArgs e)
        {
            Review addReview = new Review();
            addReview.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadRequests();
            RepairRequestsListBox.Items.Refresh();
        }
    }
}
