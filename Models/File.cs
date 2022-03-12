using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public class File : Caliburn.Micro.PropertyChangedBase
	{
		public static readonly System.Reflection.Assembly ExecutingAssembly = System.Reflection.Assembly.GetExecutingAssembly();

		public static readonly char[] InvalidFileNameCharacters = new char[] { '\\', '/', ':', '*', '?', '"', '>', '<', '|' };

		private string _Name;
		public string Name
		{
			get
			{
				if (this._Name == null)
					this._Name = System.IO.Path.GetFileNameWithoutExtension(this.Path);
				return this._Name;
			}
		}

		private string _Directory;
		public string Directory
		{
			get
			{
				if (this._Directory == null)
					this._Directory = System.IO.Path.GetDirectoryName(this.Path);
				return this._Directory;
			}
		}

		public string Path { get; set; }

		private CopyResult _Result;
		public CopyResult Result
		{
			get
			{
				return this._Result;
			}
			set
			{
				this._Result = value;
				this.NotifyOfPropertyChange(() => this.Result);
			}
		}

		private string _Error;
		public string Error
		{
			get
			{
				return this._Error;
			}
			set
			{
				this._Error = value;
				this.NotifyOfPropertyChange(() => this.Error);
			}
		}

		private System.IO.FileInfo _FileInfo;
		public System.IO.FileInfo FileInfo
		{
			get
			{
				if (this._FileInfo == null)
					this._FileInfo = new System.IO.FileInfo(this.Path);
				return this._FileInfo;
			}
		}

		public readonly int SourceDirectoryLength;

		private string _RelativeDirectoryPath;
		public string RelativeDirectoryPath
		{
			get
			{
				if (this._RelativeDirectoryPath == null)
					this._RelativeDirectoryPath = this.Directory.Substring(this.SourceDirectoryLength);
				return this._RelativeDirectoryPath;
			}
		}

		//public File()
		//{

		//}

		public File(System.IO.FileInfo fileInfo,int sourceDirectoryLength)
		{
			this._FileInfo = fileInfo;
			this.Path = fileInfo.FullName;

			this.SourceDirectoryLength = sourceDirectoryLength;
		}
	}
}