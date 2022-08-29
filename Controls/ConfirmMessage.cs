using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Controls
{
	public class ConfirmMessage:System.Windows.FrameworkElement
	{
		[Bindables.DependencyProperty]
		public string Title { get; set; }

		[Bindables.DependencyProperty]
		public string Message { get; set; }

		[Bindables.DependencyProperty]
		public string OkText { get; set; }

		[Bindables.DependencyProperty]
		public string CancelText { get; set; }

		[Bindables.DependencyProperty(OnPropertyChanged = nameof(OnShowChanged), Options = System.Windows.FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)]
		public bool Show { get; set; }

		[Bindables.DependencyProperty(Options = System.Windows.FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)]
		public bool Result { get; set; }

		protected static void OnShowChanged(System.Windows.DependencyObject dependencyObject, System.Windows.DependencyPropertyChangedEventArgs eventArgs)
		{
			ConfirmMessage confirmMessage = (ConfirmMessage)dependencyObject;
			if (!confirmMessage.Show)
				return;

			//string title = confirmMessage.Title;
			//string message = confirmMessage.Message;
			//string okText = confirmMessage.OkText;
			//string cancelText = confirmMessage.CancelText;
			//MahApps.Metro.Controls.Dialogs.MessageDialogResult result = MahApps.Metro.Controls.Dialogs.MessageDialogResult.Canceled;
			//MahApps.Metro.Controls.MetroWindow window = (MahApps.Metro.Controls.MetroWindow)System.Windows.Window.GetWindow(confirmMessage);

			//System.Threading.Tasks.Task.Run(async () =>
			//{
			//	try
			//	{
			//		System.Windows.Application.Current.Dispatcher.Invoke(async  () =>
			//		{
			//		result = await MahApps.Metro.Controls.Dialogs.DialogManager.ShowMessageAsync(window, title, message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative, new MahApps.Metro.Controls.Dialogs.MetroDialogSettings { AffirmativeButtonText = okText, NegativeButtonText = cancelText });

			//		}).Wait();
			//	}
			//	catch (System.Exception exception)
			//	{

			//	}
			//}).ConfigureAwait(false).GetAwaiter().GetResult();
			confirmMessage.Result = MahApps.Metro.Controls.Dialogs.DialogManager.ShowModalMessageExternal((MahApps.Metro.Controls.MetroWindow)System.Windows.Window.GetWindow(confirmMessage), confirmMessage.Title, confirmMessage.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative, new MahApps.Metro.Controls.Dialogs.MetroDialogSettings { AffirmativeButtonText = confirmMessage.OkText, NegativeButtonText = confirmMessage.CancelText }) == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative;
			//MahApps.Metro.Controls.Dialogs.DialogManager.ShowMessageAsync((MahApps.Metro.Controls.MetroWindow)System.Windows.Window.GetWindow(confirmMessage), confirmMessage.Title, confirmMessage.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative, new MahApps.Metro.Controls.Dialogs.MetroDialogSettings { AffirmativeButtonText = confirmMessage.OkText, NegativeButtonText = confirmMessage.CancelText }).ConfigureAwait(false).GetAwaiter();

			////confirmMessage.Result = MahApps.Metro.Controls.Dialogs.DialogManager.ShowMessageAsync((MahApps.Metro.Controls.MetroWindow)System.Windows.Window.GetWindow(confirmMessage), confirmMessage.Title, confirmMessage.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative, new MahApps.Metro.Controls.Dialogs.MetroDialogSettings { AffirmativeButtonText = confirmMessage.OkText, NegativeButtonText = confirmMessage.CancelText }).Result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative;
			//var task = MahApps.Metro.Controls.Dialogs.DialogManager.ShowMessageAsync((MahApps.Metro.Controls.MetroWindow)System.Windows.Window.GetWindow(confirmMessage), confirmMessage.Title, confirmMessage.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative, new MahApps.Metro.Controls.Dialogs.MetroDialogSettings { AffirmativeButtonText = confirmMessage.OkText, NegativeButtonText = confirmMessage.CancelText }).ConfigureAwait(false).GetAwaiter();
			//task.GetResult();
			//confirmMessage.Result = task.ConfigureAwait(false).GetAwaiter().GetResult() == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative;

			//MahApps.Metro.Controls.Dialogs.MessageDialogResult result =await MahApps.Metro.Controls.Dialogs.DialogManager.ShowMessageAsync((MahApps.Metro.Controls.MetroWindow)System.Windows.Window.GetWindow(confirmMessage), confirmMessage.Title, confirmMessage.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative, new MahApps.Metro.Controls.Dialogs.MetroDialogSettings { AffirmativeButtonText = confirmMessage.OkText, NegativeButtonText = confirmMessage.CancelText });
			confirmMessage.Show = false;
		}
	}
}