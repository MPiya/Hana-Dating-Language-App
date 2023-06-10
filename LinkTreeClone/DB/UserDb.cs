
using LinkTreeClone.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LinkTreeClone.DB
{
    public class UserDb
    {
        public string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=LinkTreeClone;Trusted_Connection=True;";
        public List<UserProfile> GetUser()
        {
           

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM UserProfile";
                var users = connection.Query<UserProfile>(query).AsList();

                return users;
            }
        }

        public void CreateUser(UserProfile userProfile)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO UserProfile (Name, Age, InstagramAccount) VALUES (@Name, @Age, @InstagramAccount)";
                connection.Execute(query, userProfile);
            }
        }

    }

}
