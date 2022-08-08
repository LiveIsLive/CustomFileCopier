using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Handlers
{
	public abstract class BaseHandler : Caliburn.Micro.PropertyChangedBase
	{
		public Models.Job Job { get; set; }

		public abstract string Name { get; }

		public abstract bool Remote { get; }

		protected static readonly System.Globalization.CultureInfo ChineseCulture;
		protected static readonly System.Globalization.CultureInfo EnglishCulture;

		public abstract void Execute();

		static BaseHandler()
		{
			try
			{
				EnglishCulture = System.Globalization.CultureInfo.GetCultureInfo("en");
				ChineseCulture = System.Globalization.CultureInfo.GetCultureInfo("zh-CN");
			}
			catch
			{
			}
		}
	}
}