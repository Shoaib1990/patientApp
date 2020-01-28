using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace patientApp.Models
{
    public class WebLogin{
        

        private MySqlConnection dbConnection;
		private string connectionString;
		private MySqlCommand dbCommand;
		private MySqlDataReader dbReader;
		private HttpContext context;

		private string _username;
		private string _password;
		private bool _access;

		public WebLogin(string myConnectionString, HttpContext myHttpContext) {
			connectionString = myConnectionString;
			context = myHttpContext;

			// initialize properties
			_username = "";
			_password = "";
			_access = false;

			// clear out the session each time object is constructed
			context.Session.Clear();
		}

		// ------------------------------------------------------- get/sets
		public string username {
			set {
				_username = (value == null ? "" : value);
			}
		}

		public string password {
			set {
				_password = (value == null ? "" : value);
			}
		}

		public bool access {
			get {
				return _access;
			}
		}

		// ------------------------------------------------------- public methods
		public bool unlock() {
			// assume no access
			_access = false;

			// server side validation
			_username = truncate(_username, 10);
			_password = truncate(_password, 10);

			try {
				dbConnection = new MySqlConnection(connectionString);
				dbConnection.Open();
				dbCommand = new MySqlCommand("SELECT password,salt FROM tblLogin WHERE username=?username", dbConnection);
				dbCommand.Parameters.AddWithValue("?username", _username);
				dbReader = dbCommand.ExecuteReader();

				// username exists?
				if (!dbReader.HasRows) {
					dbConnection.Close();
					return _access;
				}

				// move on to the first record of data reader
				dbReader.Read();

				// hash and salt the user's password
				string hashedPassword = getHashed(_password, dbReader["salt"].ToString());
				// access granted?
				if (hashedPassword == dbReader["password"].ToString()) {
					_access = true;
					// user has access! Store it in the session
					context.Session.SetString("auth", "true");
					context.Session.SetString("user", _username);
				}

			} finally {
				dbConnection.Close();
			}

			return _access;
		}

		// ------------------------------------------------------- private methods
		private string getSalt() {
			// generate a 128-bit salt using a secure PRNG (pseudo-random number generator)
			byte[] salt = new byte[128 / 8];
			using (var rng = RandomNumberGenerator.Create()) {
				rng.GetBytes(salt);
			}
			//Console.WriteLine(">>> Salt: " + Convert.ToBase64String(salt));

			return Convert.ToBase64String(salt);
		}

		private string getHashed(string myPassword, string mySalt) {
			// convert string to Byte[] for hashing
			byte[] salt = Convert.FromBase64String(mySalt);
	
			// hashing done using PBKDF2 algorithm
			// derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
			string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: myPassword,
				salt: salt,
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 10000,
				numBytesRequested: 256 / 8));
			//Console.WriteLine(">>> Hashed: " + hashed);

			return hashed;
		}

		private string truncate(string value, int maxLength) {
			return value.Length <= maxLength ? value : value.Substring(0,maxLength);
		}

    }
}