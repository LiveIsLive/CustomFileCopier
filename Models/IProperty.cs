using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public interface IProperty
	{
		int PropertyId { get; }

		string VariableName { get; }

		//DynamicExpresso.Parameter Parameter { get; }

		IOperator[] AllowOperators { get; }

		System.Type Type { get; }
	}
}