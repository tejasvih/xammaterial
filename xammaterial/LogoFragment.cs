using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace xammaterial
{
    public class LogoFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.logo_fragment, container, false);

            var buttonChangeContent = view.FindViewById<Button>(Resource.Id.button1);
            buttonChangeContent.Click += delegate
            {
                //Intent intent = new Intent(this.Activity, typeof(MainActivity));
                //StartActivity(intent);
                Snackbar.Make(view, "Clicked", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            };

            return view;
        }
    }
}