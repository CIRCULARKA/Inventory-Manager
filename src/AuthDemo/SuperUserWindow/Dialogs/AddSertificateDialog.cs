using System;
using System.Globalization;
using System.Windows;

using AuthDemo.Data;
using AuthDemo.Exceptions;

namespace AuthDemo
{
	class AddSertificateDialog : ChangeSertificateDialog
	{

		public AddSertificateDialog(SuperUserWindow parent) : base(parent)
		{
			applyButton.Content = "Добавить";
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

			try
			{
				var newSertificate = new Sertificate()
				{
					SubjectName = subjectNameBox.Text,
					SerialNumber = int.Parse(serialNumberBox.Text),
					ValidFromDateTime = DateTime.ParseExact(
						validFromBox.Text,
					 	dateFormats,
						CultureInfo.InvariantCulture
					),
					ValidUntilDateTime = DateTime.ParseExact(validUntilBox.Text,
					 	dateFormats,
						CultureInfo.InvariantCulture
					),
				};

				Sertificate.Write(newSertificate);
				HidePopupMessage(mainLayout);
				ShowPopupMessage("Сертификат добавлен", mainLayout);
				RestoreFieldsBordersColor(inputsLayout);
				ClearInputFields(inputsLayout);

				parentWindow.UpdateSertificatesGrid();
				parentWindow.Show();
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
			catch (OverflowException)
			{
				MakeRedBorders(serialNumberBox);
				ShowPopupMessage("Серийный номер слишком больой", mainLayout);
			}
		}
	}
}
