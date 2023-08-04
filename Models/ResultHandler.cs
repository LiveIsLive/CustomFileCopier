using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public abstract class ResultHandler : Caliburn.Micro.PropertyChangedBase
	{
		public abstract string Name { get; }

		public abstract bool Remote { get; }

		protected static readonly System.Globalization.CultureInfo ChineseCulture;
		protected static readonly System.Globalization.CultureInfo EnglishCulture;

		public abstract string CheckTargetDirectoryValid(Models.Job job);

		public abstract System.Threading.Tasks.Task<bool> TargetDirectoryEmpty(Models.Job job);

		public abstract System.Threading.Tasks.Task Execute(Models.Job job);

		static ResultHandler()
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

		private static System.Collections.Generic.Dictionary<string,ResultHandler> _All;
		public static System.Collections.Generic.Dictionary<string, ResultHandler> All
		{
			get
			{
				if (_All == null)
				{
					System.Type baseType = typeof(ResultHandler);
					_All = System.Reflection.Assembly.Load("ColdShineSoft.CustomFileCopier.Handlers").GetTypes().Where(t=>t.IsSubclassOf(baseType)).Select(t => (ResultHandler)System.Activator.CreateInstance(t)).OrderBy(h => h.Remote).ToDictionary(h => h.GetTypeFullName()); ;
				}
				return _All;
			}
		}
	}
}