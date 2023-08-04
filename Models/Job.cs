using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public class Job : Caliburn.Micro.PropertyChangedBase
	{
		private Models.Task _Task;
		public Models.Task Task
		{
			get
			{
				return this._Task;
			}
			set
			{
				if (this._Task == null)
					this._Task = value;
				else throw new System.Security.VerificationException("不允许更改Task的值");
			}
		}

		private string _Name;
		[Newtonsoft.Json.JsonProperty]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
				this.NotifyOfPropertyChange(() => this.Name);
			}
		}

		private string _SourceDirectoryPath;
		[Newtonsoft.Json.JsonProperty]
		public string SourceDirectoryPath
		{
			get
			{
				return this._SourceDirectoryPath;
			}
			set
			{
				this._SourceDirectoryPath = value;
				this._SourceDirectoryLength = null;
				//this._SourceFiles = null;

				this.NotifyOfPropertyChange(() => this.SourceDirectoryPath);
			}
		}

		private int? _SourceDirectoryLength;
		public int SourceDirectoryLength
		{
			get
			{
				if (this._SourceDirectoryLength == null)
					if (this.SourceDirectoryPath == null)
						this._SourceDirectoryLength = 0;
					else
					{
						this._SourceDirectoryLength = this.SourceDirectoryPath.Length;
						if (!this.SourceDirectoryPath.EndsWith("\\") && !this.SourceDirectoryPath.EndsWith("/"))
							this._SourceDirectoryLength++;
					}
				return this._SourceDirectoryLength.Value;
			}
		}

		private string _ResultHandlerTypeName;
		[Newtonsoft.Json.JsonProperty]
		public string ResultHandlerTypeName
		{
			get
			{
				if (this._ResultHandlerTypeName == null)
				{
					if (this._ResultHandler == null)
						this._ResultHandler = ResultHandler.All.Values.First();
					this._ResultHandlerTypeName = ResultHandler.All.First(h => h.Value == this._ResultHandler).Key;
				}
				return this._ResultHandlerTypeName;
			}
			set
			{
				this._ResultHandlerTypeName = value;
				this._ResultHandler = ResultHandler.All[value];
			}
		}

		private ResultHandler _ResultHandler;
		public ResultHandler ResultHandler
		{
			get
			{
				if(this._ResultHandler==null)
				{
					if (this._ResultHandlerTypeName == null)
						this._ResultHandlerTypeName = ResultHandler.All.Keys.First();
					this._ResultHandler = ResultHandler.All[this._ResultHandlerTypeName];
				}
				return this._ResultHandler;
			}
			set
			{
				this._ResultHandler = value;
				this._ResultHandlerTypeName = ResultHandler.All.First(h => h.Value == value).Key;
				this.NotifyOfPropertyChange(() => this.ResultHandler);
			}
		}

		private string _TargetDirectoryPath;
		[Newtonsoft.Json.JsonProperty]
		public string TargetDirectoryPath
		{
			get
			{
				return this._TargetDirectoryPath;
			}
			set
			{
				this._TargetDirectoryPath = value;

				this.NotifyOfPropertyChange(() => this.TargetDirectoryPath);
			}
		}

		[Newtonsoft.Json.JsonProperty]
		public string TargetServer { get; set; }

		[Newtonsoft.Json.JsonProperty]
		public int TargetPort { get; set; }

		[Newtonsoft.Json.JsonProperty]
		public string TargetUserName { get; set; }

		[Newtonsoft.Json.JsonProperty]
		public string TargetPassword { get; set; }

		private ConditionMode _ConditionMode;
		[Newtonsoft.Json.JsonProperty]
		public ConditionMode ConditionMode
		{
			get
			{
				return this._ConditionMode;
			}
			set
			{
				this._ConditionMode = value;
				//this._SourceFiles = null;
				this._FileFilter = null;
				this.NotifyOfPropertyChange(() => this.ConditionMode);
			}
		}

		public string ConditionVariableName { get; } = "fileInfo";

		private string _CustomExpression;
		[Newtonsoft.Json.JsonProperty]
		public virtual string CustomExpression
		{
			get
			{
				if (this._CustomExpression == null)
					this._CustomExpression = @"using System.Linq;
using System.IO;
public class CustomFileFilter:ColdShineSoft.CustomFileCopier.Models.FileFilter
{
	public override System.Collections.Generic.IEnumerable<System.IO.FileInfo> GetFiles(string sourceDirectoryPath)
	{
		return Directory.EnumerateFiles(sourceDirectoryPath, ""*"", SearchOption.AllDirectories).Select(f => new System.IO.FileInfo(f)).Where(f=>LastWriteTime>=System.DateTime.Today);
	}
}";
				return this._CustomExpression;
			}
			set
			{
				this._CustomExpression = value;
				this._FileFilter = null;
				//this._SourceFiles = null;

				this.NotifyOfPropertyChange(() => this.CustomExpression);
			}
		}

		private FileFilter _FileFilter;
		public FileFilter FileFilter
		{
			get
			{
				if(this._FileFilter==null)
					switch(this.ConditionMode)
					{
						case ConditionMode.Designer:
							this._FileFilter = (FileFilter)CSScriptLibrary.CSScript.LoadCode(new FileFilterTemplate { Job = this }.TransformText()).CreateObject("*", this.Conditions);
							break;
						case ConditionMode.SpecifyPaths:
							this._FileFilter = new FilesAndDirectoriesFileFilter(this.FilePaths, this.DirectoryPaths); ;
							break;
						case ConditionMode.Expression:
							this._FileFilter= (FileFilter)CSScriptLibrary.CSScript.LoadCode(this.CustomExpression).CreateObject("*");
							break;
					}
				return this._FileFilter;
			}
		}

		private System.Collections.ObjectModel.ObservableCollection<Condition> _Conditions;
		[Newtonsoft.Json.JsonProperty]
		public System.Collections.ObjectModel.ObservableCollection<Condition> Conditions
		{
			get
			{
				if(this._Conditions==null)
				{
					this._Conditions = new System.Collections.ObjectModel.ObservableCollection<Condition>();
					this._Conditions.CollectionChanged += this.Conditions_CollectionChanged;
				}
				return this._Conditions;
			}
			set
			{
				this._Conditions = value;
				value.CollectionChanged += Conditions_CollectionChanged;
			}
		}

		public Condition FirstCondition => this.Conditions.FirstOrDefault();

		public Condition LastCondition => this.Conditions.LastOrDefault();

		private void Conditions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
				foreach (Condition condition in e.NewItems)
					condition.PropertyChanged += Condition_PropertyChanged;
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move|| e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
				this.NotifyConditionChange(e.OldStartingIndex, e.NewStartingIndex);
		}

		private void Condition_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(this.DataErrorInfo))
				this.OnConditionsChanged();
		}

		protected virtual void OnConditionsChanged()
		{
			//this._SourceFiles = null;
			this._FileFilter = null;
		}

		protected void NotifyConditionChange(params int[] indexes)
		{
			foreach (int index in indexes)
			{
				if (index <= 0)
				{
					this.NotifyOfPropertyChange(() => this.FirstCondition);

				}
				if (index >= this.Conditions.Count - 1)
					this.NotifyOfPropertyChange(() => this.LastCondition);
			}
		}

		public string CombinedExpression
		{
			get
			{
				return string.Join("", this.Conditions.Select(c => c.Expression)).TrimStart(Condition.Connectives.Values.Select(c => c[0]).Concat(" ").ToArray());
			}
		}

		public IProperty[] Properties
		{
			get
			{
				return this.Conditions.Select(c => c.Property).Distinct().ToArray();
			}
		}

		private System.Collections.ObjectModel.ObservableCollection<string> _FilePaths;
		[Newtonsoft.Json.JsonProperty]
		public System.Collections.ObjectModel.ObservableCollection<string> FilePaths
		{
			get
			{
				if (this._FilePaths == null)
					this._FilePaths = new System.Collections.ObjectModel.ObservableCollection<string>();
				return this._FilePaths;
			}
			set
			{
				this._FilePaths = value;
			}
		}

		private System.Collections.ObjectModel.ObservableCollection<string> _DirectoryPaths;
		[Newtonsoft.Json.JsonProperty]
		public System.Collections.ObjectModel.ObservableCollection<string> DirectoryPaths
		{
			get
			{
				if (this._DirectoryPaths == null)
					this._DirectoryPaths = new System.Collections.ObjectModel.ObservableCollection<string>();
				return this._DirectoryPaths;
			}
			set
			{
				this._DirectoryPaths = value;
			}
		}

		//public DynamicExpresso.Parameter[] Parameters
		//{
		//	get
		//	{
		//		System.Collections.Generic.List<DynamicExpresso.Parameter> propertyParameters = new List<DynamicExpresso.Parameter>();
		//		System.Collections.Generic.List<DynamicExpresso.Parameter> operatorParameters = new List<DynamicExpresso.Parameter>();
		//		System.Collections.Generic.List<DynamicExpresso.Parameter> conditionParameters = new List<DynamicExpresso.Parameter>();
		//		foreach(Condition condition in this.Conditions)
		//		{
		//			if (!propertyParameters.Contains(condition.Property.Parameter))
		//				propertyParameters.Add(condition.Property.Parameter);
		//			if (!operatorParameters.Contains(condition.Operator.Parameter))
		//				operatorParameters.Add(condition.Operator.Parameter);
		//			conditionParameters.Add(condition.Parameter);
		//		}

		//		return propertyParameters.Concat(operatorParameters).Concat(conditionParameters).ToArray();
		//	}
		//}

		//public System.Collections.Generic.Dictionary<string, DynamicExpresso.Parameter> Parameters
		//{
		//	get
		//	{
		//		System.Collections.Generic.Dictionary<string, DynamicExpresso.Parameter> parameters = new Dictionary<string, DynamicExpresso.Parameter>();
		//		//foreach()
		//		return parameters;
		//	}
		//}

		private File[] _SourceFiles;
		public File[] SourceFiles
		{
			get
			{
				lock(System.DBNull.Value)
				if(this._SourceFiles==null)
					this._SourceFiles = this.FileFilter.GetFiles(this.SourceDirectoryPath).Select(f => new File(f, this.SourceDirectoryLength)).ToArray();
				//this._SourceFiles = new CustomFileFilter(this.Conditions).GetFiles(System.IO.Directory.GetFiles(this.SourceDirectoryPath, "*", System.IO.SearchOption.AllDirectories).Select(f => new System.IO.FileInfo(f))).Select(f => new File(f, this.SourceDirectoryLength)).ToArray();
				return this._SourceFiles;
			}
		}

		private string _JobNameToDirectoryName;
		public string JobNameToDirectoryName
		{
			get
			{
				if (this._JobNameToDirectoryName == null)
				{
					this._JobNameToDirectoryName = this.Name;
					foreach (char c in File.InvalidFileNameCharacters)
						this._JobNameToDirectoryName = this._JobNameToDirectoryName.Replace(c, (char)(c + 65248));
				}
				return this._JobNameToDirectoryName;
			}
		}

		public event System.Action<(int CopiedCount, long CopiedSize)> FileCopied;
		protected void OnFileCopied(int copiedCount,long copiedSize)
		{
			if (this.FileCopied != null)
				this.FileCopied((copiedCount, copiedSize));
		}

		//public event System.Action Done;
		//protected void OnDone()
		//{
		//	if (this.Done != null)
		//		this.Done();
		//	//this._SourceFiles = null;
		//}

		public void ClearCache()
		{
			this._SourceFiles = null;
		}

		private Models.DataErrorInfos.Job _DataErrorInfo;
		public Models.DataErrorInfos.Job DataErrorInfo
		{
			get
			{
				return this._DataErrorInfo;
			}
			protected set
			{
				this._DataErrorInfo = value;
				this.NotifyOfPropertyChange(() => this.DataErrorInfo);
			}
		}

		public string GetTargetAbsoluteFilePath(string sourceFilePath)
		{
			string path = System.IO.Path.Combine(this.TargetDirectoryPath, sourceFilePath.Substring(this.SourceDirectoryLength));
			if (ResultHandler.Remote)
				return path.Replace('\\', '/');
			return path;
		}

		public string GetTargetRelativeFilePath(string sourceFilePath)
		{
			return sourceFilePath.Substring(this.SourceDirectoryLength);
		}

		//public int CopiedFileCount;   
		//public long CopiedFileSize;
		public void Execute()
		{
			this.ResultHandler.Execute(this);
		}

		//private static System.IO.FileInfo[] _TestFileInfos;
		//public System.IO.FileInfo[] TestFileInfos
		//{
		//	get
		//	{
		//		if (_TestFileInfos == null)
		//			_TestFileInfos = new System.IO.FileInfo[] { new System.IO.FileInfo(File.ExecutingAssembly.Location) };
		//		return _TestFileInfos;
		//	}
		//}

		//private static System.IO.FileInfo[] _TestFileInfos;
		//public System.IO.FileInfo[] TestFileInfos
		//{
		//	get
		//	{
		//		if (_TestFileInfos == null)
		//			_TestFileInfos = new System.IO.FileInfo[] { new System.IO.FileInfo(File.ExecutingAssembly.Location) };
		//		return _TestFileInfos;
		//	}
		//}

		private static string _TestDirectoryPath;
		public string TestDirectoryPath
		{
			get
			{
				if (_TestDirectoryPath == null)
					_TestDirectoryPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "Localization");
				return _TestDirectoryPath;
			}
		}

		public bool InSourceDirectory(string path)
		{
			string souceDirectory = this.SourceDirectoryPath.TrimEnd('\\', '/');
			if (!path.StartsWith(souceDirectory, System.StringComparison.OrdinalIgnoreCase))
				return false;
			if (souceDirectory.Length == path.Length)
				return true;
			char separator = path.Substring(souceDirectory.Length, 1)[0];
			if (separator != '\\' && separator != '/')
				return false;
			return true;
		}

		public bool ValidateData(Localization localization)
		{
			this.DataErrorInfo = new DataErrorInfos.Job();
			bool result = true;
			if(string.IsNullOrWhiteSpace(this.Name))
			{
				result = false;
				this.DataErrorInfo.Name = string.Format(localization.ValidationError[ValidationError.Required], localization.Name);
			}

			if(string.IsNullOrWhiteSpace(this.SourceDirectoryPath))
			{
				result = false;
				this.DataErrorInfo.SourceDirectoryPath = string.Format(localization.ValidationError[ValidationError.Required], localization.SourceDirectory);
			}
			else if(!System.IO.Directory.Exists(this.SourceDirectoryPath))
			{
				result = false;
				this.DataErrorInfo.SourceDirectoryPath = string.Format(localization.ValidationError[ValidationError.InvalidDirectoryPath], localization.SourceDirectory);
			}

			if(!this.Task.CompressToZipFile)
			{
				if(string.IsNullOrWhiteSpace(this.TargetDirectoryPath))
				{
					result = false;
					this.DataErrorInfo.TargetDirectoryPath = string.Format(localization.ValidationError[ValidationError.Required], localization.TargetDirectory);
				}
				else
				{
					this.DataErrorInfo.TargetDirectoryPath = this.ResultHandler.CheckTargetDirectoryValid(this);
					if(this.DataErrorInfo.TargetDirectoryPath!=null)
					{
						result = false;
						this.DataErrorInfo.TargetDirectoryPath = string.Format(localization.ValidationError[ValidationError.InvalidDirectoryPath], localization.TargetDirectory) + this.DataErrorInfo.TargetDirectoryPath;
					}
				}
				if (this.ResultHandler.Remote)
					if (string.IsNullOrWhiteSpace(this.TargetServer))
						this.DataErrorInfo.TargetServer = string.Format(localization.ValidationError[ValidationError.Required], localization.Server);
			}

			switch(this.ConditionMode)
			{
				case ConditionMode.Designer:
					if (this.Conditions.Count == 0)
					{
						result = false;
						this.DataErrorInfo.Condition = string.Format(localization.ValidationError[ValidationError.Required], localization.Condition);
					}
					else
					{
						if(this.Conditions.Count(c=>c.LeftBracket==true)!= this.Conditions.Count(c => c.RightBracket == true))
						{
							result = false;
							this.DataErrorInfo.Condition = localization.ValidationError[ValidationError.BracketMissing];
						}

						foreach (Condition condition in this.Conditions)
							if (!condition.ValidateData(localization))
								result = false;
					}
					break;
				case ConditionMode.SpecifyPaths:
					foreach(string path in this.FilePaths)
					{
						if(!System.IO.File.Exists(path))
						{
							this.DataErrorInfo.FilePaths.Add(string.Format(localization.ValidationError[ValidationError.InvalidFilePath], path));
							result = false;
							continue;
						}
						if (!this.InSourceDirectory(path))
						{
							this.DataErrorInfo.FilePaths.Add(string.Format(localization.ValidationError[ValidationError.FileNotInSourceDirectoy], path));
							result = false;
						}
					}
					foreach(string path in this.DirectoryPaths)
					{
						if (!System.IO.Directory.Exists(path))
						{
							this.DataErrorInfo.DirectoryPaths.Add(string.Format(localization.ValidationError[ValidationError.InvalidDirectoryPath], path));
							result = false;
							continue;
						}
						if (!this.InSourceDirectory(path))
						{
							this.DataErrorInfo.DirectoryPaths.Add(string.Format(localization.ValidationError[ValidationError.DirectoryNotInSourceDirectoy],path));
							result = false;
						}
					}
					break;
				case ConditionMode.Expression:
					if(string.IsNullOrWhiteSpace(this.CustomExpression?.Trim()))
					{
						result = false;
						this.DataErrorInfo.CustomExpression = string.Format(localization.ValidationError[ValidationError.Required], localization.Expression);
					}
					else try
					{
						this.FileFilter.GetFiles(this.TestDirectoryPath);
					}
					catch(Exception exception)
					{
						result = false;
						this.DataErrorInfo.CustomExpression = localization.ValidationError[ValidationError.InvalidCsScript] + exception.Message;
					}
					break;
			}
			return result;
		}
	}
}