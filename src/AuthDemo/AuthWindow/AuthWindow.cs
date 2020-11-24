using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

using AuthDemo.Data;
using AuthDemo.Exceptions;

namespace AuthDemo
{
	public partial class AuthWindow : MonoWindowBehavior
	{
		public AuthWindow()
		{
			InitializeComponent();

			userLogin.Focus();
		}

		private void OnKeyPressed(object s, KeyEventArgs eventArgs)
		{
			if (eventArgs.Key == Key.Enter)
				FocusOnControl(authButton);
		}

		private void OnAuthButton(object s, RoutedEventArgs rea)
		{
			try
			{
				var currentUser = User.GetUserByLogin(userLogin.Text);

				if (userPassword.Password == currentUser.Password)
				{
					OpenWindowForUserGroup(currentUser);
					Close();
				}
				else
				{
					throw new WrongPasswordException();
				}
			}
			catch (NoSuchDataException e)
			{
				MakeRedBorders(userLogin);
				ShowPopupMessage(e.Message, authLayout);
			}
			catch (WrongPasswordException e)
			{
				MakeRedBorders(userPassword);
				ShowPopupMessage(e.Message, authLayout);
				FocusOnControl(userPassword);
			}
			catch (ConnectionLostException e)
			{
				ShowPopupMessage(e.Message, authLayout);
			}
		}

		private void OnLoginChanged(object s, RoutedEventArgs rea)
		{
			TextBox sender = s as TextBox;

			try
			{
				User.GetUserByLogin(sender.Text);
				RestoreBorderBrush(sender);
				EnableControl(authButton);
				HidePopupMessage(authLayout);
				FocusOnControl(userPassword);
			}
			catch (NoSuchDataException e)
			{
				DisableControl(authButton);
				MakeRedBorders(sender);
				ShowPopupMessage(e.Message, authLayout);
			}
		}

		private void OpenWindowForUserGroup(User user)
		{
			// Temporary solution
			if (user.Group.ID == Group.GetGroupByName("Техник").ID)
				ShowWindow(new SuperUserWindow());
			else if (user.Group.ID == Group.GetGroupByName("Администратор").ID)
				ShowWindow(new SuperUserWindow());
			else if (user.Group.ID == Group.GetGroupByName("Суперпользователь").ID)
				ShowWindow(new SuperUserWindow());
		}

		private void ShowWindow(Window window) { window.Show(); }
	}
}
