using System.Windows;

namespace AuthDemo
{
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs sea)
		{
			base.OnStartup(sea);

			var mainWindow = new AuthWindow();
			mainWindow.Show();
		}
	}
}
