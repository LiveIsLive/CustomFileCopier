﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.SmartFileCopier.Models
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
			}
		}

		private FileFilter _FileFilter;
		public FileFilter FileFilter
		{
			get
			{
				if(this._FileFilter==null)
					this._FileFilter= (FileFilter)CSScriptLibrary.CSScript.LoadCode($@"
using System.Linq;
public class CustomFileFilter:ColdShineSoft.SmartFileCopier.Models.FileFilter
{{
	public override System.Collections.Generic.IEnumerable<System.IO.FileInfo> GetFiles(System.Collections.Generic.IEnumerable<System.IO.FileInfo> fileInfos)
	{{
		return fileInfos.Where(fileInfo=>{this.CustomExpression});
	}}
}}").CreateObject("*");
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
					//this._Conditions.CollectionChanged += this.Conditions_CollectionChanged;
				}
				return this._Conditions;
			}
			set
			{
				this._Conditions = value;
				//value.CollectionChanged += Conditions_CollectionChanged;
			}
		}

		//private void Conditions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		//{
		//	if(e.NewItems!=null&& e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
		//		foreach(Condition condition in e.NewItems)
		//			condition.PropertyChanged += Condition_PropertyChanged;
		//}

		//private void Condition_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		//{
		//	this._SourceFiles = null;
		//}

		public string CombinedExpression
		{
			get
			{
				return string.Join("", this.Conditions.Select(c => c.Expression));
			}
		}

		public DynamicExpresso.Parameter[] Parameters
		{
			get
			{
				return this.Conditions.SelectMany(c => c.Parameters).ToArray();
			}
		}

		private File[] _SourceFiles;
		public File[] SourceFiles
		{
			get
			{
				if(this._SourceFiles==null)
				{
					switch(this.ConditionMode)
					{
						case ConditionMode.Designer:
							System.Collections.Generic.List<File> files = new List<File>();
							DynamicExpresso.Lambda lambda = new DynamicExpresso.Interpreter().Parse(this.CombinedExpression, typeof(bool), this.Parameters);
							foreach (string filePath in System.IO.Directory.GetFiles(this.SourceDirectoryPath, "*", System.IO.SearchOption.AllDirectories))
							{
								System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
								if ((bool)lambda.Invoke(this.Conditions.SelectMany(c => c.GetValues(fileInfo)).ToArray()))
									files.Add(new File(fileInfo,this.SourceDirectoryLength));
							}
							this._SourceFiles = files.ToArray();
							break;
						case ConditionMode.Expression:
							//string[] allFiles = System.IO.Directory.GetFiles(this.SourceDirectory, "*", System.IO.SearchOption.AllDirectories);
							//Func<System.IO.FileInfo, bool> func = new DynamicExpresso.Interpreter().ParseAsDelegate<Func<System.IO.FileInfo, bool>>(this.CustomExpression, this.ConditionVariableName);
							//this._SourceFiles = allFiles.Select(f => new System.IO.FileInfo(f)).Where(func).Select(f => new File(f, this.SourceDirectoryLength)).ToArray();
							this._SourceFiles = this.FileFilter.GetFiles(System.IO.Directory.GetFiles(this.SourceDirectoryPath, "*", System.IO.SearchOption.AllDirectories).Select(f => new System.IO.FileInfo(f))).Select(f=>new File(f,this.SourceDirectoryLength)).ToArray();
							break;
					}
				}
				return this._SourceFiles;
			}
		}

		private static readonly char[] SpecialCharacters = new char[] { '\\', '/', ':', '*', '?', '"', '>', '<', '|' };

		private string _JobNameToDirectoryName;
		public string JobNameToDirectoryName
		{
			get
			{
				if (this._JobNameToDirectoryName == null)
				{
					this._JobNameToDirectoryName = this.Name;
					foreach (char c in SpecialCharacters)
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
			this._SourceFiles = null;
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
	}
}