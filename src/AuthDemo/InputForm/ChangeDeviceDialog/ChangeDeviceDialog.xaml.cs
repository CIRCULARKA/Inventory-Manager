using System.Windows;

using AuthDemo.Data;
using AuthDemo.Exceptions;

namespace AuthDemo
{
	public abstract partial class ChangeDeviceDialog : InputForm
	{
		protected SuperUserWindow parentWindow;

		public ChangeDeviceDialog(SuperUserWindow parent)
		{
			parentWindow = parent;

			InitializeComponent();
			FillComboBoxesWithItems();
		}

		/// <summary>
		///	This method won't fill cabinetBox with items - it must be done separately
		/// in dialog constructor
		/// </summary>
		void FillComboBoxesWithItems()
		{
			try
			{
				foreach (DeviceType type in DeviceType.GetAll())
					deviceTypeBox.Items.Add(type);
				deviceTypeBox.SelectedIndex = 0;
			}
			catch (NoSuchDataException) { }

			try
			{
				foreach (var ip in IPAddress.GetAll())
					ipAddressBox.Items.Add(ip);
				ipAddressBox.SelectedIndex = 0;
			}
			catch (NoSuchDataException) { }

			try
			{
				foreach (Corps Corps in Corps.GetAll())
					corpsBox.Items.Add(Corps);
			}
			catch (NoSuchDataException) { }
		}

		protected void DisplayDeviceCongifuration()
		{
			DeviceConfiguration currentDeviceConfiguration;
			History currentHistoryNote;

			// Initializing selected in SuperUserWindow list Device state
			try
			{
				currentDeviceConfiguration =
					DeviceConfiguration.
						GetDeviceConfiguration(
							SelectedDevice
						);

				// Defining which IP do device have
				for (int i = 0; i < ipAddressBox.Items.Count; i++)
					if ((ipAddressBox.Items[i] as IPAddress).ID ==
						currentDeviceConfiguration.IP.ID)
						ipAddressBox.SelectedIndex = i;

				// Displaying device password in passwordTextBox
				passwordBox.Text =
					currentDeviceConfiguration.
						AccountPassword;
				// Same for account name
				accoutNameBox.Text = currentDeviceConfiguration.AccountName;
			}
			catch (NoSuchDataException) { }

			try
			{
				currentHistoryNote = History.GetDeviceLastHistoryNote(SelectedDevice);

				// Setting device's Corps
				for (int i = 0; i < corpsBox.Items.Count; i++)
				{
					if ((corpsBox.Items[i] as Corps).ID == currentHistoryNote.CorpsID)
						corpsBox.SelectedIndex = i;
				}
			}
			catch (NoSuchDataException) { }
		}

		protected Device SelectedDevice =>
			parentWindow.devicesGrid.SelectedItem as Device;

		protected History SelectedDevicelastHistoryNote =>
			History.GetDeviceLastHistoryNote(SelectedDevice);

		protected DeviceConfiguration SelectedDeviceCongifuration =>
			DeviceConfiguration.GetDeviceConfiguration(SelectedDevice);

		/// <summary>
		///	Refreshes items in cabinetBox according to chosen Corps
		/// </summary>
		void OnCorpsChanged(object sender, RoutedEventArgs rea)
		{
			cabinetBox.Items.Clear();
			try
			{
				foreach (Cabinet cabinet in Cabinet.GetAll(corpsBox.SelectedItem as Corps))
					cabinetBox.Items.Add(cabinet);
				cabinetBox.SelectedIndex = 0;
			}
			catch (NoSuchDataException) { }
		}

		/// <summary>
		/// Called when status check box turned on or off
		/// Status changes means either move Device to storage or just
		/// move it to another cabinet
		/// </summary>
		void OnStatusSwitched(object sender, RoutedEventArgs info)
		{
			if ((bool)statusBox.IsChecked)
				DisableAllControls(contentLayout);
			else
				EnableAllControls(contentLayout);
		}
	}

}
