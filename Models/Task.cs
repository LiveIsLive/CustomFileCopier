using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace ColdShineSoft.SmartFileCopier.Models
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
					//this._Jobs.CollectionChanged += (sender, e) =>
					//{
					//	if (e.NewItems != null)
					//		foreach (Job job in e.NewItems)
					//		{
					//			job.FileCopied += j => this.OnFileCopied(j.CopiedCount, j.CopiedSize);
					//			if (!this.CompressTargetDirectory)
					//				job.SpecifyTargetDirectory = true;
					//		}
					//};
				}
				return this._Jobs;
			}
		}

		private bool _CompressTargetDirectory;
		[Newtonsoft.Json.JsonProperty]
		public bool CompressTargetDirectory
		{
			get
			{
				return this._CompressTargetDirectory;
			}
			set
			{
				this._CompressTargetDirectory = value;
				this.NotifyOfPropertyChange(() => this.CompressTargetDirectory);
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

		public event System.Action<(int CopiedCount, long CopiedSize)> FileCopied;
		protected void OnFileCopied(int copiedCount, long copiedSize)
		{
			this.CopiedFileCount = this.Jobs.Sum(f => f.CopiedFileCount); ;
			this.CopiedFileSize = this.Jobs.Sum(f => f.CopiedFileSize); ;
			if (this.FileCopied != null)
				this.FileCopied((this.CopiedFileCount, this.CopiedFileSize));
		}

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
			this.Files = null;
			this.CopiedFileSize = 0;
			this.CopiedFileCount = 0;
		}

		public void CopyFiles()
		{
			foreach (Job job in this.Jobs)
				job.CopyFiles();
		}

		public void Run()
		{
			this.Status = TaskStatus.CollectingFiles;
			this.LoadFiles();
			this.Status = TaskStatus.Copying;
			this.CopyFiles();
			if (this.CompressTargetDirectory && this.Jobs.Count > 0)
			{
				this.Status = TaskStatus.Compressing;
				var groups = (from job in this.Jobs group job by job.RealTargetDirectoryPath.ToLower() into g select g.ToArray()).ToArray();
				if (groups.Length == 1)
				{
					if(System.IO.File.Exists(this.CompressFilePath))
						//try
						//{
							System.IO.File.Move(this.CompressFilePath, System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.CompressFilePath), $"{System.IO.Path.GetFileNameWithoutExtension(this.CompressFilePath)}(Old_{System.DateTime.Now.ToString("yyyyMMddHHmmss")}).zip"));
						//}
						//catch
						//{
						//	System.IO.File.Delete(this.CompressFilePath);
						//}
					System.IO.Compression.ZipFile.CreateFromDirectory(this.Jobs[0].RealTargetDirectoryPath, this.CompressFilePath, System.IO.Compression.CompressionLevel.Optimal, false);
				}
				else
				{
					System.IO.FileStream stream = new System.IO.FileStream(this.CompressFilePath, System.IO.FileMode.Create);
					using (System.IO.Compression.ZipArchive zip = new System.IO.Compression.ZipArchive(stream, System.IO.Compression.ZipArchiveMode.Create))
					{
						foreach (Job[] jobs in groups)
						{
							string directory = string.Join("、", jobs.Select(j => j.JobNameToDirectoryName));
							foreach (string file in System.IO.Directory.GetFiles(jobs[0].RealTargetDirectoryPath, "*", System.IO.SearchOption.AllDirectories))
								zip.CreateEntryFromFile(file, System.IO.Path.Combine(directory, file.Substring(jobs[0].RealTargetDirectoryLength)));
						}
					}
					stream.Close();
				}
			}
			foreach(Job job in this.Jobs)
				if(!job.SpecifyTargetDirectory)
					try
					{
						System.IO.Directory.Delete(job.RealTargetDirectoryPath, true);
					}
					catch
					{
					}
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
			if(this.CompressTargetDirectory)
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
						result = false;
						System.IO.File.WriteAllBytes(this.CompressFilePath, new byte[1]);
						System.IO.File.Delete(this.CompressFilePath);
					}
					catch (System.Exception exception)
					{
						this.DataErrorInfo.CompressFilePath = string.Format(localization.ValidationError[ValidationError.InvalidFilePath], localization.CompressFilePath) + exception.Message;
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