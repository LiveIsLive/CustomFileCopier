using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.SmartFileCopier.Models
{
	public class File
	{
		private string _Name;
		[Newtonsoft.Json.JsonIgnore]
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
		[Newtonsoft.Json.JsonIgnore]
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

		public CopyResult Result { get; set; }

		public string Error { get; set; }

		private System.IO.FileInfo _FileInfo;
		[Newtonsoft.Json.JsonIgnore]
		public System.IO.FileInfo FileInfo
		{
			get
			{
				if (this._FileInfo == null)
					this._FileInfo = new System.IO.FileInfo(this.Path);
				return this._FileInfo;
			}
		}

		public int SourceDirectoryLength;

		public File()
		{

		}

		public File(System.IO.FileInfo fileInfo,int sourceDirectoryLength)
		{
			this._FileInfo = fileInfo;
			this.Path = fileInfo.FullName;

			this.SourceDirectoryLength = sourceDirectoryLength;
		}
	}
}