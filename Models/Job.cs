using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public class Job : Caliburn.Micro.PropertyChangedBase
	{
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

		private bool _SpecifyTargetDirectory;
		[Newtonsoft.Json.JsonProperty]
		public bool SpecifyTargetDirectory
		{
			get
			{
				return this._SpecifyTargetDirectory;
			}
			set
			{
				this._SpecifyTargetDirectory = value;
				this.RealTargetDirectoryPath = null;

				this.NotifyOfPropertyChange(() => this.SpecifyTargetDirectory);
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
				this.RealTargetDirectoryPath = null;

				this.NotifyOfPropertyChange(() => this.TargetDirectoryPath);
			}
		}

		private string _RealTargetDirectoryPath;
		public string RealTargetDirectoryPath
		{
			get
			{
				if (this._RealTargetDirectoryPath == null)
					if (this.SpecifyTargetDirectory)
						this._RealTargetDirectoryPath = this.TargetDirectoryPath;
					else this._RealTargetDirectoryPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.Guid.NewGuid().ToString());
				return this._RealTargetDirectoryPath;
			}
			protected set
			{
				this._RealTargetDirectoryPath = value;
				this._RealTargetDirectoryLength = null;
			}
		}

		private int? _RealTargetDirectoryLength;
		public int RealTargetDirectoryLength
		{
			get
			{
				if (this._RealTargetDirectoryLength == null)
					if (this.RealTargetDirectoryPath == null)
						this._RealTargetDirectoryLength = 0;
					else
					{
						this._RealTargetDirectoryLength = this.RealTargetDirectoryPath.Length;
						if (!this.RealTargetDirectoryPath.EndsWith("\\") && !this.RealTargetDirectoryPath.EndsWith("/"))
							this._RealTargetDirectoryLength++;
					}
				return this._RealTargetDirectoryLength.Value;
			}
		}

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
		public string CustomExpression
		{
			get
			{
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
							this._FileFilter = (FileFilter)CSScriptLibrary.CSScript.LoadCode(new FileFilterTemplate { Conditions = this.Conditions }.TransformText()).CreateObject("*", this.Conditions);
							break;
						case ConditionMode.Expression:
							this._FileFilter= (FileFilter)CSScriptLibrary.CSScript.LoadCode($@"
using System.Linq;
public class CustomFileFilter:ColdShineSoft.CustomFileCopier.Models.FileFilter
{{
	public override System.Collections.Generic.IEnumerable<System.IO.FileInfo> GetFiles(System.Collections.Generic.IEnumerable<System.IO.FileInfo> fileInfos)
	{{
		return fileInfos.Where(fileInfo=>{this.CustomExpression});
	}}
}}").CreateObject("*");
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
					this.NotifyOfPropertyChange(() => this.FirstCondition);
				if (index >= this.Conditions.Count - 1)
					this.NotifyOfPropertyChange(() => this.LastCondition);
			}
		}

		public string CombinedExpression
		{
			get
			{
				return string.Join("", this.Conditions.Select(c => c.Expression));
			}
		}

		public IProperty[] Properties
		{
			get
			{
				return this.Conditions.Select(c => c.Property).Distinct().ToArray();
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
					this._SourceFiles = this.FileFilter.GetFiles(System.IO.Directory.GetFiles(this.SourceDirectoryPath, "*", System.IO.SearchOption.AllDirectories).Select(f => new System.IO.FileInfo(f))).Select(f => new File(f, this.SourceDirectoryLength)).ToArray();
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

		public event System.Action Done;
		protected void OnDone()
		{
			if (this.Done != null)
				this.Done();
			//this._SourceFiles = null;
		}

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

		protected string GetTargetFilePath(string sourceFilePath)
		{
			return System.IO.Path.Combine(this.RealTargetDirectoryPath, sourceFilePath.Substring(this.SourceDirectoryLength));
		}

		public int CopiedFileCount;
		public long CopiedFileSize;
		public void CopyFiles()
		{
			this.CopiedFileCount = 0;
			this.CopiedFileSize = 0;

			foreach(File sourceFile in this.SourceFiles)
			{
				if(sourceFile.Result!=CopyResult.Success)
				{
					sourceFile.Result = CopyResult.Copying;
					string targetFilePath = this.GetTargetFilePath(sourceFile.Path);
					string targetDirectory = System.IO.Path.GetDirectoryName(targetFilePath);
					if(!System.IO.Directory.Exists(targetDirectory))
						try
						{
							System.IO.Directory.CreateDirectory(targetDirectory);
						}
						catch(System.Exception exception)
						{
							sourceFile.Result = CopyResult.Failure;
							sourceFile.Error = exception.Message;
							//return exception.Message;
							continue;
						}
					try
					{
						System.IO.File.Copy(sourceFile.Path, targetFilePath, true);
					}
					catch (System.Exception exception)
					{
						sourceFile.Result = CopyResult.Failure;
						sourceFile.Error = exception.Message;
						//return exception.Message;
						continue;
					}
				}
				sourceFile.Result = CopyResult.Success;
				this.CopiedFileCount++;
				this.CopiedFileSize += sourceFile.FileInfo.Length;
				this.OnFileCopied(this.CopiedFileCount, this.CopiedFileSize);
			}
			this.OnDone();
		}

		private static System.IO.FileInfo[] _TestFileInfos;
		public System.IO.FileInfo[] TestFileInfos
		{
			get
			{
				if (_TestFileInfos == null)
					_TestFileInfos = new System.IO.FileInfo[] { new System.IO.FileInfo(File.ExecutingAssembly.Location) };
				return _TestFileInfos;
			}
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

			if(this.SpecifyTargetDirectory)
				if(string.IsNullOrWhiteSpace(this.TargetDirectoryPath))
				{
					result = false;
					this.DataErrorInfo.TargetDirectoryPath = string.Format(localization.ValidationError[ValidationError.Required], localization.TargetDirectory);
				}
				else if(!System.IO.Directory.Exists(this.TargetDirectoryPath))
					try
					{
						System.IO.Directory.CreateDirectory(this.TargetDirectoryPath);
					}
					catch(System.Exception exception)
					{
						result = false;
						this.DataErrorInfo.TargetDirectoryPath = string.Format(localization.ValidationError[ValidationError.InvalidDirectoryPath], localization.TargetDirectory) + exception.Message;
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
				case ConditionMode.Expression:
					if(string.IsNullOrWhiteSpace(this.CustomExpression?.Trim()))
					{
						result = false;
						this.DataErrorInfo.CustomExpression = string.Format(localization.ValidationError[ValidationError.Required], localization.Expression);
					}
					else try
					{
						this.FileFilter.GetFiles(this.TestFileInfos);
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