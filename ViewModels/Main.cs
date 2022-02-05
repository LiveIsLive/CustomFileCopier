using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.SmartFileCopier.ViewModels
{
	public class Main : Screen
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

		private System.Globalization.CultureInfo[] _InstalledCultures;
		public System.Globalization.CultureInfo[] InstalledCultures
		{
			get
			{
				if (this._InstalledCultures == null)
					this._InstalledCultures = System.IO.Directory.GetFiles(LocalizationDirectory).Select(path => System.Globalization.CultureInfo.GetCultureInfo(System.IO.Path.GetFileNameWithoutExtension(path))).ToArray();
				return this._InstalledCultures;
			}
		}

		private System.Windows.GridLength _FileListHeight = new System.Windows.GridLength(3, System.Windows.GridUnitType.Star);
		public System.Windows.GridLength FileListHeight
		{
			get
			{
				return this._FileListHeight;
			}
			set
			{
				this._FileListHeight = value;
				this.NotifyOfPropertyChange(() => this.FileListHeight);

			}
		}

		private bool _FileListExpanded = true;
		public bool FileListExpanded 
		{
			get
			{
				return this._FileListExpanded;
			}
			set
			{
				this._FileListExpanded = value;
				this.NotifyOfPropertyChange(() => this.FileListExpanded);
			}
		}

		public double FileListPreviousHeight;

		public string Colon { get; } = "：";

		public Models.Property[] Properties { get; }= Models.Property.Properties;

		public Models.LogicalConnective[] Connectives { get; } = System.Enum.GetValues(typeof(Models.LogicalConnective)).Cast<Models.LogicalConnective>().ToArray();

		public System.Collections.Generic.Dictionary<System.Type, Models.Operator[]> Operators { get; } = Models.Operator.Operators;

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

		private string[] _RecentFiles;
		public string[] RecentFiles
		{
			get
			{
				if (this._RecentFiles == null)
					this._RecentFiles = this.Setting.RecentFiles.Where(f => f != OpeningFilePath).ToArray();
				return this._RecentFiles;
			}
		}

		protected const string ProgramName = "Smart File Copier";

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

		public void AddJob()
		{
			Models.Job job = new Models.Job();
			job.Name = this.Localization.NewJob + (this.Task.Jobs.Count + 1);
			this.Task.Jobs.Add(job);
			this.SelectedJob = job;
		}

		public void Save(string path)
		{
			if (this.OpeningFilePath == null)
				this.OpeningFilePath = path;
			this.Save();
		}

		public void SaveAs(string path)
		{
			this.OpeningFilePath = path;
			this.Save();
		}

		public void Save()
		{
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

		public void RemoveCondition(Models.Job job, Models.Condition condition)
		{
			job.Conditions.Remove(condition);
			job.Conditions[0].Connective = null;
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
			this.Setting.SelectedCultureName = culture.Name;
			this.SetUiLang(this.Setting.SelectedCultureName);
			System.Threading.Tasks.Task.Run(() => this.Setting.Save());
		}

		//public void SelectProperty(Models.Condition condition)
		//{

		//}

		public void CollapsedExpander(double expanderHeight,double fileListHeight)
		{
			this.FileListPreviousHeight = fileListHeight;
			this.FileListHeight = new System.Windows.GridLength(expanderHeight);
		}

		public void ExpandExpander()
		{
			this.FileListHeight = new System.Windows.GridLength(this.FileListPreviousHeight);
		}

		public void Run()
		{
			this.WindowManager.ShowDialog(new Runner(this.Task, System.IO.Path.GetFileNameWithoutExtension(this.OpeningFilePath)));
		}


	}
}