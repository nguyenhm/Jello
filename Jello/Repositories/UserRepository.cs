﻿using Jello.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MvcApplication1.Services
{
    public class UserRepository
    {
        string _connStr = ConfigurationManager.ConnectionStrings["JelloConnectionString"].ConnectionString;

        public List<User> GetAllUser(int boardID)
        {
            List<User> users = new List<User>();

            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "UserGetAll",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@BoardID", SqlDbType.Int).Value = boardID;

                connection.Open();
                using (IDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        users.Add(FillModel(dr));
                    }
                }
            }
            return users;
        }


        public bool IsLoginValid(string email, string password)
        {
            bool isLoginValid = false;
            var cryto = new SimpleCrypto.PBKDF2();
            var user = GetUserByEmail(email);
            string salt = user.Salt;
            string encryptedPass = "";

            if (salt == null)
                throw new Exception("User is not registered");

            try
            {
                encryptedPass = cryto.Compute(password, salt);

                using (var connection = new SqlConnection(_connStr))
                {
                    var command = new SqlCommand
                    {
                        Connection = connection,
                        CommandText = "UserGetByEmailPass",
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.Add("@Email", SqlDbType.VarChar, 255).Value = email;
                    command.Parameters.Add("@Password", SqlDbType.VarChar, 255).Value = encryptedPass;

                    connection.Open();
                    using (IDataReader dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            isLoginValid = true;
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Failed to connect to the Database");
            }



            return isLoginValid;
        }

        public void CreateNewAccount(string username, string firstName, string lastName, string email, string password, string salt)
        {
            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "UserRegistration",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("Username", username);
                command.Parameters.AddWithValue("Fname", firstName);
                command.Parameters.AddWithValue("Lname", lastName);
                command.Parameters.AddWithValue("Email", email);
                command.Parameters.AddWithValue("Password", password);
                command.Parameters.AddWithValue("Salt", salt);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public User GetUserByEmail(string email)
        {
            User user = new User();

            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "UserGetByEmail",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@Email", SqlDbType.VarChar, 150).Value = email;

                connection.Open();
                using (IDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        user = FillModel(dr);
                    }
                }
            }
            return user;
        }

        private User FillModel(IDataReader dr)
        {
            var user = new User
            {
                UserID = (int)dr["UserID"],
                FName = dr["FName"] != DBNull.Value ? dr["FName"].ToString() : null,
                LName = dr["LName"] != DBNull.Value ? dr["LName"].ToString() : null,
                Email = (string)dr["Email"],
                Password = (string)dr["Password"],
                Salt = (string)dr["Salt"]
            };

            return user;
        }
    }
}