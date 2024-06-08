using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace techo
{
    public partial class RequestsList : Window
    {
        int currentID; 
        
        public RequestsList(int id)
        {
            InitializeComponent();
            LoadRequests(); 

             currentID = id;
            MessageBox.Show(currentID.ToString());
        }


        //var requests = db.Requests.Where(r => r.ReqClient.Any(rc => rc.ClientID == currentID))
        //.Join(db.Statuses, r => r.StatusID, st => st.StatusID, (r, st) => new { r, st })
        //.Join(db.HomeTechType, rt => rt.r.HomeTechID, t => t.HomeTechID, (rt, t) => new
        //{
        //    RequestID = rt.r.RequestID,
        //    TypeName = t.homeTechType1,
        //    ModelName = t.homeTechModel,
        //    StartDate = rt.r.StartDate,
        //    ProblemDescription = rt.r.ProblemDescription,
        //    RepairParts = rt.r.RepairParts,
        //    //StatusName = rt.st.StatusDescription, // Use rt.st instead of st
        //}).ToList();
        public void LoadRequests()
        {
            try
            {
                using (techoEntities db = new techoEntities())
                {
                    MessageBox.Show("1");
                    //MessageBox.Show(currentID.ToString());
                    //var requests = db.Requests
                    //    .Where(r => r.ReqClient.Any(rc => rc.ClientID == this.currentID))
                    //    .ToList(); 
                    var requests = db.Requests.Where(r => r.ReqClient.Any(rc => rc.ClientID == 7)).ToList();
                    RepairRequestsDataGrid.ItemsSource = requests; 

                    //MessageBox.Show(requests.ToString());
                    //StringBuilder messageBuilder = new StringBuilder();

                    //MessageBox.Show("2");
                    //foreach (var request in requests)
                    //{
                    //    MessageBox.Show("3");
                    //    string HomeTechType = request.HomeTechType.Any() ? request.HomeTechType.First().homeTechType1 : "";
                    //    string homeTechModel = request.HomeTechType.Any() ? request.HomeTechType.First().homeTechModel : "";
                    //    string statusDescription = request.Statuses.Any() ? request.Statuses.First().StatusDescription : "";

                    //    MessageBox.Show(HomeTechType + homeTechModel + statusDescription);

                    //    messageBuilder.AppendLine($"Request currentID: {request.RequestID}");
                    //    messageBuilder.AppendLine($"Start Date: {request.StartDate}");
                    //    messageBuilder.AppendLine($"Home Tech Type: {HomeTechType}");
                    //    messageBuilder.AppendLine($"Home Tech Model: {homeTechModel}");
                    //    messageBuilder.AppendLine($"Problem Description: {request.ProblemDescription}");
                    //    messageBuilder.AppendLine($"Repair Parts: {request.RepairParts}");
                    //    messageBuilder.AppendLine($"Status Description: {statusDescription}");
                    //    messageBuilder.AppendLine(); // Add a line break between requests

                    //    MessageBox.Show("4");

                    //    RepairRequestsListBox.Items.Add(
                    //        $"Request currentID: {request.RequestID}\n" +
                    //        $"Start Date: {request.StartDate}\n" +
                    //        $"Home Tech Type: {HomeTechType}\n" +
                    //        $"Home Tech Model: {homeTechModel}\n" +
                    //        $"Problem Description: {request.ProblemDescription}\n" +
                    //        $"Repair Parts: {request.RepairParts}\n" +
                    //        $"Status Description: {statusDescription}\n\n"
                    //    );
                    //    MessageBox.Show("5");

                    //    MessageBox.Show( messageBuilder.ToString() );
                   // }
                   //}

                //using (techoEntities db = new techoEntities())
                //{
                //    var requests = db.Requests
                //        .Where(r => r.ReqClient.Any(rc => rc.ClientID == this.currentID))
                //        .Join(db.Statuses, r => r.StatusID, st => st.StatusID, (r, st) => new { r, st })
                //        .Join(db.HomeTechType, rt => rt.r.HomeTechID, t => t.HomeTechID, (rt, t) => new
                //        {
                //            RequestID = rt.r.RequestID,
                //            TypeName = t.homeTechType1,
                //            ModelName = t.homeTechModel,
                //            StartDate = rt.r.StartDate,
                //            ProblemDescription = rt.r.ProblemDescription,
                //            RepairParts = rt.r.RepairParts,
                //            StatusName = rt.st.StatusDescription,
                //        })
                //        .ToList();

                //    StringBuilder messageBuilder = new StringBuilder();
                //    foreach (var request in requests)
                //    {
                //        string homeTechType = request.TypeName;
                //        string homeTechModel = request.ModelName;
                //        string statusDescription = request.StatusName;

                //        messageBuilder.AppendLine($"Request currentID: {request.RequestID}");
                //        messageBuilder.AppendLine($"Start Date: {request.StartDate}");
                //        messageBuilder.AppendLine($"Home Tech Type: {homeTechType}");
                //        messageBuilder.AppendLine($"Home Tech Model: {homeTechModel}");
                //        messageBuilder.AppendLine($"Problem Description: {request.ProblemDescription}");
                //        messageBuilder.AppendLine($"Repair Parts: {request.RepairParts}");
                //        messageBuilder.AppendLine($"Status Description: {statusDescription}");
                //        messageBuilder.AppendLine(); // Add a line break between requests
                //    }
                //    RepairRequestsDataGrid.Items.Add(messageBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        public void AddRequestButton_Click(object sender, RoutedEventArgs e)
        {
            AddRequest addRequest = new AddRequest();
            addRequest.Show();
            LoadRequests();
        }
        public void EditRequestButton_Click(object sender, RoutedEventArgs e)
        {
            EditRequest addRequest = new EditRequest();
            addRequest.Show();
            this.Close();
        }
    }
}
