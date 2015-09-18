using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Diagnostics;

namespace PaypalForXamarin
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent ();
			if (AppConfig.Instance.Env == Environment.Mock) {
				swtMock.IsToggled = true;
			} else if (AppConfig.Instance.Env == Environment.Sandbox) {
				swtSandbox.IsToggled = true;
			} else {
				swtProduction.IsToggled = true;
			}

			this.ToolbarItems.Add (new ToolbarItem ("Done", null, () => {
				if(swtProduction.IsToggled)
				{
					AppConfig.Instance.Env = Environment.Production;
				}
				else if(swtSandbox.IsToggled)
				{
					AppConfig.Instance.Env = Environment.Sandbox;
				}
				else
				{
					AppConfig.Instance.Env = Environment.Mock;
				}
				Debug.WriteLine("Environment selected is " + AppConfig.Instance.Env.ToString());
				this.Navigation.PopAsync(false);
			}));
		}


		private void OnProductionSwitchToggled(object sender, EventArgs e)
		{
			Switch s = sender as Switch;
			if (s.IsToggled) {
				swtMock.IsToggled = false;
				swtSandbox.IsToggled = false;
			}
		}

		private void OnSandboxSwitchToggled(object sender, EventArgs e)
		{
			Switch s = sender as Switch;
			if (s.IsToggled) {
				swtProduction.IsToggled = false;
				swtMock.IsToggled = false;
			}
		}

		private void OnMockSwitchToggled(object sender, EventArgs e)
		{
			Switch s = sender as Switch;
			if (s.IsToggled) {
				swtProduction.IsToggled = false;
				swtSandbox.IsToggled = false;
			}
		}
	}
}

