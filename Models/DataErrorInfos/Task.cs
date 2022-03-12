using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models.DataErrorInfos
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

		private string _NowFormatString;
		public override string NowFormatString
		{
			get
			{
				return this._NowFormatString;
			}
			set
			{
				this._NowFormatString = value;
				this.NotifyOfPropertyChange(() => this.NowFormatString);
			}
		}
	}
}