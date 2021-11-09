using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodDonorManager.Models
{
    public class Donor
    {
        [PrimaryKey, AutoIncrement]
        public int DonorID { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string BloodGroup { get; set; }
        public string UserName { get; set; }
    }
}