using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace xammaterial
{
    [Activity(Label = "@string/app_name",  Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Window.DecorView.SystemUiVisibility = StatusBarVisibility.Hidden;
            ActionBar?.Hide();
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            initApp();
            StartActivity(typeof(MainActivity));
        }
        void initApp()
        {
            AppInstance.Init();
    }
        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

       
    }
}