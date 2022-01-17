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

		private File[] _Files = new File[0];
		public File[] Files
		{
			get
			{
				if (this._Files == null)
					this._Files = this.Jobs.SelectMany(j => j.SourceFiles).ToArray();
				return this._Files;
			}
		}

		public event System.Action<(int CopiedCount, long CopiedSize)> FileCopied;
		protected void OnFileCopied(int copiedCount, long copiedSize)
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

		public void LoadFiles()
		{
			this._Files = null;
			this.NotifyOfPropertyChange(() => this.Files);
		}

		public void CopyFiles()
		{
			foreach (Job job in this.Jobs)
				job.CopyFiles();
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