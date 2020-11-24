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
		private readonly List<User> users;
		private readonly List<Sertificate> sertificates;
		private readonly List<Device> devices;

		public SuperUserWindow(Window parentWindow)
		{
			users = new List<User>();
			sertificates = new List<Sertificate>();
			devices = new List<Device>();

			FillGridsWithDataAndInitXaml();
		}

		void FillGridsWithDataAndInitXaml()
		{
			GetUsersFromDB();
			GetSertificatesFromDB();
			GetDevicesFromDB();
			InitializeComponent();

			usersGrid.ItemsSource = users;
			sertificatesGrid.ItemsSource = sertificates;
			devicesGrid.ItemsSource = devices;
		}

		void GetUsersFromDB()
		{
			try { users.AddRange(User.GetAllEntities()); }
			catch (NoSuchDataException) { }
		}

		void GetSertificatesFromDB()
		{
			try
			{
				sertificates.
					AddRange(
						Sertificate.
							GetAllEntities()
					);
			}
			catch (NoSuchDataException) { }
		}

		void GetDevicesFromDB()
		{
			try { devices.AddRange(Device.GetAll()); }
			catch (NoSuchDataException) { }
		}

		public void UpdateUsersGrid()
		{
			users.RemoveRange(0, users.Count);
			users.AddRange(User.GetAllEntities());
			usersGrid.ItemsSource = null;
			usersGrid.ItemsSource = users;
		}

		public void UpdateSertificatesGrid()
		{
			sertificates.RemoveRange(0, sertificates.Count);
			sertificates.
				AddRange(Sertificate.GetAllEntities());
			sertificatesGrid.ItemsSource = null;
			sertificatesGrid.ItemsSource = sertificates;
		}

		public void UpdateDevicesGrid()
		{
			devices.RemoveRange(0, devices.Count);
			try
			{
				devices.AddRange(Device.GetAll());
				devicesGrid.ItemsSource = null;
				devicesGrid.ItemsSource = devices;
			}
			catch (NoSuchDataException)
			{
				devicesGrid.ItemsSource = null;
			}
		}

		void ShowChildWindowAndFocus(Window window) =>
			window.ShowDialog();

		void OnAddUser(object sender, RoutedEventArgs rea) =>
			ShowChildWindowAndFocus(new AddUserDialog(this));

		void OnEditUser(object sender, RoutedEventArgs rea)
		{
			HidePopupMessage(mainLayout);

			if (usersGrid.SelectedIndex == -1)
				ShowPopupMessage("Выберите пользователя из списка", mainLayout);
			else
				ShowChildWindowAndFocus(new EditUserDialog(this));
		}

		void OnDeleteUser(object sender, RoutedEventArgs rea)
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

		void OnAddSertificate(object sender, RoutedEventArgs info)
		{
			ShowChildWindowAndFocus(new AddSertificateDialog(this));
		}

		void OnEditSertificate(object sender, RoutedEventArgs rea)
		{
			HidePopupMessage(mainLayout);

			if (sertificatesGrid.SelectedIndex == -1)
				ShowPopupMessage("Выберите сертификат из списка", mainLayout);
			else
				ShowChildWindowAndFocus(new EditSertificateDialog(this));
		}

		void OnDeleteSertificate(object sender, RoutedEventArgs rea)
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

		void OnTabSwitched(object sender, SelectionChangedEventArgs args)
		{
			HidePopupMessage(mainLayout);
		}

		void OnUsersGridScroll(object sender, MouseWheelEventArgs info)
		{
			if (info.Delta > 0)
				usersGridScroll.LineUp();
			else usersGridScroll.LineDown();
		}

		void OnSertificatesGridScroll(object sender, MouseWheelEventArgs info)
		{
			if (info.Delta > 0)
				sertificatesGridScroll.LineUp();
			else sertificatesGridScroll.LineDown();
		}

		void OnDevicesGridScroll(object sender, MouseWheelEventArgs info)
		{
			if (info.Delta > 0)
				devicesGridScroll.LineUp();
			else devicesGridScroll.LineDown();
		}
	}
}
