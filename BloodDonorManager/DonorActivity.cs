using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using BloodDonorManager.Data;
using BloodDonorManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodDonorManager
{
    [Activity(Label = "Blood Donors")]
    public class DonorActivity : AppCompatActivity
    {
        string username;
        RecyclerView donorsRecyclerView;
        DonorsAdapter donorsAdapter;
        List<Donor> listOfDonors;
        NewDonorFragment newDonorFragment;
        DataLayer handler;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.donor_page);
            username = Intent.GetStringExtra("username");
            handler = new DataLayer();
            donorsRecyclerView = (RecyclerView)FindViewById(Resource.Id.donorsRecyclerView);
            FloatingActionButton fab = (FloatingActionButton)FindViewById(Resource.Id.fab);
            fab.Click += Fab_Click;
            CreateData();
            SetupRecyclerView();
        }

        private void Fab_Click(object sender, System.EventArgs e)
        {
            newDonorFragment = new NewDonorFragment();
            var trans = SupportFragmentManager.BeginTransaction();
            newDonorFragment.Show(trans, "new donor");
            newDonorFragment.OnDonorRegistered += NewDonorFragment_OnDonorRegistered;
        }

        private void NewDonorFragment_OnDonorRegistered(object sender, NewDonorFragment.DonorDetailsEventArgs e)
        {
            if (newDonorFragment != null)
            {
                newDonorFragment.Dismiss();
                newDonorFragment = null;
            }
            e.Donor.UserName = username;
            listOfDonors.Insert(0, e.Donor);            
            handler.AddDonor(e.Donor);
            donorsAdapter.NotifyItemInserted(0);
        }

        void CreateData()
        {
            listOfDonors = handler.GetAllDonorsOfUser(username);
        }

        void SetupRecyclerView()
        {
            donorsRecyclerView.SetLayoutManager(new LinearLayoutManager(donorsRecyclerView.Context));
            donorsAdapter = new DonorsAdapter(listOfDonors);
            donorsAdapter.ItemClick += DonorsAdapter_ItemClick;
            donorsAdapter.CallClick += DonorsAdapter_CallClick;
            donorsAdapter.EmailClick += DonorsAdapter_EmailClick;
            donorsAdapter.DeleteClick += DonorsAdapter_DeleteClick;
            donorsRecyclerView.SetAdapter(donorsAdapter);
        }

        private void DonorsAdapter_DeleteClick(object sender, DonorsAdapterClickEventArgs e)
        {
            var donor = listOfDonors[e.Position];
            Android.Support.V7.App.AlertDialog.Builder DeletAlert = new Android.Support.V7.App.AlertDialog.Builder(this);
            DeletAlert.SetMessage("Are you sure");
            DeletAlert.SetTitle("Delete Donor");

            DeletAlert.SetPositiveButton("Delete", (alert, args) =>
            {
                Donor donor = listOfDonors[e.Position];
                listOfDonors.RemoveAt(e.Position);
                donorsAdapter.NotifyItemRemoved(e.Position);
                handler.DeleteDonor(donor);
            });

            DeletAlert.SetNegativeButton("Cancel", (alert, args) =>
            {
                DeletAlert.Dispose();
            });

            DeletAlert.Show();

        }

        private void DonorsAdapter_EmailClick(object sender, DonorsAdapterClickEventArgs e)
        {

            var donor = listOfDonors[e.Position];
            Android.Support.V7.App.AlertDialog.Builder EmailAlert = new Android.Support.V7.App.AlertDialog.Builder(this);
            EmailAlert.SetMessage("Send Mail to " + donor.Fullname);

            EmailAlert.SetPositiveButton("Send", (alert, args) =>
            {
                // Send Email
                Intent intent = new Intent();
                intent.SetType("plain/text");
                intent.SetAction(Intent.ActionSend);
                intent.PutExtra(Intent.ExtraEmail, new string[] { donor.Email });
                intent.PutExtra(Intent.ExtraSubject, "Enquiry on your availability for blood donation");
                StartActivity(intent);
            });

            EmailAlert.SetNegativeButton("Cancel", (alert, args) =>
            {
                EmailAlert.Dispose();
            });

            EmailAlert.Show();
        }

        private void DonorsAdapter_CallClick(object sender, DonorsAdapterClickEventArgs e)
        {
            var donor = listOfDonors[e.Position];

            Android.Support.V7.App.AlertDialog.Builder CallAlert = new Android.Support.V7.App.AlertDialog.Builder(this);
            CallAlert.SetMessage("Call " + donor.Fullname);

            CallAlert.SetPositiveButton("Call", (alert, args) =>
            {
                var uri = Android.Net.Uri.Parse("tel:" + donor.Phone);
                var intent = new Intent(Intent.ActionDial, uri);
                StartActivity(intent);
            });

            CallAlert.SetNegativeButton("Cancel", (alert, args) =>
            {
                CallAlert.Dispose();
            });

            CallAlert.Show();
        }

        private void DonorsAdapter_ItemClick(object sender, DonorsAdapterClickEventArgs e)
        {
            Toast.MakeText(this, "Row was clicked", ToastLength.Short).Show();

        }
    }
}