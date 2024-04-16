using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TeachersPoint.BusinessLayer.Interface;
using TeachersPoint.Core.RequestDto;

namespace TeachersPoint.BusinessLayer.Implementation
{
    public class TestService : ITestService
    {
        private readonly ITestService _testService;
        //public TestService( ITestService testService) {
        //    _testService = testService;

        //}
        public string ThisIsTestServiceMethodCalling(string s)
        {
            Console.WriteLine(s);
            return "true";
        }

        public int RegisterNewUser(UserDto user)
        {
            //Check if the same email Exist in the database
            //if exist then procede else 


            // User's password
            string password = user.password;

            //Generate a random salt
            byte[] salt = GenerateSalt();

            //Salt to Hex
            string saltHexString = BitConverter.ToString(salt).Replace("-", "");

            //Hash the password with the salt
            string hashedPassword = HashPassword(password, salt);

            //Store the PassowrdInDB in SQL
            string PasswordInDB = $"{saltHexString}:{hashedPassword}";
            


            return 0;
        }

        static byte[] GenerateSalt()
        {
            const int saltSize = 16; // 16 bytes for a 128-bit salt
            byte[] salt = new byte[saltSize];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        static string HashPassword(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                // Concatenate password and salt
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] passwordWithSalt = new byte[passwordBytes.Length + salt.Length];
                Array.Copy(passwordBytes, passwordWithSalt, passwordBytes.Length);
                Array.Copy(salt, 0, passwordWithSalt, passwordBytes.Length, salt.Length);

                // Hash the concatenated password and salt
                byte[] hashedBytes = sha256.ComputeHash(passwordWithSalt);

                // Convert byte array to a base64 encoded string
                return Convert.ToBase64String(hashedBytes);
            }
        }



        public string LogIn(UserDto user)
        {
            //search DB with emailID


            //Fetch the password from db
            var passwordFromDB = "7C01E85FF4E7CC617B689E86BD1821EE:gedcVv7RazWffz8Nx/A7yByTt/seakeF+XZAOac2zJo=";

            byte[] storedSalt = ParseHexStringToByteArray(passwordFromDB.Split(':')[0]);

            string hashedPasswordVerify = HashPassword(user.password, storedSalt);

            return passwordFromDB.Split(':')[1] == hashedPasswordVerify ? "Success" : "Error";

        }

        static byte[] ParseHexStringToByteArray(string hexString)
        {
            // Remove any dashes from the hexadecimal string
            hexString = hexString.Replace("-", "");

            // Convert the hexadecimal string to a byte array
            byte[] byteArray = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                byteArray[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return byteArray;
        }

    }
}
