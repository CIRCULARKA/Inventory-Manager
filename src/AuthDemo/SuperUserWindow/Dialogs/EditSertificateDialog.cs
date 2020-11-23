using System;
using System.Windows;

using AuthDemo.Data;
using AuthDemo.Exceptions;

namespace AuthDemo
{
	class EditSertificateDialog : ChangeSertificateDialog
	{

		public EditSertificateDialog(SuperUserWindow parent) : base(parent)
		{
			applyButton.Content = "Применить";

			FillInputFieldsWithSertificatesData();
		}

		protected override void OnApplyButton(object sender, RoutedEventArgs info)
		{
			RestoreFieldsBordersColor(inputsLayout);
			HidePopupMessage(mainLayout);

			if (IsThereEmptyFields(inputsLayout))
			{
				MakeEmptyFieldsRed(inputsLayout);
				ShowPopupMessage("Не все поля заполнены", mainLayout);
				return;
			}

			var newSertificate = new Sertificate()
			{
				SubjectName = subjectNameBox.Text,
				SerialNumber = int.Parse(serialNumberBox.Text),
				ValidFromDateTime = DateTime.Parse(validFromBox.Text),
				ValidUntilDateTime = DateTime.Parse(validUntilBox.Text)
			};

			try
			{
				Sertificate.Edit(
					SelectedSertificate(),
					newSertificate
				);

				parentWindow.UpdateSertificatesGrid();

				CloseThisWindow();
			}
			catch (DataAlreadyExistException e)
			{
				MakeRedBorders(serialNumberBox);
				ShowPopupMessage(e.Message, mainLayout);
			}
			catch (ConnectionLostException e) { ShowPopupMessage(e.Message, mainLayout); }
			catch (FormatException e)
			{
				if (e.TargetSite.Name == "ThrowOverflowOrFormatException")
				{
					MakeRedBorders(serialNumberBox);
					ShowPopupMessage("Неверное значение серийного номера", mainLayout);
				}
				else
				{
					MakeRedBorders(validFromBox);
					MakeRedBorders(validUntilBox);
					ShowPopupMessage("Некорректная дата", mainLayout);
				}

			}
		}

		void FillInputFieldsWithSertificatesData()
		{
			try
			{
				var currentSertificate =
					parentWindow.sertificatesGrid.
						SelectedItem as Sertificate;

				subjectNameBox.Text =
					currentSertificate.SubjectName;
				serialNumberBox.Text =
					currentSertificate.SerialNumber.ToString();
				validFromBox.Text =
					$"{currentSertificate.ValidFromDateTime.Day}/" +
					$"{currentSertificate.ValidFromDateTime.Month}/" +
					$"{currentSertificate.ValidFromDateTime.Year}";
				validUntilBox.Text =
					$"{currentSertificate.ValidUntilDateTime.Day}/" +
					$"{currentSertificate.ValidUntilDateTime.Month}/" +
					$"{currentSertificate.ValidUntilDateTime.Year}";
			}
			catch (NullReferenceException) { }
		}

		protected Sertificate SelectedSertificate()
		{
			return parentWindow.
				sertificatesGrid.
				SelectedItem as Sertificate;
		}
	}
}
