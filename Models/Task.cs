using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
					this._Jobs.CollectionChanged += (sender, e) =>
					{
						if (e.NewItems != null)
							foreach (Job job in e.NewItems)
								job.FileCopied += j => this.OnFileCopied(j.CopiedCount, j.CopiedSize);
					};
				}
				return this._Jobs;
			}
		}

		[Newtonsoft.Json.JsonProperty]
		public bool ZipTargetDirectory { get; set; }

		[Newtonsoft.Json.JsonProperty]
		public string ZipFilePath { get; set; }

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
	}
}