using System.Windows.Input;

namespace AuthDemo
{
	public partial class DeviceHistoryWindow : MonoWindowBehavior
	{
		public DeviceHistoryWindow(SuperUserWindow parentWindow)
		{
			ParentWindow = parentWindow;

			InitializeComponent();
		}

		private SuperUserWindow ParentWindow { get; set; }

		private void OnHistoryGridScroll(object sender, MouseWheelEventArgs info) { }
	}
}
