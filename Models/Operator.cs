using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColdShineSoft.SmartFileCopier.Models
{
	public class Operator
	{
		public int OperatorId { get; protected set; }

		public string Name { get; protected set; }


		private string _Expression;
		public string Expression
		{
			get
			{
				return this._Expression;
			}
			set
			{
				this._Expression = value;
			}
		}

		public bool IsRegularExpressionOperator
		{
			get
			{
				return this == RegularExpressionOperator;
			}
		}

		//private System.Text.RegularExpressions.Regex _RegularExpression;
		//public System.Text.RegularExpressions.Regex RegularExpression
		//{
		//	get
		//	{
		//		if (this._RegularExpression == null)
		//			this._RegularExpression = new System.Text.RegularExpressions.Regex(this.Expression);
		//		return this._RegularExpression;
		//	}
		//}

		public Operator(int operatorId, string name, string expression)
		{
			this.OperatorId = operatorId;
			this.Name = name;
			this.Expression = expression;
		}

		public readonly static Operator RegularExpressionOperator = new Operator(999, "RegularExpression", "{1}.IsMatch({0})");

		private static Operator[] _StringOperators;
		public static Operator[] StringOperators
		{
			get
			{
				if (_StringOperators == null)
					_StringOperators = new Operator[]
					{
						new Operator(101,"Contains","{0}.Contains({1})"),
						new Operator(102,"NotContains","!{0}.Contains({1})"),
						new Operator(103,"Equals","{0}=={1}"),
						new Operator(103,"NotEquals","{0}!={1}"),
						new Operator(104,"StartsWith","{0}.StartsWith({1})"),
						new Operator(105,"NotStartsWith","!{0}.StartsWith({1})"),
						new Operator(106,"EndsWith","{0}.EndsWith({1})"),
						new Operator(107,"NotEndsWith","!{0}.EndsWith({1})"),
						RegularExpressionOperator
					};
				return _StringOperators;
			}
		}

		private static Operator[] _DateTimeOperators;
		public static Operator[] DateTimeOperators
		{
			get
			{
				if (_DateTimeOperators == null)
					_DateTimeOperators = new Operator[]
					{
						new Operator(201,">","{0}>{1}"),
						new Operator(202,"≥","{0}>={1}"),
						new Operator(203,"<","{0}<{1}"),
						new Operator(204,"≤","{0}<={1}"),
						new Operator(205,"=","{0}={1}"),
						new Operator(206,"≠","{0}!={1}"),
						RegularExpressionOperator
					};
				return _DateTimeOperators;
			}
		}

		private static Operator[] _Int64Operators;
		public static Operator[] Int64Operators
		{
			get
			{
				if (_Int64Operators == null)
					_Int64Operators = new Operator[]
					{
						new Operator(301,">","{0}>{1}"),
						new Operator(302,"≥","{0}>={1}"),
						new Operator(303,"<","{0}<{1}"),
						new Operator(304,"≤","{0}<={1}"),
						new Operator(305,"=","{0}={1}"),
						new Operator(306,"≠","{0}!={1}"),
						RegularExpressionOperator
					};
				return _Int64Operators;
			}
		}

		public static readonly Operator[] AllOperators = StringOperators.Concat(DateTimeOperators).Concat(Int64Operators).ToArray();

		public static System.Collections.Generic.Dictionary<System.Type, Operator[]> Operators = new Dictionary<Type, Operator[]>(){ { PropertyTypes.String, StringOperators }, { PropertyTypes.DateTime, DateTimeOperators }, { PropertyTypes.Int64, Int64Operators } };
	}
}
