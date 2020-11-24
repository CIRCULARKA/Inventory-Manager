using System.Windows.Controls;
using System.Windows;

namespace AuthDemo
{
	public abstract class InputForm : MonoWindowBehavior
	{
		protected readonly string[] dateFormats;

		protected InputForm()
		{
			dateFormats = new string[]
			{
				"dd/MM/yyyy",
				"dd.MM.yyyy",
				"dd\\MM\\yyyy",
				"dd-MM-yyyy"
			};
		}

		protected abstract void OnApplyButton(object sender, RoutedEventArgs rea);

		protected void RestoreFieldsBordersColor(Panel layout)
		{
			foreach (UIElement item in layout.Children)
			{
				if (item is TextBox)
					RestoreBorderBrush(item as TextBox);
			}
		}

		/// <summary>
		/// Checks if there empty TextBoxes in layout
		/// </summary>
		protected bool IsThereEmptyFields(Panel layout)
		{
			foreach (UIElement item in layout.Children)
			{
				if (item is TextBox)
				{
					if (string.IsNullOrWhiteSpace((item as TextBox).Text))
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Checks is there a specified number of first TextBoxes in the layout
		/// </summary>
		protected bool IsThereEmptyFields(Panel layout, int fieldsToCheck)
		{
			int counter = 0;

			foreach (UIElement item in layout.Children)
			{
				if (item is TextBox)
				{
					if (string.IsNullOrWhiteSpace((item as TextBox).Text))
						return true;
					else counter++;
					if (counter == fieldsToCheck) return false;
				}
			}

			return false;
		}

		protected void ClearInputFields(Panel layout)
		{
			foreach (UIElement input in layout.Children)
			{
				if (input is TextBox)
					(input as TextBox).Text = "";
			}
		}

		protected void RestoreComboBoxState(ComboBox item)
		{
			item.SelectedIndex = 0;
		}

		protected void MakeEmptyFieldsRed(Panel layout)
		{
			foreach (UIElement item in layout.Children)
			{
				if (item is TextBox)
				{
					int currLenght = (item as TextBox).Text.Length;
					if (currLenght == 0)
						MakeRedBorders(item as TextBox);
				}
			}
		}

		protected void MakeEmptyFieldsRed(Panel layout, int fieldsToBrush)
		{
			int counter = 0;

			foreach (UIElement item in layout.Children)
			{
				if (item is TextBox)
				{
					counter++;
					int currLenght = (item as TextBox).Text.Length;
					if (currLenght == 0)
						MakeRedBorders(item as TextBox);
					if (counter == fieldsToBrush) return;
				}
			}
		}
	}
}
