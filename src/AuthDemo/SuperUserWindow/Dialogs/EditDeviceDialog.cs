using System;
using System.Windows;
using System.Globalization;

using AuthDemo.Data;
using AuthDemo.Exceptions;

namespace AuthDemo
{
	internal class EditDeviceDialog : ChangeDeviceDialog
	{
		public EditDeviceDialog(SuperUserWindow parent) : base(parent)
		{
			DisplayDeviceData();
			DisplayDeviceCongifuration();

			DisableControl(serialNumberBox);

			try
			{
				var lastDeviceNote = History.GetDeviceLastHirstoryNote(SelectedDevice);
				if (lastDeviceNote.Status.Name == "Убрано на склад")
					statusBox.IsChecked = true;
			}
			catch (NoSuchDataException) { }
		}

		private void DisplayDeviceData()
		{
			serialNumberBox.Text = SelectedDevice.SerialNumber.ToString();
			inventoryNumberBox.Text = SelectedDevice.InventoryNumber;
			networkNameBox.Text = SelectedDevice.NetworkName;

			try
			{
				for (int i = 0; i < deviceTypeBox.Items.Count; i++)
				{
					if (SelectedDevice.Type.ID == (deviceTypeBox.Items[i] as DeviceType).ID)
						deviceTypeBox.SelectedIndex = i;
				}
			}
			catch (NoSuchDataException) { }

			try
			{
				for (int i = 0; i < corpsBox.Items.Count; i++)
				{
					var currentCorps = corpsBox.Items[i] as Corps;
					if (SelectedDevicelastHistoryNote.CorpsID == currentCorps.ID)
					{
						corpsBox.SelectedIndex = i;
						break;
					}
				}
			}
			catch (NoSuchDataException)
			{
				for (int i = 0; i < corpsBox.Items.Count; i++)
				{
					if (SelectedDevicelastHistoryNote.Corps.Name == "N/A")
					{
						corpsBox.SelectedIndex = i;
						break;
					}
				}
			}

			try
			{
				foreach (Cabinet cabinet in Cabinet.GetAll(corpsBox.SelectedItem as Corps))
					cabinetBox.Items.Add(cabinet);
			}
			catch (NullReferenceException)
			{
				foreach (Cabinet cabinet in Cabinet.GetAll(corpsBox.SelectedItem as Corps))
					cabinetBox.Items.Add(cabinet);
			}
		}

		protected override void OnApplyButton(object sender, RoutedEventArgs rea)
		{
			try
			{
				RestoreFieldsBordersColor(mainLayout);
				HidePopupMessage(mainLayout);
				if (IsThereEmptyFields(mainLayout, 4))
				{
					MakeEmptyFieldsRed(mainLayout, 4);
					ShowPopupMessage("Не все поля заполнены", mainLayout);
					return;
				}

				var newDevice = new Device()
				{
					SerialNumber = int.Parse(serialNumberBox.Text),
					InventoryNumber = inventoryNumberBox.Text,
					Type = deviceTypeBox.SelectedItem as DeviceType,
					NetworkName = networkNameBox.Text
				};

				var newConfiguration = new DeviceConfiguration()
				{
					DeviceSerialNumber = int.Parse(serialNumberBox.Text),
					IP = ipAddressBox.SelectedItem as IPAddress,
					AccountName = accoutNameBox.Text,
					AccountPassword = passwordBox.Text
				};

				var newHistoryNote = new History()
				{
					DeviceSerialNumber = long.Parse(serialNumberBox.Text),
					Corps = corpsBox.SelectedItem as Corps,
					Cabinet = cabinetBox.SelectedItem as Cabinet,
					Status = (bool)statusBox.IsChecked ?
						Status.GetStatusByName("Убрано на склад") :
						Status.GetStatusByName("Перемещено"),
					ChangeTimeDateTime = DateTime.Now
				};

				Device.Edit(
					SelectedDevice,
					newDevice
				);

				DeviceConfiguration.Edit(
					DeviceConfiguration.GetDeviceConfiguration(SelectedDevice),
					newConfiguration
				);

				History.Write(
					newHistoryNote
				);
			}
			catch (FormatException)
			{
				MakeRedBorders(serialNumberBox);
				ShowPopupMessage("Серийный номер должен быть числом", mainLayout);
			}
			catch (DataAlreadyExistException) { }
			catch (ConnectionLostException e) { ShowPopupMessage(e.Message, mainLayout); }
			catch (NoSuchDataException) { }
			finally
			{
				CloseThisWindow();
				parentWindow.UpdateDevicesGrid();
			}
		}
	}
}
