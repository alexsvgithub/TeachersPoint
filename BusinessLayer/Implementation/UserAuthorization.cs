using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TeachersPoint.BusinessLayer.Interface;
using TeachersPoint.Core.RequestDto;
using TeachersPoint.DataAccessLayer.Interface;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace TeachersPoint.BusinessLayer.Implementation
{
    public class UserAuthorization: IUserAuthorization
    {
        private readonly ISqlQueryResolver _sqlQueryResolver;

        public UserAuthorization(ISqlQueryResolver sqlQueryResolver)
        {
            _sqlQueryResolver = sqlQueryResolver;
        }
        public JsonResult RegisterNewUser(UserDto user)
        {
            //Check if the same email Exist in the database
            //if exist then procede else 
            var query = @$"SELECT EXISTS (
                            SELECT 1
                            FROM users
                            WHERE email = '{user.email}'
                        );";
            var ifEmailExist = _sqlQueryResolver.ResolveSqlQuery(query);
            var a = JsonConvert.SerializeObject(ifEmailExist);
            if (!Convert.ToBoolean(ifEmailExist.Rows[0]["exists"]))
            {
                //Getting Hashed Password
                string hashedPassword = GenerateSaltedAndHashedPassword(user.password);
                
                var queryToInsertNewEntryInDB = $@" Insert into users(email,password)
	                                                values('{user.email}','{hashedPassword}');";

                return new JsonResult(_sqlQueryResolver.ResolveSqlQuery(queryToInsertNewEntryInDB));

            } 
            else
            {
                return new JsonResult("User Already exists");
            }
           

        }
        

        public string LogIn(UserDto user)
        {
            var query = @$"SELECT * 
                            FROM users
                            WHERE email = '{user.email}'
                        ";
            var ifEmailExist = _sqlQueryResolver.ResolveSqlQuery(query);
            var a = JsonConvert.SerializeObject(ifEmailExist);
            if (ifEmailExist.Rows.Count > 0)
            {
                //Fetch the password from db
                string passwordFromDB = Convert.ToString(ifEmailExist.Rows[0]["password"]);

                byte[] storedSalt = ParseHexStringToByteArray(passwordFromDB.Split(':')[0]);

                string hashedPasswordVerify = HashPassword(user.password, storedSalt);

                if(passwordFromDB.Split(':')[1] == hashedPasswordVerify)
                {
                    return "User Authenticated, SUCCESS";
                }
                else
                {
                    return "Invalid Password";
                }
            }
            else
            {
                return ("User is not registered");
            }

        }

        private string GenerateSaltedAndHashedPassword(string password)
        {
            //Generate a random salt
            byte[] salt = GenerateSalt();

            //Salt to Hex
            string saltHexString = BitConverter.ToString(salt).Replace("-", "");

            //Hash the password with the salt
            string hashedPassword = HashPassword(password, salt);

            //Store the PassowrdInDB in SQL
            return $"{saltHexString}:{hashedPassword}";
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
