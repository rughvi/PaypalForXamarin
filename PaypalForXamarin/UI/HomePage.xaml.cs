using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PaypalForXamarin
{
	public partial class HomePage : ContentPage
	{
		

		public string Environment { get; set; }

		public bool AcceptCreditCards { get; set; }

		public Button BuyAnItem
		{
			get
			{ 
				return btnBuyAnItem;
			}
		}
		public HomePage ()
		{
			InitializeComponent ();
		}

		private void OnSettingsClicked(object sender, EventArgs e)
		{
			this.Navigation.PushAsync (new SettingsPage ());
		}
	}
}

