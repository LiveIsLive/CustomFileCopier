using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public abstract class FileFilter
	{
		public abstract System.Collections.Generic.IEnumerable<System.IO.FileInfo> GetFiles(string sourceDirectoryPath);
	}

	public partial class FileFilterTemplate
	{
		public Models.Job Job;
	}
}