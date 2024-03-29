﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models.DataErrorInfos
{
	public class Job : Models.Job
	{
		//private string _Bracket;
		//public string Bracket
		//{
		//	get
		//	{
		//		return this._Bracket;
		//	}
		//	set
		//	{
		//		this._Bracket = value;
		//		this.NotifyOfPropertyChange(() => this.Bracket);
		//	}
		//}

		private string _Condition;
		public string Condition
		{
			get
			{
				return this._Condition;
			}
			set
			{
				this._Condition = value;
				this.NotifyOfPropertyChange(() => this.Condition);
			}
		}

		private string _CustomExpression;
		public override string CustomExpression
		{
			get
			{
				return this._CustomExpression;
			}
			set
			{
				this._CustomExpression = value;
				this.NotifyOfPropertyChange(() => this.CustomExpression);
			}
		}
	}
}