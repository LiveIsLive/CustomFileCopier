using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public class Property
	{
		public int PropertyId { get; protected set; }

		public string Name { get; protected set; }

		[Newtonsoft.Json.JsonIgnore]
		public System.Func<System.IO.FileInfo, object> GetValue { get; protected set; }

		private System.Type _Type;
		[Newtonsoft.Json.JsonIgnore]
		public System.Type Type
		{
			get
			{
				if (this._Type == null)
					this._Type = this.GetValue(ExecutableFileInfo).GetType();
				return this._Type;
			}
		}

		protected static readonly System.IO.FileInfo ExecutableFileInfo = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

		private Operator[] _AllowOperators;
		public Operator[] AllowOperators
		{
			get
			{
				if (this._AllowOperators == null)
					this._AllowOperators = Operator.Operators[this.Type];
				return this._AllowOperators;
			}
		}

		public Property(int propertyId, string name, Func<FileInfo, object> getValue)
		{
			this.PropertyId = propertyId;
			this.Name = name;
			this.GetValue = getValue;
		}

		public static readonly Property[] Properties = new Property[]
		{
			new Property(101,"FileName",f=>f.Name)
			,new Property(102,"FilePath",f=>f.FullName)
			,new Property(103,"FileSize",f=>f.Length)
			,new Property(104,"DirectoryPath",f=>f.DirectoryName)
			,new Property(105,"Extension",f=>f.Extension)
			,new Property(106,"LastWriteTime",f=>f.LastWriteTime)
			,new Property(107,"CreationTime",f=>f.CreationTime)
			,new Property(108,"LastAccessTime",f=>f.LastAccessTime)
		};
	}
}