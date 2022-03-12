namespace ColdShineSoft.CustomFileCopier.Controls
{
	public partial class SaveFileSplitButton : HandyControl.Controls.SplitButton
	{
		private static Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog _SaveFileDialog;
		protected Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog SaveFileDialog
		{
			get
			{
				if (_SaveFileDialog == null)
				{
					_SaveFileDialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog();
					_SaveFileDialog.DefaultExtension = "json";
					_SaveFileDialog.AlwaysAppendDefaultExtension = true;
					_SaveFileDialog.Filters.Add(new Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogFilter("*.json", "*.json"));
				}
				return _SaveFileDialog;
			}
		}

		[Bindables.DependencyProperty(Options =System.Windows.FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)]
		public string SelectedFilePath { get; set; }

		public static readonly System.Windows.RoutedEvent SaveFileEvent = System.Windows.EventManager.RegisterRoutedEvent("SaveFile", System.Windows.RoutingStrategy.Bubble, typeof(System.Windows.RoutedPropertyChangedEventHandler<string>), typeof(SaveFileSplitButton));

		public event System.Windows.RoutedPropertyChangedEventHandler<string> SaveFile
		{
			add
			{
				this.AddHandler(SaveFileEvent, value);
			}
			remove
			{
				this.RemoveHandler(SaveFileEvent, value);
			}
		}

		public SaveFileSplitButton()
		{
			this.Style = new System.Windows.Style(typeof(SaveFileSplitButton), (System.Windows.Style)System.Windows.Application.Current.TryFindResource(typeof(HandyControl.Controls.SplitButton)));
		}

		protected virtual void OnSaveFile(string path)
		{
			System.Windows.RoutedPropertyChangedEventArgs<string> args = new System.Windows.RoutedPropertyChangedEventArgs<string>(this.SelectedFilePath, path);
			this.SelectedFilePath = path;
			args.RoutedEvent = SaveFileEvent;
			RaiseEvent(args);
		}

		protected override void OnClick()
		{
			if (this.SelectedFilePath==null)
			{
				if(this.SaveFileDialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
					this.OnSaveFile(this.SaveFileDialog.FileName);             
			}
			else this.OnSaveFile(this.SelectedFilePath);

			base.OnClick();
		}
	}
}