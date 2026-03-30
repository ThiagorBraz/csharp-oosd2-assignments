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

namespace CA2___Thiago_Braz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Bike> allBikes = new List<Bike>();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateBikes();
            lbxBikes.ItemsSource = allBikes;
        }

        private void GenerateBikes()
        {
            
            var motorBike1 = new MotorBike("Yamaha", "FZ", new DateTime(2020, 5, 10), new Engine(FuelType.Petrol, 150), 15, 180);
            var motorBike2 = new MotorBike("Harley-Davidson", "Iron 883", new DateTime(2021, 7, 15), new Engine(FuelType.Petrol, 100), 12, 160);

            var electricBike1 = new ElectricBike("Tesla", "Model S Electric Bike", new DateTime(2022, 3, 20), new Engine(FuelType.Electric, 50), 3, 80);
            var electricBike2 = new ElectricBike("Rad Power Bikes", "RadRover 6 Plus", new DateTime(2023, 2, 5), new Engine(FuelType.Electric, 60), 4, 90);

            var traditionalBike1 = new TraditionalBike("Schwinn", "Classic Cruiser", new DateTime(2019, 8, 12), true, true);
            var traditionalBike2 = new TraditionalBike("Trek", "Marlin 7", new DateTime(2020, 4, 10), false, true);

            allBikes.Add(motorBike1);
            allBikes.Add(motorBike2);
            allBikes.Add(electricBike1);
            allBikes.Add(electricBike2);
            allBikes.Add(traditionalBike1);
            allBikes.Add(traditionalBike2);
        }

        private void Filter_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            string tag = rb.Tag.ToString();

            if (tag == "All")
            {
                lbxBikes.ItemsSource = allBikes;
            }
            else
            {
                lbxBikes.ItemsSource = allBikes.Where(b => b.GetType().Name == tag).ToList();
            }

            imgBike.Source = null;
            txtBikeDetails.Text = "";
        }

        private void lbxBikes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bike selected = lbxBikes.SelectedItem as Bike;
            if (selected != null)
            {
                txtBikeDetails.Text = selected.DisplayInfo();
                imgBike.Source = new BitmapImage(new Uri("Images/" + selected.Image, UriKind.Relative));
            }
        }
    }
}
