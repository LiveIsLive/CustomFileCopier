using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public static class Operators
	{
		private static IOperator[] _StringOperators;
		public static IOperator[] StringOperators
		{
			get
			{
				if (_StringOperators == null)
					_StringOperators = new IOperator[]
					{
						new Operator<string>(101,"Contains",(souce,comparison)=>souce.Contains(comparison)),
						new Operator<string>(102,"NotContains",(souce,comparison)=>!souce.Contains(comparison)),
						new Operator<string>(103,"Equals",(souce,comparison)=>souce.Equals(comparison,System.StringComparison.OrdinalIgnoreCase)),
						new Operator<string>(104,"NotEquals",(souce,comparison)=>!souce.Equals(comparison,System.StringComparison.OrdinalIgnoreCase)),
						new Operator<string>(105,"StartsWith",(souce,comparison)=>souce.StartsWith(comparison)),
						new Operator<string>(106,"NotStartsWith",(souce,comparison)=>!souce.StartsWith(comparison)),
						new Operator<string>(107,"EndsWith",(souce,comparison)=>souce.EndsWith(comparison)),
						new Operator<string>(108,"NotEndsWith",(souce,comparison)=>!souce.EndsWith(comparison)),
						new RegularExpressionOperator<string>(199)
					};
				return _StringOperators;
			}
		}

		private static IOperator[] _DateTimeOperators;
		public static IOperator[] DateTimeOperators
		{
			get
			{
				if (_DateTimeOperators == null)
					_DateTimeOperators = new IOperator[]
					{
						new Operator<System.DateTime>(201,">",(souce,comparison)=>souce>comparison),
						new Operator<System.DateTime>(202,"≥",(souce,comparison)=>souce>=comparison),
						new Operator<System.DateTime>(203,"<",(souce,comparison)=>souce<comparison),
						new Operator<System.DateTime>(204,"≤",(souce,comparison)=>souce<=comparison),
						new Operator<System.DateTime>(205,"=",(souce,comparison)=>souce==comparison),
						new Operator<System.DateTime>(206,"≠",(souce,comparison)=>souce!=comparison),
						new RegularExpressionOperator<System.DateTime>(299)
					};
				return _DateTimeOperators;
			}
		}

		private static IOperator[] _Int64Operators;
		public static IOperator[] Int64Operators
		{
			get
			{
				if (_Int64Operators == null)
					_Int64Operators = new IOperator[]
					{
						new Operator<long>(301,">",(souce,comparison)=>souce>comparison),
						new Operator<long>(302,"≥",(souce,comparison)=>souce>=comparison),
						new Operator<long>(303,"<",(souce,comparison)=>souce<comparison),
						new Operator<long>(304,"≤",(souce,comparison)=>souce<=comparison),
						new Operator<long>(305,"=",(souce,comparison)=>souce==comparison),
						new Operator<long>(306,"≠",(souce,comparison)=>souce!=comparison),
						new RegularExpressionOperator<long>(399)
					};
				return _Int64Operators;
			}
		}

		public static readonly IOperator[] AllOperators = StringOperators.Concat(DateTimeOperators).Concat(Int64Operators).ToArray();

		public static System.Collections.Generic.Dictionary<System.Type, IOperator[]> TypedOperators = new Dictionary<Type, IOperator[]>() { { PropertyTypes.String, StringOperators }, { PropertyTypes.DateTime, DateTimeOperators }, { PropertyTypes.Int64, Int64Operators } };
	}
}