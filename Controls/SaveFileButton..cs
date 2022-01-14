using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ColdShineSoft.SmartFileCopier.Controls
{
	/// <summary>
	/// SaveFileButton.xaml 的交互逻辑
	/// </summary>
	public partial class SaveFileButton : Button
	{
		private static Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog _SaveFileDialog;
		protected Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog SaveFileDialog
		{
			get
			{
				if (_SaveFileDialog == null)
				{
					_SaveFileDialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonSaveFileDialog();
					_SaveFileDialog.Filters.Add(new Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogFilter("*.json", "*.json"));
				}
				return _SaveFileDialog;
			}
		}

		[Bindables.DependencyProperty]
		public string SelectedFilePath { get; set; }

		public static readonly RoutedEvent SaveFileEvent = EventManager.RegisterRoutedEvent("SaveFile", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<string>), typeof(SaveFileButton));

		public event RoutedPropertyChangedEventHandler<string> SaveFile
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

		public SaveFileButton()
		{
			this.Style = new Style(typeof(SaveFileButton), (System.Windows.Style)Application.Current.TryFindResource("ButtonDefault"));
		}
		protected virtual void OnSaveFile(string path)
		{
			RoutedPropertyChangedEventArgs<string> args = new RoutedPropertyChangedEventArgs<string>(this.SelectedFilePath, path);
			this.SelectedFilePath = path;
			args.RoutedEvent = SaveFileEvent;
			RaiseEvent(args);
		}

		//public SaveFileButton()
		//{
		//	InitializeComponent();
		//}

		protected override void OnClick()
		{
			if (this.SaveFileDialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
				this.OnSaveFile(this.SaveFileDialog.FileName);             

			base.OnClick();
		}
	}
}
