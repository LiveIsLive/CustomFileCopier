using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public class RegularExpressionOperator<SourceType> : Operator<SourceType,System.Text.RegularExpressions.Regex>
	{
		public RegularExpressionOperator(int operatorId) : base(operatorId, "RegularExpression",(source, regex) => regex.IsMatch(source.ToString()))
		{
		}
	}
}