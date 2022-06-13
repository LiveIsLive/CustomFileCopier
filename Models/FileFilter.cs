using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public abstract class FileFilter
	{
		public abstract System.Collections.Generic.IEnumerable<System.IO.FileInfo> GetFiles(System.Collections.Generic.IEnumerable<System.IO.FileInfo> fileInfos);
	}

	public partial class FileFilterTemplate
	{
		public System.Collections.Generic.IList<Condition> Conditions;
	}
}