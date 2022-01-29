using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.SmartFileCopier.Models
{
	public enum TaskStatus
	{
		Standby,
		CollectingFiles,
		Copying,
		Done
	}
}