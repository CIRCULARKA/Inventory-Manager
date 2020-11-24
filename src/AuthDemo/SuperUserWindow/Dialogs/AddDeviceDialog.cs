using System;
using System.Windows;

using AuthDemo.Data;
using AuthDemo.Exceptions;

namespace AuthDemo
{
	internal class AddDeviceDialog : ChangeDeviceDialog
	{
		public AddDeviceDialog(SuperUserWindow parent) : base(parent)
		{
			// Picking N/A Corps as a default value in ComboBox
			for (int i = 0; i < corpsBox.Items.Count; i++)
			{
				var currentCorps = corpsBox.Items[i] as Corps;
				if (currentCorps.Name == "N/A")
					corpsBox.SelectedIndex = i;
			}

			// Picking cabinet according to Corps
			foreach (Cabinet cabinet in Cabinet.GetAll(corpsBox.SelectedItem as Corps))
				cabinetBox.Items.Add(cabinet);
			cabinetBox.SelectedIndex = 0;

			// Hiding statusBox - any device addition means putting it into storage
			contentLayout.Children.Remove(checkBoxLayout);
		}

		protected override void OnApplyButton(object sender, RoutedEventArgs rea)
		{
			try
			{
				RestoreFieldsBordersColor(contentLayout);
				HidePopupMessage(mainLayout);

				if (IsThereEmptyFields(contentLayout, 3))
				{
					MakeEmptyFieldsRed(contentLayout, 3);
					ShowPopupMessage("Не все поля заполнены", mainLayout);
					return;
				}

				var newDevice = new Device()
				{
					SerialNumber = long.Parse(serialNumberBox.Text),
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
					Status = Status.GetStatusByID(1),
					ChangeTimeDateTime = DateTime.Now
				};

				Device.Write(newDevice);
				DeviceConfiguration.Write(newConfiguration);
				History.Write(newHistoryNote);
				HidePopupMessage(mainLayout);
				ShowPopupMessage("Устройство добавлено", mainLayout);
				RestoreFieldsBordersColor(contentLayout);
				ClearInputFields(contentLayout);
				RestoreComboBoxState(deviceTypeBox);

				parentWindow.UpdateDevicesGrid();
				parentWindow.Show();
			}
			catch (FormatException)
			{
				MakeRedBorders(serialNumberBox);
				ShowPopupMessage("Серийный номер должен быть числом", mainLayout);
			}
			catch (DataAlreadyExistException e)
			{
				MakeRedBorders(serialNumberBox);
				MakeRedBorders(inventoryNumberBox);
				ShowPopupMessage(e.Message, mainLayout);
			}
			catch (ConnectionLostException e) { ShowPopupMessage(e.Message, mainLayout); }
		}
	}
}
