using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AuthDemo
{
	public class MonoWindowBehavior : Window
	{
		protected TextBlock _popupMessage;
		protected Brush _defaultBorderBrush;

		public MonoWindowBehavior()
		{
			_popupMessage = new TextBlock();
			_defaultBorderBrush = Brushes.Gray;

			_popupMessage = new TextBlock()
			{
				Height = 15,
				TextWrapping = TextWrapping.Wrap,
				Margin = new Thickness(5, 0, 5, 5),
				TextAlignment = TextAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};
		}

		protected void HidePopupMessage(Panel layout)
		{
			try
			{
				layout.Children.Remove(_popupMessage);
			}
			catch (InvalidOperationException) { }
		}

		protected void ShowPopupMessage(string message, Panel layout)
		{
			try
			{
				_popupMessage.Text = message;
				layout.Children.Add(_popupMessage);
			}
			catch (ArgumentException) { }
			catch (InvalidOperationException) { }
		}

		protected void EnableControl(Control item)
		{
			item.IsEnabled = true;
		}

		protected void DisableControl(Control item)
		{
			item.IsEnabled = false;
		}

		/// <summary>
		/// Disables each Control except Buttons
		/// </summary>
		protected void DisableAllControls(Panel layout)
		{
			foreach (UIElement element in layout.Children)
			{
				if (element is Control & !(element is Button))
				{
					DisableControl(element as Control);
				}
			}
		}

		protected void EnableAllControls(Panel layout)
		{
			foreach (UIElement element in layout.Children)
				if (element is Control)
					EnableControl(element as Control);
		}

		protected void MakeRedBorders(Control obj)
		{
			obj.BorderBrush = Brushes.Red;
		}

		protected void RestoreBorderBrush(Control obj)
		{
			obj.BorderBrush = _defaultBorderBrush;
		}

		protected void FocusOnControl(Control item)
		{
			item.Focus();
		}

		protected void CloseThisWindow()
		{
			this.Close();
		}
	}
}
