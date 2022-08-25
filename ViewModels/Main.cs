using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.ViewModels
{
	public class Main : Screen, GongSolutions.Wpf.DragDrop.IDropTarget
	{
		private Models.Task _Task = new Models.Task();
		public Models.Task Task
		{
			get
			{
				return this._Task;
			}
			set
			{
				this._Task = value;
				this.NotifyOfPropertyChange(() => this.Task);
			}
		}

		public Models.Theme[] Themes { get; } = Models.Theme.All;

		private System.Globalization.CultureInfo[] _InstalledCultures;
		public System.Globalization.CultureInfo[] InstalledCultures
		{
			get
			{
				if (this._InstalledCultures == null)
				{
					System.Collections.Generic.List<System.Globalization.CultureInfo> cultures = new List<System.Globalization.CultureInfo>();
					foreach (string path in System.IO.Directory.GetFiles(LocalizationDirectory))
					{
						string name = System.IO.Path.GetFileNameWithoutExtension(path);
						try
						{
							cultures.Add(System.Globalization.CultureInfo.GetCultureInfo(name));
						}
						catch
						{
						}
					}
					this._InstalledCultures = cultures.ToArray();

				}
				return this._InstalledCultures;
			}
		}

		public Models.ResultHandler[] ResultHandlers { get; } = Models.ResultHandler.All.Values.ToArray();

		//public string Colon { get; } = "：";

		public Models.IProperty[] Properties { get; }= Models.Properties.FileProperties;

		public Models.LogicalConnective[] Connectives { get; } = System.Enum.GetValues(typeof(Models.LogicalConnective)).Cast<Models.LogicalConnective>().ToArray();

		public System.Collections.Generic.Dictionary<System.Type, Models.IOperator[]> Operators { get; } = Models.Operators.TypedOperators;

		public System.Array ConditionModes { get; } = System.Enum.GetValues(typeof(Models.ConditionMode));

		public void SetConditionMode(Models.ConditionMode mode)
		{
			this.SelectedJob.ConditionMode = mode;
		}

		public static string _OpeningFilePath;
		public string OpeningFilePath
		{
			get
			{
				return _OpeningFilePath;
			}
			set
			{
				_OpeningFilePath = value;
				this.NotifyOfPropertyChange(() => this.OpeningFilePath);
			}
		}

		public static void SetOpeningFilePath(string path)
		{
			_OpeningFilePath = path;
		}

		private System.Collections.ObjectModel.ObservableCollection<string> _RecentFiles;
		public System.Collections.ObjectModel.ObservableCollection<string> RecentFiles
		{
			get
			{
				if (this._RecentFiles == null)
					this._RecentFiles = new System.Collections.ObjectModel.ObservableCollection<string>(this.Setting.RecentFiles.Where(f => f != OpeningFilePath));
				return this._RecentFiles;
			}
		}

		protected const string ProgramName = "Custom File Copier";

		private bool _UpdateJobBinding;
		public bool UpdateJobBinding
		{
			get
			{
				return this._UpdateJobBinding;
			}
			set
			{
				this._UpdateJobBinding = value;
				this.NotifyOfPropertyChange(() => this.UpdateJobBinding);
			}
		}

		private string _Title = ProgramName;
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				this._Title = value;
				this.NotifyOfPropertyChange(() => this.Title);
			}
		}

		private Models.Job _SelectedJob;
		public Models.Job SelectedJob
		{
			get
			{
				return this._SelectedJob;
			}
			set
			{
				this._SelectedJob = value;
				this.NotifyOfPropertyChange(() => this.SelectedJob);
			}
		}

		public Main()
		{
			this.ChangeTheme(this.Setting.Theme);
		}

		public void AddJob()
		{
			Models.Job job = new Models.Job();
			job.Name = this.Localization.NewJob + " " + (this.Task.Jobs.Count + 1);
			this.Task.Jobs.Add(job);
			this.SelectedJob = job;
		}

		public void Save(string path)
		{
			this.UpdateJobBinding = true;
			if (this.OpeningFilePath == null)
				this.OpeningFilePath = path;
			this.Save();
		}

		public void SaveAs(string path)
		{
			this.UpdateJobBinding = true;
			this.OpeningFilePath = path;
			this.Save();
		}

		public void Save()
		{
			//if (!this.ValidateData())
			//	return;

			this.SelectedJob = null;
			this.SelectedJob = this.Task.Jobs[0];

			this.Task.Save(OpeningFilePath);
			this._RecentFiles = null;
			this.NotifyOfPropertyChange(() => this.RecentFiles);
			this.SetTitle();
		}

		public void Open(string path)
		{
			this.Task = Models.Task.Open(path);
			this.OpeningFilePath = path;
			this._RecentFiles = null;
			this.NotifyOfPropertyChange(() => this.RecentFiles);
			this.SetTitle();
			this.SelectedJob = this.Task.Jobs.FirstOrDefault();
		}

		public void RemoveRecentFile(string path)
		{
			this.RecentFiles.Remove(path);
			this.Setting.RecentFiles.Remove(path);
			this.Setting.Save();
		}

		protected void SetTitle()
		{
			this.Title = ProgramName + " - " + System.IO.Path.GetFileNameWithoutExtension(OpeningFilePath);
		}

		public void AddCondition(Models.Job job)
		{
			Models.Condition condition = new Models.Condition();
			condition.Property = this.Properties[0];
			if (job.Conditions.Count > 0)
				condition.Connective = Models.LogicalConnective.And;
			//condition.Operator = condition.Property.AllowOperators[0];
			job.Conditions.Add(condition);
		}

		public void MoveUpCondition(Models.Job job, Models.Condition condition)
		{
			int oldIndex = job.Conditions.IndexOf(condition);
			int newIndex = oldIndex - 1;
			job.Conditions.Move(oldIndex, newIndex);
		}

		public void MoveDownCondition(Models.Job job, Models.Condition condition)
		{
			int oldIndex = job.Conditions.IndexOf(condition);
			int newIndex = oldIndex + 1;
			job.Conditions.Move(oldIndex, newIndex);
		}

		public void RemoveCondition(Models.Job job, Models.Condition condition)
		{
			job.Conditions.Remove(condition);
			//if (job.Conditions.Count > 0)
			//	job.Conditions[0].Connective = null;
		}

		protected Models.Localization GetLocalization(System.Globalization.CultureInfo culture)
		{
			System.IO.StreamReader reader = new System.IO.StreamReader(LocalizationDirectory + culture.Name + ".json");
			try
			{
				return new Newtonsoft.Json.JsonSerializer().Deserialize<Models.Localization>(new Newtonsoft.Json.JsonTextReader(reader));
				//return NetJSON.NetJSON.Deserialize<Models.Localization>(reader);
			}
			finally
			{
				reader.Close();
			}
		}

		public void SelectLanguage(System.Globalization.CultureInfo culture)
		{
			System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
			System.Threading.Thread.CurrentThread.CurrentCulture = culture;

			this.Localization = this.GetLocalization(culture);
			Models.Global.Instance.Localization = this.Localization;
			this.Setting.SelectedCultureName = culture.Name;
			//this.SetUiLang(this.Setting.SelectedCultureName);
			System.Threading.Tasks.Task.Run(() => this.Setting.Save());
		}

		public void Run()
		{
			this.UpdateJobBinding = true;

			if (!this.ValidateData())
				return;

			if (!this.Task.CompressToZipFile)
				foreach (Models.Job job in this.Task.Jobs)
					if (!job.ResultHandler.TargetDirectoryEmpty(job))
						if (this.DialogService.ShowMessageBox(this, String.Format(this.Localization.TargetDirectoryIsNotEmpty, job.TargetDirectoryPath), "", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Warning) != System.Windows.MessageBoxResult.OK)
							return;

			this.WindowManager.ShowDialogAsync(new Runner(this.Task, System.IO.Path.GetFileNameWithoutExtension(this.OpeningFilePath)));
		}

		public void UncheckedCompressToZipFile()
		{
		}

		public bool ValidateData()
		{
			if (this.Task.ValidateData(this.Localization))
				return true;
			if (this.Task.DataErrorInfo.LastInvalidJob != null)
				this.SelectedJob = this.Task.DataErrorInfo.LastInvalidJob;
			return false;
		}

		//public void EnsureJobBinding()
		//{
		//	Models.Job job = this.SelectedJob;
		//	this.SelectedJob = null;
		//	this.SelectedJob = job;
		//}

		public void ShowTutorial()
		{
			System.Diagnostics.Process.Start("https://github.com/LiveIsLive/CustomFileCopier/");
		}

		public void ShowAboutWindow()
		{
			this.WindowManager.ShowDialogAsync(new About());
		}

		void IDropTarget.DragEnter(IDropInfo dropInfo)
		{
		}

		void IDropTarget.DragOver(IDropInfo dropInfo)
		{
			if (dropInfo.Data == null || dropInfo.TargetItem == null)
				return;

			dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
			dropInfo.Effects = System.Windows.DragDropEffects.Move;
		}

		void IDropTarget.DragLeave(IDropInfo dropInfo)
		{
		}

		void IDropTarget.Drop(IDropInfo dropInfo)
		{
			Models.Condition sourceCondition = (Models.Condition)dropInfo.Data;
			Models.Condition targetCondition = (Models.Condition)dropInfo.TargetItem;

			if (sourceCondition == null || targetCondition == null)
				return;
			
			int oldIndex=this.SelectedJob.Conditions.IndexOf(sourceCondition);
			this.SelectedJob.Conditions.Move(oldIndex, dropInfo.InsertIndex);
		}

		public void ChangeTheme(Models.Theme theme)
		{
			if (theme.Parent == null)
				return;
			object themeManager = System.Type.GetType("ControlzEx.Theming.ThemeManager,ControlzEx")?.GetProperty("Current").GetValue(null);
			//themeManager.ChangeTheme(System.Windows.Application.Current, theme.ToString());
			if (themeManager!= null)
				themeManager.GetType().GetMethod("ChangeTheme", new System.Type[] { typeof(System.Windows.Application), typeof(string), typeof(bool) }).Invoke(themeManager, new object[] { System.Windows.Application.Current, theme.ToString(), false });

			this.Setting.ThemeId = theme.ThemeId;
			this.Setting.Save();
		}
	}
}