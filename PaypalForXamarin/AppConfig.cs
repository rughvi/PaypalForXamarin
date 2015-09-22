using System;

namespace PaypalForXamarin
{
	public class AppConfig
	{
		private static AppConfig instance;
		public static AppConfig Instance
		{
			get
			{
				if (instance == null)
					instance = new AppConfig ();
				return instance;
			}
		}
		private AppConfig ()
		{
		}

		public Environment Env {
			get;
			set;
		}

		public const string kPayPalClientId = @"YOUR_PAYPAL_CLIENT_ID";
		public const string kPayPalReceiverEmail = @"YOUR_RECEIVER_EMAIL_ID";
		public const int REQUEST_CODE_PAYPAL_PAYMENT = 1;
	}

	public enum Environment
	{
		Mock,
		Sandbox,
		Production
	}

}

