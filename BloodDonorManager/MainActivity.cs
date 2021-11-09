using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.Design.Widget;
using BloodDonorManager.Data;
using Android.Content;
using BloodDonorManager.Models;

namespace BloodDonorManager
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText etuser, password;
        Button login;
        TextView register;
        bool isUserValid, isPasswordValid;
        TextInputLayout userError, passError;
        DataLayer handler;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.login_page);
            handler = new DataLayer();

            etuser = FindViewById<EditText>(Resource.Id.user);
            password = FindViewById<EditText>(Resource.Id.password);
            userError = FindViewById<TextInputLayout>(Resource.Id.userError);
            passError = FindViewById<TextInputLayout>(Resource.Id.passError);
            login = FindViewById<Button>(Resource.Id.login);
            register = FindViewById<TextView>(Resource.Id.register);
            login.Click += Login_Click;
            register.Click += Register_Click;
        }

        private void Register_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegisterActivity));
            StartActivity(intent);
            Finish();
        }

        private void Login_Click(object sender, System.EventArgs e)
        {
            if (etuser.Text.ToString().Length == 0)
            {
                userError.Error = Resources.GetString(Resource.String.name_error);
                isUserValid = false;
            }
            else
            {
                isUserValid = true;
                userError.ErrorEnabled = false;
            }

            if (password.Text.ToString().Length == 0)
            {
                passError.Error = Resources.GetString(Resource.String.password_error);
                isPasswordValid = false;
            }
            else if (password.Text.ToString().Length < 6)
            {
                passError.Error = Resources.GetString(Resource.String.error_invalid_password);
                isPasswordValid = false;
            }
            else
            {
                isPasswordValid = true;
                passError.ErrorEnabled = false;
            }
            if (isUserValid && isPasswordValid)
            {
                User user = new User();
                user.UserName = etuser.Text;
                user.Password = password.Text;
                if (handler.CheckUser(user.UserName, user.Password))
                {
                    Toast.MakeText(this, "Welcome To Blood Donor Manager", ToastLength.Long).Show();
                    Intent intent = new Intent(this, typeof(DonorActivity));
                    intent.PutExtra("username", user.UserName);
                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Invalid User Name and Password", ToastLength.Long).Show();
                }
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}