using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace techo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AddRequest : Window
    {
        public AddRequest()
        {
            InitializeComponent();
            LoadHomeTechTypes();
        }

        public List<HomeTechType> homeTechTypes;

        public void LoadHomeTechTypes()
        {
            using (techoEntities db = new techoEntities())
            {
                // Загрузите все уникальные типы техники
                homeTechTypes = db.HomeTechType
                                  .GroupBy(ht => ht.homeTechType1)
                                  .Select(g => g.FirstOrDefault())
                                  .ToList();
                homeTechType1.ItemsSource = homeTechTypes;
            }
        }

        public void AddHomeTechType(string techType)
        {
            using (techoEntities db = new techoEntities())
            {
                // Проверьте, существует ли уже этот тип техники
                var existingType = db.HomeTechType.FirstOrDefault(ht => ht.homeTechType1 == techType);
                if (existingType == null)
                {
                    var newType = new HomeTechType
                    {
                        homeTechType1 = techType
                    };
                    db.HomeTechType.Add(newType);
                    db.SaveChanges();
                    LoadHomeTechTypes();
                }
            }
        }

        public void AddNewRequest(Requests newRequest)
        {
            using (techoEntities db = new techoEntities())
            {
                // Получите максимальный RequestID из существующих заявок
                int maxRequestID = db.Requests.Max(r => r.RequestID);

                newRequest.RequestID = maxRequestID + 1;

                db.Requests.Add(newRequest);
                db.SaveChanges();
                MessageBox.Show("Заявка успешно добавлена");

                this.Close();
            }
        }

        public void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (techoEntities db = new techoEntities())
                {
                    HomeTechType selectedType = homeTechType1.SelectedItem as HomeTechType;
                    if (selectedType == null)
                    {
                        string newType = newHomeTechType.Text;
                        if (!string.IsNullOrWhiteSpace(newType))
                        {
                            AddHomeTechType(newType);
                            selectedType = homeTechTypes.FirstOrDefault(ht => ht.homeTechType1 == newType);
                        }
                        else
                        {
                            MessageBox.Show("Выберите или введите тип техники.");
                            return;
                        }
                    }

                    string model = homeTechModel.Text;
                    string problem = problemDescription.Text;

                    if (string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(problem))
                    {
                        MessageBox.Show("Все поля обязательны для заполнения.");
                        return;
                    }

                    var newRequest = new Requests
                    {
                        HomeTechID = selectedType.HomeTechID,
                        ProblemDescription = problem,
                        StartDate = DateTime.Now,
                        StatusID = 3 // Новая заявка
                    };

                    AddNewRequest(newRequest);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Возникла ошибка: {ex.Message}");
            }
           
        }

        public void AddNewTypeButton_Click(object sender, RoutedEventArgs e)
        {
            string newType = newHomeTechType.Text;
            if (!string.IsNullOrWhiteSpace(newType))
            {
                AddHomeTechType(newType);
                MessageBox.Show("Новый тип техники добавлен.");
                newHomeTechType.Clear();
            }
            else
            {
                MessageBox.Show("Введите тип техники.");
            }
        }
    }
}
