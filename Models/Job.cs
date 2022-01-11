using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.SmartFileCopier.Models
{
	public class Job : Caliburn.Micro.PropertyChangedBase
	{
		private string _Name;
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

		private string _SourceDirectory;
		public string SourceDirectory
		{
			get
			{
				return this._SourceDirectory;
			}
			set
			{
				this._SourceDirectory = value;
				this._SourceDirectoryLength = null;
			}
		}

		private int? _SourceDirectoryLength;
		[Newtonsoft.Json.JsonIgnore]
		public int SourceDirectoryLength
		{
			get
			{
				if (this._SourceDirectoryLength == null)
					if (this.SourceDirectory == null)
						this._SourceDirectoryLength = 0;
					else
					{
						this._SourceDirectoryLength = this.SourceDirectory.Length;
						if (!this.SourceDirectory.EndsWith("\\") && !this.SourceDirectory.EndsWith("/"))
							this._SourceDirectoryLength++;
					}
				return this._SourceDirectoryLength.Value;
			}
		}

		private string _TargetDirectory;
		public string TargetDirectory
		{
			get
			{
				return this._TargetDirectory;
			}
			set
			{
				this._TargetDirectory = value;
			}
		}

		//private int? _TargetDirectoryLength;
		//[Newtonsoft.Json.JsonIgnore]
		//public int TargetDirectoryLength
		//{
		//	get
		//	{
		//		if (this._TargetDirectoryLength == null)
		//			if (this.TargetDirectory == null)
		//				this._TargetDirectoryLength = 0;
		//			else
		//			{
		//				this._TargetDirectoryLength = this.TargetDirectory.Length;
		//				if (!this.TargetDirectory.EndsWith("\\") && !this.TargetDirectory.EndsWith("/"))
		//					this._TargetDirectoryLength++;
		//			}
		//		return this._TargetDirectoryLength.Value;
		//	}
		//}

		public System.Collections.ObjectModel.ObservableCollection<Condition> Conditions { get; set; } = new System.Collections.ObjectModel.ObservableCollection<Condition>();

		[Newtonsoft.Json.JsonIgnore]
		public string Expression
		{
			get
			{
				return string.Join("", this.Conditions.Select(c => c.Expression));
			}
		}

		[Newtonsoft.Json.JsonIgnore]
		public DynamicExpresso.Parameter[] Parameters
		{
			get
			{
				return this.Conditions.SelectMany(c => c.Parameters).ToArray();
			}
		}


		[Newtonsoft.Json.JsonIgnore]
		public File[] SourceFiles
		{
			get
			{
				System.Collections.Generic.List<File> files = new List<File>();
				DynamicExpresso.Lambda lambda = new DynamicExpresso.Interpreter().Parse(this.Expression, typeof(bool), this.Parameters);
				foreach (string filePath in System.IO.Directory.GetFiles(this.SourceDirectory, "*", System.IO.SearchOption.AllDirectories))
				{
					System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
					if ((bool)lambda.Invoke(this.Conditions.SelectMany(c => c.GetValues(fileInfo)).ToArray()))
						files.Add(new File(fileInfo));
				}
				return files.ToArray();
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
		}

		protected string GetTargetFilePath(string sourceFilePath)
		{
			return System.IO.Path.Combine(this.TargetDirectory, sourceFilePath.Substring(this.SourceDirectoryLength));
		}

		public void CopyFiles()
		{
			int fileCount = 0;
			long fileSize = 0;
			foreach(File sourceFile in this.SourceFiles)
			{
				if(sourceFile.Result!=CopyResult.复制成功)
				{
					string targetFilePath = this.GetTargetFilePath(sourceFile.Path);
					string targetDirectory = System.IO.Path.GetDirectoryName(targetFilePath);
					if(!System.IO.Directory.Exists(targetDirectory))
						try
						{
							System.IO.Directory.CreateDirectory(targetDirectory);
						}
						catch(System.Exception exception)
						{
							sourceFile.Result = CopyResult.复制失败;
							sourceFile.Error = exception.Message;
							//return exception.Message;
							continue;
						}
					try
					{
						System.IO.File.Copy(sourceFile.Path,targetFilePath);
					}
					catch(System.Exception exception)
					{
						sourceFile.Result = CopyResult.复制失败;
						sourceFile.Error = exception.Message;
						//return exception.Message;
						continue;
					}
				}
				sourceFile.Result = CopyResult.复制成功;
				fileCount++;
				fileSize += sourceFile.FileInfo.Length;
				this.OnFileCopied(fileCount, fileSize);
			}
			this.OnDone();
		}
	}
}