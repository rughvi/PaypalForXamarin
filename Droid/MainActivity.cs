using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace PaypalForXamarin.Droid
{
	[Activity (Label = "PaypalForXamarin.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		Action<int, Result, Intent> paypalActivityResult;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			LoadApplication (new App ());
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{			
			base.OnActivityResult (requestCode, resultCode, data);
			if (requestCode == AppConfig.REQUEST_CODE_PAYPAL_PAYMENT) {
				if (paypalActivityResult != null) {
					paypalActivityResult (requestCode, resultCode, data);
				}
			}
		}

		public void StartActivity(Intent intent, int requestCode, Action<int, Result, Intent> OnActivityResult)
		{
			if (requestCode == AppConfig.REQUEST_CODE_PAYPAL_PAYMENT) {
				paypalActivityResult = OnActivityResult;
			}
			this.StartActivityForResult (intent, requestCode);
		}
	}
}

