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

namespace ColdShineSoft.CustomFileCopier.Controls
{
	/// <summary>
	/// DirectorySelector.xaml 的交互逻辑
	/// </summary>
	public partial class DirectorySelector : Control
	{
		private static Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog _FolderDialog;
		protected Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog FolderDialog
		{
			get
			{
				if(_FolderDialog==null)
				{
					_FolderDialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
					_FolderDialog.IsFolderPicker = true;
				}
				return _FolderDialog;
			}
		}

		[Bindables.DependencyProperty]
		public string OpenButtonText { get; set; } = "选择...";

		[Bindables.DependencyProperty(Options =FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)]
		public string Path { get; set; }

		public DirectorySelector()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (this.FolderDialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
				this.Path = this.FolderDialog.FileName;
		}
	}
}
