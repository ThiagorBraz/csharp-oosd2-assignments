using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;

namespace OOSD2_CA1___Thiago_Braz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            UpdateWardHeader();

        }

        // ObservableCollection to update UI automatically
        public ObservableCollection<Ward> Wards { get; set; } = new ObservableCollection<Ward>();


        //Method to update the Ward Capacity textblock when the slider values changes
        public void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double val = SliderCapacity.Value;

            if (WardCapacity != null)
            {
                WardCapacity.Text = string.Format("{0:F0}", val);
            }
        }

        //method to update the Ward Header text
        private string _wardHeaderText = "Ward List (0)";
        public string WardHeaderText
        {
            get { return _wardHeaderText; }
            set
            {
                _wardHeaderText = value; OnPropertyChanged(nameof(WardHeaderText));
            }
        }

        private string _newWardname;
        public string NewWardName
        {
            get { return _newWardname; }
            set
            {
                _newWardname = value;
                OnPropertyChanged(nameof(NewWardName));
                OnPropertyChanged(nameof(IsAddWardEnable));
            }
        }

        public void UpdateWardHeader()
        {
            WardHeaderText = $"Ward List ({Wards.Count})";
        }


        // Method to add a new ward and reset the input field
        public void AddWard(object sender, RoutedEventArgs e)
        {
            string wardName = NewWardName;
            int wardCapacity = (int)Math.Round(SliderCapacity.Value);

            if (!string.IsNullOrWhiteSpace(wardName))
            {
                Ward newWard = new Ward(wardName, wardCapacity);
                Wards.Add(newWard);
                UpdateWardHeader();
                NewWardName = string.Empty;
            }
        }

        // Property to determine if the Add Ward button should be enabled

        public bool IsAddWardEnable => !string.IsNullOrWhiteSpace(NewWardName);


        // Property and method to add a new patient to a ward

        private DateTime _newPatientDateOfBirth = DateTime.Today;
        public DateTime NewPatientDateOfBirth
        {
            get { return _newPatientDateOfBirth; }
            set
            {
                if (value > DateTime.Today)
                {
                    MessageBox.Show("Invalid Birth Date.");
                    return;
                }

                _newPatientDateOfBirth = value;
                OnPropertyChanged(nameof(NewPatientDateOfBirth));
            }
        }

        private BloodType _newPatientBloodType;
        public BloodType NewPatientBloodType
        {
            get { return _newPatientBloodType; }
            set
            {
                _newPatientBloodType = value;
                OnPropertyChanged(nameof(NewPatientBloodType));

            }
        }

        // Method to add a new patient to a ward
        public void AddPatient(object sender, RoutedEventArgs e)
        {
            if (wardsList.SelectedItem is Ward selectWard)
            {
                if (selectWard.Patients.Count >= selectWard.WardCapacity)
                {
                    MessageBox.Show("This ward has reached its full capacity!");
                    return;
                }
                Patient newPatient = new Patient(NewPatientName, NewPatientDateOfBirth, NewPatientBloodType);

                selectWard.Patients.Add(newPatient);

                patientsList.ItemsSource = null;
                patientsList.ItemsSource = selectWard.Patients;

                NewPatientName = string.Empty;
                NewPatientDateOfBirth = DateTime.Today;

            }
        }

        // Property to determine if the Add Patient button should be enabled

        private string _newPatientName;
        public string NewPatientName
        {
            get { return _newPatientName; }
            set
            {
                _newPatientName = value;
                OnPropertyChanged(nameof(NewPatientName));
                OnPropertyChanged(nameof(IsAddPatientEnable));
            }
        }
        public bool IsAddPatientEnable => !string.IsNullOrWhiteSpace(NewPatientName);


        //method to show the Patient Details
        private void patientsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selectedPatient = (Patient)patientsList.SelectedItem;

            if (selectedPatient != null)
            {

                detailsName.Text = selectedPatient.Name;


                string bloodTypeImage = selectedPatient.BloodType.ToString() + ".png";


                string imagePath = $"Images/{bloodTypeImage}";


                if (System.IO.File.Exists(imagePath))
                {
                    detailsBloodImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                }
                else
                {

                    detailsBloodImage.Source = null;
                }


                detailsPanel.Visibility = Visibility.Visible;
            }
        }

        private void wardsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wardsList.SelectedItem is Ward selectedWard)
            {
                // Atualiza a ListBox de pacientes
                patientsList.ItemsSource = selectedWard.Patients;
            }
        }

        // Method to save data to a JSON file
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if the Wards list is not empty
            if (Wards.Count == 0)
            {
                MessageBox.Show("No wards to save.");
                return; // Don't proceed if there are no wards
            }

            //get string of objects - json formatted
            string json = JsonConvert.SerializeObject(Wards, Formatting.Indented);

            //write to the file
            using (StreamWriter sw = new StreamWriter(@"c:\json\WardsandPatients.json"))
            {
                sw.Write(json);
            }

            MessageBox.Show("Data saved successfully!");
        }
        // Method to load data from a JSON file
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = @"c:\json\WardsandPatients.json";

            if (File.Exists(filePath))

            {
                //conect to a file
                using (StreamReader sr = new StreamReader(@"c:\json\WardsandPatients.json"))
                {
                    //read text
                    string json = sr.ReadToEnd();
                    //convert from json to objects
                    Wards = JsonConvert.DeserializeObject<ObservableCollection<Ward>>(json);
                }
                // clear befor using ItemSource
                wardsList.ItemsSource = null;

                //refresh the display
                wardsList.ItemsSource = Wards;
                UpdateWardHeader();
            }

            else
            {
                MessageBox.Show("No saved Data found!");
            } 
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

