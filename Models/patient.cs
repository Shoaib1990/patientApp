using System;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations; // creattive programming
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace patientApp.Models{

    public class Patient{
        // database connectivity variables
        private MySqlConnection dbConnection;
        private MySqlCommand dbCommand;

        public Patient() {
            populateDefault();
        }

        [Key]
        public int OPD {get;set;}

        public string regDate {get;set;}
        // name: data notations
        [Required]
        [MaxLength(100)]
        [Display(Name = "patient name")]
        public string fullName {get;set;}
        [Required]
        [MaxLength(13)]
        [Display(Name = "contact number")]
        [RegularExpression(@"^(\d{3}-)?\d{3}-\d{4}$", ErrorMessage="Phone number should be in format 111-111-1111")]
        public string contactNum {get;set;}
        [Required]
        [MaxLength(25)]
        [Display(Name = "city name")]
        public string city {get;set;}
        public string dateOfBirth {get;set;}
        [Required]
        [MaxLength(25)]
        [Display(Name = "nationality")]
        public string nationality {get;set;}
        [Required]
        [MaxLength(25)]
        [Display(Name = "email")]
        [RegularExpression(@"^\S+@\S+$", ErrorMessage="Email should be in format shoaib.mandavia@yahoo.com")]
        public string email {get;set;}
        [Required]
        [MaxLength(10)]
        [Display(Name = "gender")]
        public string gender {get;set;}

    // -------------------------------------------------------- public methods
        public int create() {
            try {
                // open connection
                dbConnection = new MySqlConnection(Connection.CONNECTION_STRING);
                dbConnection.Open();

                string sqlString = "INSERT INTO tblPatients " +
                "(OPD,Date,Name,Contact,Email,Gender,DOB,City,Nationality) VALUES " +
                "(?OPD,?Date,?Name,?Contact,?Email,?Gender,?DOB,?City,?Nationality)";

                // Populate Command Object
                dbCommand = new MySqlCommand(sqlString,dbConnection);
                dbCommand.Parameters.AddWithValue("?OPD", OPD);
                dbCommand.Parameters.AddWithValue("?Date", regDate);
                dbCommand.Parameters.AddWithValue("?Name", fullName);
                dbCommand.Parameters.AddWithValue("?Contact", contactNum);
                dbCommand.Parameters.AddWithValue("?Email", email);
                dbCommand.Parameters.AddWithValue("?Gender", gender);
                dbCommand.Parameters.AddWithValue("?DOB", dateOfBirth);
                dbCommand.Parameters.AddWithValue("?City", city);
                dbCommand.Parameters.AddWithValue("?Nationality", nationality);
                dbCommand.ExecuteNonQuery();

                dbCommand.Parameters.Clear();

                // sqlString = "SELECT @@identity";
                // dbCommand.CommandText = sqlString;
                // customerID = Convert.ToInt32(dbCommand.ExecuteScalar());

            } finally {
                dbConnection.Close();
            }

            return OPD;
        }

        public bool delete() {
            if(OPD == 0) {
                return false;
            } else {
                try {
                    dbConnection = new MySqlConnection(Connection.CONNECTION_STRING);
                    dbConnection.Open();
                    string sqlString = "DELETE FROM tblPatients WHERE OPD = ?OPD";

                    // populate command object
                    dbCommand = new MySqlCommand(sqlString,dbConnection);
                    dbCommand.Parameters.AddWithValue("?OPD", OPD);
                    dbCommand.ExecuteNonQuery();
                }   
                finally {
                    dbConnection.Close();
                }
                return true;
            }


        }

            public void populateDefault() {
                OPD = 0;
                regDate = "";
                fullName = "";
                contactNum = "";
                city = "";
                dateOfBirth = "";
                nationality = "";
                email = "";
                gender = "";
            }
        
    }
}