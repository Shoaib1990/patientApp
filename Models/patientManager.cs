using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace patientApp.Models{
    public class patientManager{
        // database connectivity variables
        private MySqlConnection dbConnection; 
        private MySqlCommand dbCommand;
        private MySqlDataReader dbReader;

        // property variables
        private List<Patient> _patients;

        public patientManager() {

            _patients = new List<Patient>();

            dbConnection = new MySqlConnection(Connection.CONNECTION_STRING);
            dbCommand = new MySqlCommand("", dbConnection);

        }

        public List<Patient> patients {
            get {
                return _patients;
            }
        }

    

    public void viewPatientDiary() {
        try {
            dbConnection.Open();

            dbCommand.CommandText = "SELECT * FROM tblPatients";
            dbReader = dbCommand.ExecuteReader();
            while (dbReader.Read()) {
                
                Patient patient = new Patient();
                patient.OPD = Convert.ToInt32(dbReader["OPD"]);
                patient.regDate = dbReader["Date"].ToString();
                patient.fullName = dbReader["Name"].ToString();
                patient.contactNum = dbReader["Contact"].ToString();
                patient.email = dbReader["Email"].ToString();
                patient.gender = dbReader["Gender"].ToString();
                patient.dateOfBirth = dbReader["DOB"].ToString();
                patient.city = dbReader["City"].ToString();
                patient.nationality = dbReader["Nationality"].ToString();
                _patients.Add(patient);
            }
            dbReader.Close();
        } finally {
            dbConnection.Close();
        }
    }

    public List<SelectListItem> getList() {
        // get all the patient 
        viewPatientDiary();

        List<SelectListItem> list = new List<SelectListItem>();
        foreach (Patient patient in patients) {
            SelectListItem item = new SelectListItem();
            item.Text = patient.OPD + " " + patient.fullName;
            item.Value = patient.OPD.ToString();
            list.Add(item);
        }
        return list;
    }

    public Patient viewPatientDiary(int OPD) {
        viewPatientDiary();
        return _patients.Find(item=>item.OPD == OPD); 
    }

    }


}
