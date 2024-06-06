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
    /// Логика взаимодействия для Requests.xaml
    /// </summary>
    public partial class RequestsList : Window
    {
        public RequestsList()
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
                             .Include("HomeTechType")
                             .Include("Statuses")
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
