using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public class Operator<T> : Operator<T, T>
	{
		public Operator(int operatorId, string name, OperatorHandler<T, T> expression) : base(operatorId, name, expression)
		{
		}
	}

	public class Operator<SourceType, ComparisonType>: IOperator
	{
		public int OperatorId { get; protected set; }

		public string Name { get; protected set; }

		private OperatorHandler<SourceType, ComparisonType> _Expression;
		public OperatorHandler<SourceType, ComparisonType> Expression
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
				return (this.OperatorId + 1) % 100 == 0;
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

		private string _VariableName;
		public string VariableName
		{
			get
			{
				if (this._VariableName == null)
					this._VariableName = this.Name + this.OperatorId;
				return this._VariableName;
			}
		}

		private DynamicExpresso.Parameter _Parameter;
		public DynamicExpresso.Parameter Parameter
		{
			get
			{
				if (this._Parameter == null)
					this._Parameter = new DynamicExpresso.Parameter(this.VariableName, this);
				return this._Parameter;
			}
		}

		public Operator(int operatorId, string name, OperatorHandler<SourceType, ComparisonType> expression)
		{
			this.OperatorId = operatorId;
			this.Name = name;
			this.Expression = expression;
		}

		//public readonly static Operator RegularExpressionOperator = new Operator(999, "RegularExpression", "{1}.IsMatch({0})");


		bool IOperator.Validate(object source, object comparison)
		{
			return this.Expression((SourceType)source, (ComparisonType)comparison);
		}
	}
}
