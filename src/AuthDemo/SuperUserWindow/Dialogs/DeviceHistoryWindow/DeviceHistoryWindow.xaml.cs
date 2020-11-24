using System.Windows.Input;
using System.Collections.Generic;

using AuthDemo.Data;

namespace AuthDemo
{
	public partial class DeviceHistoryWindow : MonoWindowBehavior
	{
		private readonly List<History> _historyNotes;

		public DeviceHistoryWindow(SuperUserWindow parentWindow)
		{
			ParentWindow = parentWindow;

			_historyNotes = new List<History>();

			InitializeComponent();
			FillHistoryGridWithItems();
		}

		private SuperUserWindow ParentWindow { get; set; }

		private void FillHistoryGridWithItems()
		{
			_historyNotes.AddRange(
				History.GetDeviceHistory(
					ParentWindow.SelectedDevice
				)
			);
			historyGrid.ItemsSource = _historyNotes;
		}

		private void OnHistoryGridScroll(object sender, MouseWheelEventArgs info) { }
	}
}
