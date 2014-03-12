using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Hello_World
{
	[Activity (Label = "Hello_World", MainLauncher = true)]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			var aButton = FindViewById<Button>(Resource.Id.aButton);
			var aLabel = FindViewById<TextView>(Resource.Id.helloLabel);

			aButton.Click += (sender, e) => {
				aLabel.Text = "Hello from the button!!!!!!";
			};

		}
	}
}


