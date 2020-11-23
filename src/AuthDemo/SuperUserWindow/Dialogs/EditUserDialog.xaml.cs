using System;
using System.Windows;

using AuthDemo.Data;
using AuthDemo.Exceptions;

namespace AuthDemo
{
	public partial class EditUserDialog : ChangeUserDialog
	{
		SuperUserWindow parentWindow;

		public EditUserDialog(SuperUserWindow parentWindow)
		{
			this.parentWindow = parentWindow;

			applyButton.Content = "Применить";

			FillInputFieldsWithUserData();
		}

		protected override void OnApplyButton(object sender, RoutedEventArgs rea)
		{
			try
			{
				RestoreFieldsBordersColor(inputsLayout);
				HidePopupMessage(mainLayout);

				if (IsThereEmptyFields(inputsLayout))
				{
					MakeEmptyFieldsRed(inputsLayout);
					ShowPopupMessage("Не все поля заполнены", mainLayout);
					return;
				}

				CheckFullName();

				string[] fullName = SplitFullName();
				const int lastNameIndex = 0;
				const int firstNameIndex = 1;
				const int middleNameIndex = 2;

				var newUser = new User()
				{
					ID = Group.GetGroupByID(groupBox.SelectedIndex + 1).ID,
					FirstName = fullName[firstNameIndex],
					LastName = fullName[lastNameIndex],
					MiddleName = fullName[middleNameIndex],
					Login = loginBox.Text,
					Password = passwordBox.Text,
					Group = Group.GetGroupByID(groupBox.SelectedIndex + 1)
				};

				User.Edit(parentWindow.usersGrid.SelectedItem as User, newUser);
				parentWindow.UpdateUsersGrid();

				CloseThisWindow();
			}
			catch (DataAlreadyExistException e)
			{
				MakeRedBorders(loginBox);
				ShowPopupMessage(e.Message, mainLayout);
			}
			catch (ConnectionLostException e) { ShowPopupMessage(e.Message, mainLayout); }
			catch (WrongFullNameException e)
			{
				MakeRedBorders(fullNameBox);
				ShowPopupMessage(e.Message, mainLayout);
			}
		}

		void FillInputFieldsWithUserData()
		{
			try
			{
				User currentUser = parentWindow.usersGrid.SelectedItem as User;

				fullNameBox.Text = currentUser.FullName;
				loginBox.Text = currentUser.Login;
				passwordBox.Text = currentUser.Password;
				// Indexing in groupBox starts with zero while autoincrement in sqlite is starts from 1
				groupBox.SelectedIndex = (int)currentUser.Group.ID - 1;
			}
			catch (NullReferenceException) { }
		}

		protected string SelectedUserLogin()
		{
			return (parentWindow.
				usersGrid.SelectedItem as User).
				Login;
		}
	}
}
