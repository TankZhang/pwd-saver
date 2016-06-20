﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;


namespace AndroidTest
{
    [Activity(Label = "AndroidTest", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += Click;
        }

        private void Click(object sender, EventArgs e)
        {
            EditText etResult = FindViewById<EditText>(Resource.Id.etResult);
            etResult.Text += "ha";
        }
    }
}

