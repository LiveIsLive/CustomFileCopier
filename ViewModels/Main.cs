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

		public void AddJob()
		{
			Models.Job job = new Models.Job();
			job.Name = this.Localization.NewJob + (this.Task.Jobs.Count + 1);
			this.Task.Jobs.Add(job);
		}

		public void Save()
		{

		}

		public void Open()
		{

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
			System.Threading.Tasks.Task.Run(() => this.Setting.Save());
		}

		public void SelectProperty(Models.Condition condition)
		{

		}

		public void CollapsedExpander(double expanderHeight,double fileListHeight)
		{
			this.FileListPreviousHeight = fileListHeight;
			this.FileListHeight = new System.Windows.GridLength(expanderHeight);
		}

		public void ExpandExpander()
		{
			this.FileListHeight = new System.Windows.GridLength(this.FileListPreviousHeight);
		}
	}
}