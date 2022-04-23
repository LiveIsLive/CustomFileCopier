using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public static class Properties
	{
		public static readonly IProperty[] FileProperties = new IProperty[]
		{
			new Property<string>(101,"FileName",f=>f.Name)
			,new Property<string>(102,"FilePath",f=>f.FullName)
			,new Property<long>(103,"FileSize",f=>f.Length)
			,new Property<string>(104,"DirectoryPath",f=>f.DirectoryName)
			,new Property<string>(105,"Extension",f=>f.Extension)
			,new Property<System.DateTime>(106,"LastWriteTime",f=>f.LastWriteTime)
			,new Property<System.DateTime>(107,"CreationTime",f=>f.CreationTime)
			,new Property<System.DateTime>(108,"LastAccessTime",f=>f.LastAccessTime)
		};
	}
}