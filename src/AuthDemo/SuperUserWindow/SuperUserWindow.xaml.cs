using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

using AuthDemo.Exceptions;
using AuthDemo.Data;

namespace AuthDemo
{
	public partial class SuperUserWindow : MonoWindowBehavior
	{
		private readonly List<User> _users;
		private readonly List<Sertificate> _sertificates;
		private readonly List<Device> _devices;

		public SuperUserWindow()
		{
			_users = new List<User>();
			_sertificates = new List<Sertificate>();
			_devices = new List<Device>();

			FillGridsWithDataAndInitXaml();
		}

		public Device SelectedDevice =>
			devicesGrid.SelectedItem as Device;

		private void FillGridsWithDataAndInitXaml()
		{
			GetUsersFromDB();
			GetSertificatesFromDB();
			GetDevicesFromDB();
			InitializeComponent();

			usersGrid.ItemsSource = _users;
			sertificatesGrid.ItemsSource = _sertificates;
			devicesGrid.ItemsSource = _devices;
		}

		private void GetUsersFromDB()
		{
			try { _users.AddRange(User.GetAllEntities()); }
			catch (NoSuchDataException) { }
		}

		private void GetSertificatesFromDB()
		{
			try
			{
				_sertificates.
					AddRange(
						Sertificate.
							GetAllEntities()
					);
			}
			catch (NoSuchDataException) { }
		}

		private void GetDevicesFromDB()
		{
			try { _devices.AddRange(Device.GetAll()); }
			catch (NoSuchDataException) { }
		}

		public void UpdateUsersGrid()
		{
			_users.RemoveRange(0, _users.Count);
			_users.AddRange(User.GetAllEntities());
			usersGrid.ItemsSource = null;
			usersGrid.ItemsSource = _users;
		}

		public void UpdateSertificatesGrid()
		{
			_sertificates.RemoveRange(0, _sertificates.Count);
			_sertificates.
				AddRange(Sertificate.GetAllEntities());
			sertificatesGrid.ItemsSource = null;
			sertificatesGrid.ItemsSource = _sertificates;
		}

		public void UpdateDevicesGrid()
		{
			_devices.RemoveRange(0, _devices.Count);
			try
			{
				_devices.AddRange(Device.GetAll());
				devicesGrid.ItemsSource = null;
				devicesGrid.ItemsSource = _devices;
			}
			catch (NoSuchDataException)
			{
				devicesGrid.ItemsSource = null;
			}
		}

		private void OnOnlyDisplayDeviceWithIP(object sender, RoutedEventArgs info)
		{
			devicesGrid.ItemsSource = null;

			try
			{
				devicesGrid.ItemsSource = Device.GetAllWithoutIP();
			}
			catch (NoSuchDataException) { }
		}

		private void OnDisplayDeviceAnyIP(object sender, RoutedEventArgs info)
		{
			UpdateDevicesGrid();
		}

		private void ShowChildWindowAndFocus(Window window) =>
			window.ShowDialog();

		private void OnAddUser(object sender, RoutedEventArgs rea) =>
			ShowChildWindowAndFocus(new AddUserDialog(this));

		private void OnEditUser(object sender, RoutedEventArgs rea)
		{
			HidePopupMessage(mainLayout);

			if (usersGrid.SelectedIndex == -1)
				ShowPopupMessage("Выберите пользователя из списка", mainLayout);
			else
				ShowChildWindowAndFocus(new EditUserDialog(this));
		}

		private void OnDeleteUser(object sender, RoutedEventArgs rea)
		{
			try
			{
				HidePopupMessage(mainLayout);
				if (usersGrid.SelectedItem == null)
					throw new NullReferenceException("Выберите пользователя из списка");
				User.Delete(usersGrid.SelectedItem as User);
				UpdateUsersGrid();
			}
			catch (NullReferenceException e)
			{
				ShowPopupMessage(e.Message, mainLayout);
			}
		}

		private void OnAddSertificate(object sender, RoutedEventArgs info)
		{
			ShowChildWindowAndFocus(new AddSertificateDialog(this));
		}

		private void OnEditSertificate(object sender, RoutedEventArgs rea)
		{
			HidePopupMessage(mainLayout);

			if (sertificatesGrid.SelectedIndex == -1)
				ShowPopupMessage("Выберите сертификат из списка", mainLayout);
			else
				ShowChildWindowAndFocus(new EditSertificateDialog(this));
		}

		private void OnDeleteSertificate(object sender, RoutedEventArgs rea)
		{
			try
			{
				HidePopupMessage(mainLayout);
				if (sertificatesGrid.SelectedItem == null)
					throw new NullReferenceException("Выберите сертификат из списка");
				Sertificate.Delete(
					sertificatesGrid.SelectedItem as Sertificate
				);
				UpdateSertificatesGrid();
			}
			catch (NullReferenceException e)
			{
				ShowPopupMessage(e.Message, mainLayout);
			}
		}

		private void OnAddDevice(object s, RoutedEventArgs rea)
		{
			ShowChildWindowAndFocus(new AddDeviceDialog(this));
		}

		private void OnEditDevice(object s, RoutedEventArgs rea)
		{
			HidePopupMessage(mainLayout);

			if (devicesGrid.SelectedIndex == -1)
				ShowPopupMessage("Выберите устройство из списка", mainLayout);
			else
				ShowChildWindowAndFocus(new EditDeviceDialog(this));
		}

		private void OnDeviceHistory(object sender, RoutedEventArgs info)
		{
			HidePopupMessage(mainLayout);

			if (devicesGrid.SelectedIndex == -1)
				ShowPopupMessage("Выберите устройство из списка", mainLayout);
			else
				ShowChildWindowAndFocus(new DeviceHistoryWindow(this));
		}

		private void OnDeleteDevice(object s, RoutedEventArgs rea)
		{
			try
			{
				HidePopupMessage(mainLayout);

				if (devicesGrid.SelectedItem == null)
					throw new NullReferenceException("Выберите устройство из списка");
				History.DeleteAllDeviceHistory(
					devicesGrid.SelectedItem as Device
				);

				DeviceConfiguration.Delete(
					DeviceConfiguration.GetDeviceConfiguration(
						devicesGrid.SelectedItem as Device
					)
				);

				Device.Delete(
					devicesGrid.SelectedItem as Device
				);

				UpdateDevicesGrid();
			}
			catch (NullReferenceException e)
			{
				ShowPopupMessage(e.Message, mainLayout);
			}
		}

		private void OnTabSwitched(object sender, SelectionChangedEventArgs args)
		{
			HidePopupMessage(mainLayout);
		}

		private void OnUsersGridScroll(object sender, MouseWheelEventArgs info)
		{
			if (info.Delta > 0)
				usersGridScroll.LineUp();
			else usersGridScroll.LineDown();
		}

		private void OnSertificatesGridScroll(object sender, MouseWheelEventArgs info)
		{
			if (info.Delta > 0)
				sertificatesGridScroll.LineUp();
			else sertificatesGridScroll.LineDown();
		}

		private void OnDevicesGridScroll(object sender, MouseWheelEventArgs info)
		{
			if (info.Delta > 0)
				devicesGridScroll.LineUp();
			else devicesGridScroll.LineDown();
		}
	}
}
