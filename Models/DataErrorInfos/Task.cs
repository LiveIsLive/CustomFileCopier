using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.SmartFileCopier.Models.DataErrorInfos
{
	public class Task : Models.Task
	{
		private string _Job;
		public string Job
		{
			get
			{
				return this._Job;
			}
			set
			{
				this._Job = value;
				this.NotifyOfPropertyChange(() => this.Job);
			}
		}

		public Models.Job LastInvalidJob;
	}
}