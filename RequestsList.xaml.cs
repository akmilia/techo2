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

namespace techo
{
    /// <summary>
    /// Логика взаимодействия для Requests.xaml
    /// </summary>
    public partial class RequestsList : Window
    {   
        private int ID = 0; 

        public RequestsList(int id)
        {
            InitializeComponent();
            LoadRequests(); 

            ID = id;
            //RepairRequestsDataGrid.Columns.Add("RequestID"); 
        }

        private void LoadRequests()
        {
            try
            {
                using (techoEntities db = new techoEntities())
                {
                    // var orders = db.Requests.Where(u => u.ReqClient == ID).ToList();
                    //foreach (var user in orders)
                    //{
                    //    var newRow = new DataGridRow();
                    //    newRow.Cells.Add(new DataGridTextColumn { Header = "Id", Value = user.IdЗаказы });
                    //    newRow.Cells.Add(new DataGridTextColumn { Header = "Книга", Value = book.Название });

                    //    RepairRequestsDataGrid.Items.Add(newRow);

                    //}
                    var requests = db.Requests
                      .Join(db.HomeTechType, r => r.HomeTechID, t => t.HomeTechID, (r, t) => new
                      {
                          RequestID = r.RequestID,
                          TypeName = t.homeTechType1,
                          ModelName = t.homeTechModel,
                          StartDate = r.StartDate,
                          ProblemDescription = r.ProblemDescription,
                          RepairParts = r.RepairParts,
                          //StatusName = r.StatusID == null ? null : db.Statuses.Find(r.StatusID).StatusDescription,
                          //
                      })
                        .ToList();



                    RepairRequestsDataGrid.ItemsSource = requests;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Возникла ошибка: {ex.Message}");
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
