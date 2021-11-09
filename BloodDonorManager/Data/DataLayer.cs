using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BloodDonorManager.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BloodDonorManager.Data
{
    public class DataLayer
    {
        private SQLiteConnection connection;

        public string ErrorMessage { get; set; }

        public DataLayer()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            connection = new SQLiteConnection(Path.Combine(path, "donors.db"));
            CreateAndSeedData();
        }

        public void CreateAndSeedData()
        {
            try
            {
                connection.CreateTable<User>();
                connection.CreateTable<Donor>();
                User user = new User() { UserName = "emma", Password = "emma@12345" };
                connection.Insert(user);
                connection.Insert(new Donor { UserName = "emma", BloodGroup = "AB+", City = "Glendene", Country = "New Zealand", Email = "iaoyk0v6zm@temporary-mail.net", Fullname = "Nancy M Willits", Phone = "0295298153" });
                connection.Insert(new Donor { UserName = "emma", BloodGroup = "O+", City = "Marshland", Country = "New Zealand", Email = "mellissa@gmail.com", Fullname = "Mellissa R Player", Phone = "0283659971" });
                connection.Insert(new Donor { UserName = "emma", BloodGroup = "O-", City = "Kelburn", Country = "New Zealand", Email = "walterhalford234@gmail.com", Fullname = "Walter S Halford", Phone = "0226890653" });
            }
            catch (Exception ex)
            {

            }
        }

        public bool CheckUser(string username, string password)
        {
            List<User> users = connection.Query<User>("Select * from User");
            if (users != null && users.Count > 0)
            {
                foreach (User user in users)
                {
                    if (user.UserName.Equals(username) && user.Password.Equals(password))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AddUser(User user)
        {
            try
            {
                connection.Insert(user);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool AddDonor(Donor donor)
        {
            try
            {
                connection.Insert(donor);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        
        public List<Donor> GetAllDonors()
        {
            List<Donor> donors = connection.Query<Donor>("Select * from Donor");
            return donors;
        }

        public List<Donor> GetAllDonorsOfUser(string username)
        {
            List<Donor> donors = new List<Donor>();
            List<Donor> all_donors = GetAllDonors();
            if (all_donors != null && all_donors.Count > 0)
            {
                foreach (Donor donor in all_donors)
                {
                    if (donor.UserName.Equals(username))
                    {
                        donors.Add(donor);
                    }
                }
            }
            return donors;
        }

        
        public bool DeleteDonor(Donor donor)
        {
            try
            {
                connection.Delete(donor);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }


    }
}