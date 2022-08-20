using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public class Task : Caliburn.Micro.PropertyChangedBase
	{
		private System.Collections.ObjectModel.ObservableCollection<Job> _Jobs;
		[Newtonsoft.Json.JsonProperty]
		public System.Collections.ObjectModel.ObservableCollection<Job> Jobs
		{
			get
			{
				if(this._Jobs==null)
				{
					this._Jobs = new System.Collections.ObjectModel.ObservableCollection<Job>();
					this._Jobs.CollectionChanged += (sender, e) =>
					{
						if (e.NewItems != null)
							foreach (Job job in e.NewItems)
							{
								//job.FileCopied += j => this.OnFileCopied(j.CopiedCount, j.CopiedSize);
								job.Task = this;
								//if (!this.CompressToZipFile)
								//	job.SpecifyTargetDirectory = true;
							}
					};
				}
				return this._Jobs;
			}
		}

		private bool _CompressToZipFile;
		[Newtonsoft.Json.JsonProperty]
		public bool CompressToZipFile
		{
			get
			{
				return this._CompressToZipFile;
			}
			set
			{
				this._CompressToZipFile = value;
				this.NotifyOfPropertyChange(() => this.CompressToZipFile);
				//if (value && string.IsNullOrWhiteSpace(this.CompressFilePath) && this.Jobs.Count > 0)
				//	this.CompressFilePath = this.Jobs[0].TargetDirectoryPath.TrimEnd('\\', '/') + ".zip";
				//if (!value)
				//	foreach (Job job in this.Jobs)
				//		job.SpecifyTargetDirectory = true;
			}
		}

		private string _CompressFilePath;
		[Newtonsoft.Json.JsonProperty]
		public string CompressFilePath
		{
			get
			{
				return this._CompressFilePath;
			}
			set
			{
				this._CompressFilePath = value;
				this.NotifyOfPropertyChange(() => this.CompressFilePath);
			}
		}

		private bool _AddNowToCompressFileName = true;
		[Newtonsoft.Json.JsonProperty]
		public bool AddNowToCompressFileName
		{
			get
			{
				return this._AddNowToCompressFileName;
			}
			set
			{
				this._AddNowToCompressFileName = value;
				this.NotifyOfPropertyChange(() => this.AddNowToCompressFileName);
			}
		}

		private string _NowFormatString = "(yyyyMMddHHmm)";
		[Newtonsoft.Json.JsonProperty]
		public virtual string NowFormatString
		{
			get
			{
				return this._NowFormatString;
			}
			set
			{
				this._NowFormatString = value;
				this.NotifyOfPropertyChange(() => this.NowFormatString);
			}
		}

		private string _TargetCompressFilePath;
		public string TargetCompressFilePath
		{
			get
			{
				if (this._TargetCompressFilePath == null)
					if (this.AddNowToCompressFileName)
						this._TargetCompressFilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.CompressFilePath), System.IO.Path.GetFileNameWithoutExtension(this.CompressFilePath) + System.DateTime.Now.ToString(this.NowFormatString)) + System.IO.Path.GetExtension(this.CompressFilePath);
					else this._TargetCompressFilePath = this.CompressFilePath;
				return this._TargetCompressFilePath;
			}
		}

		[Newtonsoft.Json.JsonProperty]
		public bool AutoRunWhenFilesFiltered { get; set; }

		private TaskStatus _Status;
		public TaskStatus Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				this._Status = value;
				this.NotifyOfPropertyChange(() => this.Status);
			}
		}

		private JobFile[] _Files = new JobFile[0];
		public JobFile[] Files
		{
			get
			{
				lock(string.Empty)
				if (this._Files == null)
					this._Files = this.Jobs.SelectMany(j => j.SourceFiles.Select(f => new JobFile(j, f))).ToArray();
				return this._Files;
			}
			protected set
			{
				this._Files = value;
				this._TotalFileSize = null;
				this.NotifyOfPropertyChange(() => this.Files);
				this.NotifyOfPropertyChange(() => this.TotalFileSize);
			}
		}

		private long? _TotalFileSize;
		public long TotalFileSize
		{
			get
			{
				if (this._TotalFileSize == null)
					this._TotalFileSize = this.Files.Sum(f => f.File.FileInfo.Length);
				return this._TotalFileSize.Value;
			}
		}

		private long _CopiedFileSize;
		public long CopiedFileSize
		{
			get
			{
				return this._CopiedFileSize;
			}
			set
			{
				this._CopiedFileSize = value;
				this.NotifyOfPropertyChange(() => this.CopiedFileSize);
			}
		}

		private int _CopiedFileCount;
		public int CopiedFileCount
		{
			get
			{
				return this._CopiedFileCount;
			}
			set
			{
				this._CopiedFileCount = value;
				this.NotifyOfPropertyChange(() => this.CopiedFileCount);
			}
		}

		//public event System.Action<(int CopiedCount, long CopiedSize)> FileCopied;
		//protected void OnFileCopied(int copiedCount, long copiedSize)
		//{
		//	this.CopiedFileCount = this.Jobs.Sum(f => f.CopiedFileCount); ;
		//	this.CopiedFileSize = this.Jobs.Sum(f => f.CopiedFileSize); ;
		//	if (this.FileCopied != null)
		//		this.FileCopied((this.CopiedFileCount, this.CopiedFileSize));
		//}

		private Models.DataErrorInfos.Task _DataErrorInfo;
		public Models.DataErrorInfos.Task DataErrorInfo
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

		public event System.Action Done;
		protected void OnDone()
		{
			if (this.Done != null)
				this.Done();
		}

		public void LoadFiles()
		{
			foreach (Job job in this.Jobs)
				job.ClearCache();
			this.Files = null;
			this.CopiedFileSize = 0;
			this.CopiedFileCount = 0;
			this.Files.ToString();
		}

		public void Run()
		{
			this._TargetCompressFilePath = null;
			this.Status = TaskStatus.CollectingFiles;
			this.LoadFiles();
			if (this.AutoRunWhenFilesFiltered)
				this.CopyFiles();
			else this.Status = TaskStatus.Standby;
		}

		public void CopyFiles()
		{
			this.Status = TaskStatus.Copying;
			if (this.CompressToZipFile)
			{
				try
				{
					this.Status = TaskStatus.Compressing;

					System.IO.FileStream resultFile = new System.IO.FileStream(this.TargetCompressFilePath, System.IO.FileMode.Create);
					using (System.IO.Compression.ZipArchive zip = new System.IO.Compression.ZipArchive(resultFile, ZipArchiveMode.Create))
					{
						foreach (JobFile jobFile in this.Files)
						{
							string path = jobFile.Job.GetTargetRelativeFilePath(jobFile.File.Path);
							if (this.Jobs.Count > 1)
								path = System.IO.Path.Combine(jobFile.Job.JobNameToDirectoryName, path);
							try
							{
								zip.CreateEntryFromFile(jobFile.File.Path, path);
								jobFile.File.Result = CopyResult.Success;
							}
							catch(System.Exception exception)
							{
								jobFile.File.Result = CopyResult.Failure;
								jobFile.File.Error = exception.Message;
							}
							this.CopiedFileSize += jobFile.File.FileInfo.Length;
							this.CopiedFileCount++;
						}
					}
					resultFile.Close();


				}
				finally
				{
					this._TargetCompressFilePath = null;
				}
			}
			else foreach (Models.Job job in this.Jobs)
					job.Execute();
			this.Status = TaskStatus.Done;
			this.OnDone();

		}

		public void Save(string path)
		{
			System.IO.StreamWriter writer = new System.IO.StreamWriter(path);
			//NetJSON.NetJSON.Serialize(this, stream);
			new Newtonsoft.Json.JsonSerializer().Serialize(writer, this);
			writer.Close();
			Setting.Instance.AddRecentFile(path);
		}

		public static Task Open(string path)
		{
			System.IO.StreamReader stream = new System.IO.StreamReader(path);
			try
			{
				//return NetJSON.NetJSON.Deserialize<Task>(stream);
				return new Newtonsoft.Json.JsonSerializer().Deserialize<Task>(new Newtonsoft.Json.JsonTextReader(stream));
			}
			finally
			{
				stream.Close();
				Setting.Instance.AddRecentFile(path);
			}
		}

		public bool ValidateData(Localization localization)
		{
			this.DataErrorInfo = new DataErrorInfos.Task();
			bool result = true;
			if(this.CompressToZipFile)
			{
				if(string.IsNullOrWhiteSpace(this.CompressFilePath))
				{
					result = false;
					this.DataErrorInfo.CompressFilePath = string.Format(localization.ValidationError[ValidationError.Required], localization.CompressFilePath);
				}
				else if (System.IO.File.Exists(this.CompressFilePath))
					try
					{
						System.IO.File.OpenWrite(this.CompressFilePath).Close();
					}
					catch (System.Exception exception)
					{
						result = false;
						this.DataErrorInfo.CompressFilePath = string.Format(localization.ValidationError[ValidationError.InvalidFilePath], localization.CompressFilePath) + exception.Message;
					}
				else
					try
					{
						System.IO.File.WriteAllBytes(this.CompressFilePath, new byte[1]);
						System.IO.File.Delete(this.CompressFilePath);
					}
					catch (System.Exception exception)
					{
						result = false;
						this.DataErrorInfo.CompressFilePath = string.Format(localization.ValidationError[ValidationError.InvalidFilePath], localization.CompressFilePath) + exception.Message;
					}

				if(this.AddNowToCompressFileName)
					if(string.IsNullOrWhiteSpace(this.NowFormatString))
					{
						result = false;
						this.DataErrorInfo.NowFormatString = string.Format(localization.ValidationError[ValidationError.Required], localization.NowFormatString);
					}
					else
					{
						string postfix = null;
						try
						{
							postfix = System.DateTime.Now.ToString(this.NowFormatString);
						}
						catch
						{
							result = false;
							this.DataErrorInfo.NowFormatString = localization.ValidationError[ValidationError.InvalidDateTimeFormatString];
						}
						foreach(char c in File.InvalidFileNameCharacters)
							if(postfix.Contains(c))
							{
								result = false;
								this.DataErrorInfo.NowFormatString = string.Format(localization.ValidationError[ValidationError.InvalidFileNameCharacter], c);
								break;
							}
					}
			}

			if(this.Jobs.Count==0)
			{
				result = false;
				this.DataErrorInfo.Job = string.Format(localization.ValidationError[ValidationError.Required], localization.Job);
			}

			foreach(Job job in this.Jobs)
				if(!job.ValidateData(localization))
				{
					result = false;
					this.DataErrorInfo.LastInvalidJob = job;
				}

			return result;
		}
	}
}