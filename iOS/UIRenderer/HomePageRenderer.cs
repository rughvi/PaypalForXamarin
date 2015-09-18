using System;
using Xamarin.Forms.Platform.iOS;
using PaypalForXamarin;
using PaypalForXamarin.iOS;
using Xamarin.Forms;
using PaypaliOSBinding;
using System.Diagnostics;
using Foundation;

[assembly: ExportRenderer(typeof(HomePage), typeof(HomePageRenderer))]
namespace PaypalForXamarin.iOS
{
	public class SamplePayPalPaymentDelegate : PayPalPaymentDelegate
	{
		HomePageRenderer HostViewController;

		public SamplePayPalPaymentDelegate (HomePageRenderer hostViewController)
		{
			HostViewController = hostViewController;
		}

		public override void PayPalPaymentDidComplete (PayPalPayment completedPayment)
		{
			HostViewController.PayPalPaymentDidComplete (completedPayment);
		}

		public override void PayPalPaymentDidCancel ()
		{
			HostViewController.PayPalPaymentDidCancel ();
		}
	}

	public class HomePageRenderer : PageRenderer
	{
		public string Environment { get; set; }

		public bool AcceptCreditCards { get; set; }

		public PayPalPayment CompletedPayment { get; set; }

		SamplePayPalPaymentDelegate samplePayPalPaymentDelegate;

		HomePage homePage;
		public HomePageRenderer ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			homePage = Element as HomePage;
			this.Environment = PayPalPaymentDelegate.PayPalEnvironmentNoNetwork;
			homePage.BuyAnItem.Clicked += BuyAnItemClicked;
		}

		private void BuyAnItemClicked(object sender, EventArgs e)
		{
			// Any customer identifier that you have will work here. Do NOT use a device- or
			// hardware-based identifier.
			string customerId = "user-11723";

			var payment = new PayPalPayment () {
				Amount = new NSDecimalNumber("9.95"),
				CurrencyCode = "USD",
				ShortDescription = "Hipster t-shirt"
			};

			// Set the environment:
			// - For live charges, use PayPalEnvironmentProduction (default).
			// - To use the PayPal sandbox, use PayPalEnvironmentSandbox.
			// - For testing, use PayPalEnvironmentNoNetwork.
			if (AppConfig.Instance.Env == PaypalForXamarin.Environment.Mock) {
				PayPalPaymentViewController.Environment = PayPalPaymentDelegate.PayPalEnvironmentNoNetwork;	
			} else if (AppConfig.Instance.Env == PaypalForXamarin.Environment.Production) {
				PayPalPaymentViewController.Environment = PayPalPaymentDelegate.PayPalEnvironmentProduction;
			} else if (AppConfig.Instance.Env == PaypalForXamarin.Environment.Sandbox) {
				PayPalPaymentViewController.Environment = PayPalPaymentDelegate.PayPalEnvironmentSandbox;
			}

			samplePayPalPaymentDelegate = new SamplePayPalPaymentDelegate (this);

			var paymentViewController = new PayPalPaymentViewController (AppConfig.kPayPalClientId,
				AppConfig.kPayPalReceiverEmail,
				customerId,
				payment,
				samplePayPalPaymentDelegate);

			if (paymentViewController.Handle == IntPtr.Zero) {
				Debug.WriteLine ("Failed to create PayPalPaymentViewController.");
				return;
			}

			// Setting the languageOrLocale property is optional.
			//
			// If you do not set languageOrLocale, then the PayPalPaymentViewController will present
			// its user interface according to the device's current language setting.
			//
			// Setting languageOrLocale to a particular language (e.g., @"es" for Spanish) or
			// locale (e.g., @"es_MX" for Mexican Spanish) forces the PayPalPaymentViewController
			// to use that language/locale.
			//
			// For full details, including a list of available languages and locales, see PayPalPaymentViewController.h.
			paymentViewController.LanguageOrLocale = "en";

			this.PresentViewController (paymentViewController, true, null);
		}
		public void PayPalPaymentDidComplete (PayPalPayment completedPayment)
		{
			Debug.WriteLine ("PayPal Payment Success!");
			this.CompletedPayment = completedPayment;

			// Payment was processed successfully; send to server for verification and fulfillment
			this.SendCompletedPaymentToServer (completedPayment);
			this.DismissViewController (false, null);
		}
		// TODO: Expose this as a WeakDelegate through PayPalPaymentDelegate
		public void PayPalPaymentDidCancel ()
		{
			Debug.WriteLine ("PayPal Payment Canceled");
			this.CompletedPayment = null;
//			this.successView.Hidden = true;
			this.DismissViewController (true, null);
		}

		void SendCompletedPaymentToServer (PayPalPayment completedPayment)
		{
			// TODO: Send completedPayment.confirmation to server
			Debug.WriteLine ("Here is your proof of payment:\n\n{0}\n\nSend this to your server for confirmation and fulfillment.", completedPayment.Confirmation);
		}
	}
}

