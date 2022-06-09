using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public abstract class BaseProcessor
	{
		public readonly System.Collections.Generic.IEnumerable<ColdShineSoft.CustomFileCopier.Models.Condition> Conditions;

		public BaseProcessor(System.Collections.Generic.IEnumerable<ColdShineSoft.CustomFileCopier.Models.Condition> conditions)
		{
			this.Conditions = conditions;
		}

		public abstract bool Check(System.IO.FileInfo fileInfo);
	}

	public partial class ProcessorTemplate
	{
		public Condition[] Conditions;
	}
}