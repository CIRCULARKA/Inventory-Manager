namespace AuthDemo
{
	internal class DeviceHistoryWindow : MonoWindowBehavior
	{
		public DeviceHistoryWindow(SuperUserWindow parentWindow)
		{
			ParentWindow = parentWindow;
		}

		private SuperUserWindow ParentWindow { get; set; }
	}
}
