using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public class JobFile
	{
		public Job Job { get; protected set; }
		public File File { get; protected set; }

		public JobFile(Job job, File file)
		{
			this.Job = job;
			this.File = file;
		}
	}
}