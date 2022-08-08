using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public static class ExtentionMethods
	{
		public static string GetDisplayName(this System.Type type)
		{
			return $"{type.FullName.Substring(0, type.FullName.IndexOf("`"))}<{string.Join(",", type.GenericTypeArguments.Select(t => t.FullName))}>";
		}

		public static string GetGenericTypeFullName(this object o)
		{
			//System.Collections.Generic.KeyValuePair`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]

			System.Type type = o.GetType();
			return $"{System.Text.RegularExpressions.Regex.Match(type.FullName, @"((\.?\w+)+)`").Groups[1].Value}<{string.Join(",",System.Text.RegularExpressions.Regex.Matches(type.FullName, @"\[((\.?\w+)+).*?]").Cast<System.Text.RegularExpressions.Match>().Select(m=>m.Groups[1].Value))}>";
		}

		public static string GetTypeFullName(this object o)
		{
			System.Type type = o.GetType();
			return $"{type.FullName},{type.Assembly.GetName().Name}";
		}
	}
}