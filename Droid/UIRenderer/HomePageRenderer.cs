using System;
using Xamarin.Forms;
using PaypalForXamarin;
using PaypalForXamarin.Droid;
using Xamarin.Forms.Platform.Android;
using Com.Paypal.Android.Sdk.Payments;
using Java.Math;
using Android.Content;
using Android.App;

[assembly:ExportRenderer(typeof(HomePage), typeof(HomePageRenderer))]
namespace PaypalForXamarin.Droid
{
	public class HomePageRenderer : PageRenderer
	{
		HomePage homePage;
		PayPalConfiguration config;
		protected override void OnElementChanged (ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
				homePage = e.NewElement as HomePage;
				homePage.BuyAnItem.Clicked -= OnBuyAnItem;
			}
			if (e.NewElement is HomePage) {
				homePage = e.NewElement as HomePage;
				homePage.BuyAnItem.Clicked += OnBuyAnItem;
				config = new PayPalConfiguration ();
				config.ClientId (AppConfig.kPayPalClientId);
			}
		}

		private void OnBuyAnItem(object sender, EventArgs e)
		{
			Console.WriteLine ("");


			if (AppConfig.Instance.Env == Environment.Mock) {
				config.Environment (PayPalConfiguration.EnvironmentNoNetwork);
			}
			else if (AppConfig.Instance.Env == PaypalForXamarin.Environment.Production)
			{
				config.Environment (PayPalConfiguration.EnvironmentProduction);
			}else if(AppConfig.Instance.Env == PaypalForXamarin.Environment.Sandbox)
			{
				config.Environment (PayPalConfiguration.EnvironmentSandbox);
			}

			PayPalPayment thingToBuy = getThingToBuy (PayPalPayment.PaymentIntentSale);
			MainActivity activity = this.Context as MainActivity;
			Intent intent = new Intent (activity, typeof(PaymentActivity));
			intent.PutExtra (PayPalService.ExtraPaypalConfiguration, config);
			intent.PutExtra (PaymentActivity.ExtraPayment, thingToBuy);

			activity.StartActivity (intent, AppConfig.REQUEST_CODE_PAYPAL_PAYMENT, OnActivityResult);
		}

		public void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if (requestCode == AppConfig.REQUEST_CODE_PAYPAL_PAYMENT && data != null) {
				if (resultCode == Result.Ok) {
					var confirmObj = data.GetParcelableExtra (PaymentActivity.ExtraResultConfirmation);
					PaymentConfirmation confirm =Android.Runtime.Extensions.JavaCast<PaymentConfirmation> (confirmObj);
					Console.WriteLine (confirm.ToString ());
					if (confirm != null) {
						try
						{
							
						}
						catch(Exception ex) {
							Console.WriteLine (confirm.ToString ());
						}
					}

				} else if (resultCode == Result.Canceled) {
					
				} else if ((int)resultCode == PaymentActivity.ResultExtrasInvalid) {
				
				}
			}
		}

		private PayPalPayment getThingToBuy(string paymentIntent)
		{
			return new PayPalPayment (new BigDecimal ("1.75"), "GBP", "sample item", paymentIntent);
		}
	}
}

