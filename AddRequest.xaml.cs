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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace techo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class    AddRequest : Window
    {
        public AddRequest()
        {
            InitializeComponent();
        }
        public void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (techoEntities db = new techoEntities())
                {
                    string type = TypeTextBox.Text;
                    string model = ModelTextBox.Text;
                    string problem = ProblemTextBox.Text;

                    if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(problem))
                    {
                        MessageBox.Show("Все поля обязательны для заполнения.");
                        return;
                    }

                    var homeTechType = db.HomeTechType.FirstOrDefault(ht => ht.homeTechType1 == type && ht.homeTechModel == model);
                    if (homeTechType == null)
                    {
                        homeTechType = new HomeTechType
                        {
                            homeTechType1 = type,
                            homeTechModel = model
                        };
                        db.HomeTechType.Add(homeTechType);
                    }

                    var newRequest = new Requests
                    {
                        HomeTechID = homeTechType.HomeTechID,
                        ProblemDescription = problem,
                        StartDate = DateTime.Now,
                        StatusID = 1 
                    };

                    db.Requests.Add(newRequest);
                    db.SaveChanges();
                    MessageBox.Show("Заявка успешно добавлена");

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Возникла ошибка: {ex.Message}");
            }
        }

    }
    }
