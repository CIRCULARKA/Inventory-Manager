using AuthDemo.Data;

namespace AuthDemo
{
	public abstract partial class ChangeSertificateDialog : InputForm
	{
		protected SuperUserWindow parentWindow;

		public ChangeSertificateDialog(SuperUserWindow parent)
		{
			parentWindow = parent;

			InitializeComponent();
		}
	}
}
