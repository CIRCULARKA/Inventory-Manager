using System;
using System.Windows;

using AuthDemo.Data;
using AuthDemo.Exceptions;

namespace AuthDemo
{
	partial class AddUserDialog : ChangeUserDialog
	{
		SuperUserWindow parentWindow;

		public AddUserDialog(Window parent)
		{
			parentWindow = parent as SuperUserWindow;
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
					FirstName = fullName[firstNameIndex],
					LastName = fullName[lastNameIndex],
					MiddleName = fullName[middleNameIndex],
					Login = loginBox.Text,
					Password = passwordBox.Text,
					Group = Group.GetGroupByName((string)groupBox.SelectedItem)
				};

				User.Write(newUser);
				HidePopupMessage(mainLayout);
				ShowPopupMessage("Пользователь добавлен", mainLayout);
				RestoreFieldsBordersColor(inputsLayout);
				ClearInputFields(inputsLayout);
				RestoreComboBoxState(groupBox);

				parentWindow.UpdateUsersGrid();
				parentWindow.Show();
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
	}
}
