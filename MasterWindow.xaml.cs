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
    /// Логика взаимодействия для MasterWindow.xaml
    /// </summary>
    public partial class MasterWindow : Window
    {
        private int MasterID;
        public MasterWindow(int masterID)
        {
            InitializeComponent();
            this.MasterID = masterID;
            LoadRequests();
        }


        public void LoadRequests()
        {
            try
            {
                RepairRequestsListBox.Items.Clear();
                using (techoEntities db = new techoEntities())
                {
                    var requests = db.Requests.Include("Comments").ToList();

                    foreach (var request in requests)
                    {
                        StringBuilder messageBuilder = new StringBuilder();

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

                        // Добавляем комментарии к сообщению
                        messageBuilder.AppendLine("Comments:");
                        foreach (var comment in request.Comments)
                        {
                            messageBuilder.AppendLine($"- {comment.Message}");
                        }

                        messageBuilder.AppendLine();

                        RepairRequestsListBox.Items.Add(messageBuilder.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка " + ex.Message);
            }
        


    }

        private void ExButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MasterCommit requestsMaster = new MasterCommit();
            requestsMaster.Show();
            this.Close();
        }
    }
}
