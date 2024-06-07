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

namespace techo
{
    /// <summary>
    /// Логика взаимодействия для Requests.xaml
    /// </summary>
    public partial class RequestsList : Window
    {   
        private int ID;
        
        public RequestsList(int id)
        {
            InitializeComponent();
            LoadRequests(); 


            ID = id;
            //RepairRequestsDataGrid.Columns.Add("RequestID"); 
        }

        public void LoadRequests()
        {
            try
            {
                using (techoEntities db = new techoEntities())
                {
                    //var requests = db.Requests
                    // .Join(db.ReqClient, r => r.RequestID, rc => rc.RequestID, (r, rc) => r) 
                    // .Where(rc.RequestID != 0)
                    // .Join(db.HomeTechType, r => r.HomeTechID, t => t.HomeTechID, (r, t) => new
                    // {
                    //     RequestID = r.RequestID,
                    //     TypeName = t.homeTechType1,
                    //     ModelName = t.homeTechModel,
                    //     StartDate = r.StartDate,
                    //     ProblemDescription = r.ProblemDescription,
                    //     RepairParts = r.RepairParts,
                    //     StatusName = r.StatusID == null ? null : db.Statuses.Find(r.StatusID).StatusDescription,

                    // }); 

                    var requests = db.Requests.Where(r=> r.ReqClient.Any(rc=>rc.ClientID == 7)).ToList();
      


                    //  var requests = db.Requests
                    //.Join(db.HomeTechType, r => r.HomeTechID, t => t.HomeTechID, (r, t) => new
                    //{
                    //    RequestID = r.RequestID,
                    //    TypeName = t.homeTechType1,
                    //    ModelName = t.homeTechModel,
                    //    StartDate = r.StartDate,
                    //    ProblemDescription = r.ProblemDescription,
                    //    RepairParts = r.RepairParts,
                    //    //StatusName = r.StatusID == null ? null : db.Statuses.Find(r.StatusID).StatusDescription,
                    //})
                    ////.Where(r => r.RequestID == ID)
                    //.ToList();

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
