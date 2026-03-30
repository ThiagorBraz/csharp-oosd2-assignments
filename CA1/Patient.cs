using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OOSD2_CA1___Thiago_Braz
{
    // Enum representing the four possible blood types
    public enum BloodType
    {
        A,
        B,
        AB,
        O
    }
    public class Patient : INotifyPropertyChanged
    {
        private string _name;
        private DateTime? _birthDate;
        private BloodType _bloodType;

        //Constructor to initialize patients properties

        public Patient(string name, DateTime birthDate, BloodType bloodType)
        {
            Name = name;
            BirthDate = birthDate;
            BloodType = bloodType;
        }

        // Property for the patient's name with change notification
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        // Property for the patient's birth date with change notification
        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
                OnPropertyChanged(nameof(Age)); // Updates age when birth date changes
            }
        }

        // Property to Calculate the patient's age
        public int Age
        {
            get
            {
                if (!BirthDate.HasValue)
                    return 0;

                DateTime today = DateTime.Today;
                int age = today.Year - BirthDate.Value.Year;

                // Adjusts the age if the birthday has not occurred this year
                if (today < BirthDate.Value.AddYears(age))
                    age--;

                return age;
            }
        }

        // Property for the patient's blood type with change notification
        public BloodType BloodType
        {
            get => _bloodType;
            set { _bloodType = value; OnPropertyChanged(nameof(BloodType)); }
        }

        // Returns a formatted string representation of the patient
        public override string ToString()
        {
            return $"{Name} ({Age}) Type: {BloodType}";
        }

        // Event to notify when a property value changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to raise the PropertyChanged event
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
