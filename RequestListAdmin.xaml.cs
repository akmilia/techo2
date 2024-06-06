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
        private void LoadRequests()
        {
            try
            {
                using (techoEntities db = new techoEntities())
                {
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EditRequestAdmin addRequest = new EditRequestAdmin();
            addRequest.Show();
            this.Close();
        }
    }
}
