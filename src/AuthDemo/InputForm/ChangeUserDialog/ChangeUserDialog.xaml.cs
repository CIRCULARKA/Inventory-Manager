using System;

using AuthDemo.Data;
using AuthDemo.Exceptions;

namespace AuthDemo
{
	public abstract partial class ChangeUserDialog : InputForm
	{
		readonly int maxNameLenght;

		public ChangeUserDialog()
		{
			maxNameLenght = 20;

			InitializeComponent();
			FillComboBoxWithItems();
			FocusOnControl(fullNameBox);
		}

		void FillComboBoxWithItems()
		{
			try
			{
				foreach (Group group in Group.GetAll())
					groupBox.Items.Add(group.Name);
				groupBox.SelectedIndex = 0;
			}
			catch (NoSuchDataException) { }
		}

		protected void CheckFullName()
		{
			string[] fullName = SplitFullName();

			if (fullName.Length != 3)
				throw new WrongFullNameException(
						$"Полное имя должно состоять из трёх частей"
					);

			foreach (string part in fullName)
				if (part.Length > maxNameLenght)
					throw new WrongFullNameException(
						$"Одна из частей ФИО имеет более {maxNameLenght} символов"
					);

			foreach (char ch in fullNameBox.Text)
				if (Char.IsDigit(ch))
					throw new WrongFullNameException(
						$"В полном имени не должно быть цифр"
					);
			foreach (string name in fullName)
				if (char.IsLower(name[0]))
					throw new WrongFullNameException(
						$"Каждая часть ФИО должна быть с большой буквы"
					);
		}

		protected string[] SplitFullName()
		{
			return fullNameBox.Text.Split(' ');
		}
	}
}
