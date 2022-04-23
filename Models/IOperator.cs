using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public interface IOperator
	{
		int OperatorId { get; }

		bool Validate(object source, object comparison);

		bool IsRegularExpressionOperator { get; }

		string VariableName { get; }

		DynamicExpresso.Parameter Parameter { get; }
	}
}