using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OOSD2_CA1___Thiago_Braz
{
    public class Ward
    {
        //static counter to track the total of wards created
        public static int WardCount= 0;

        //name of the ward
        public string WardName { get; set; }

        //ward maximum capacity
        public int WardCapacity { get; set; }

        
        //List of patients assigned to the ward
        public ObservableCollection<Patient> Patients { get; set; }


        // contructor to initialize a new ward
        public Ward(string wardname, int wardcapacity)
        {
            WardCount++;
            WardName = wardname;
            WardCapacity = wardcapacity;
            Patients = new ObservableCollection<Patient>();
        }

        //Adds a patient to the ward if there is available space
        public bool AddPatient(Patient patient)
        {
            if (Patients.Count < WardCapacity)
            {
                Patients.Add(patient);
                return true;
            }
            return false;
        }

        // Returns a formatted string representation of the ward
        public override string ToString()
        {
            return string.Format("{0} Ward - Capacity: ({1}), Patients: {2}/{3}", WardName, WardCapacity, Patients.Count, WardCapacity);
        }

    }

}
