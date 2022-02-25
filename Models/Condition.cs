using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.SmartFileCopier.Models
{
	public class Condition : Caliburn.Micro.PropertyChangedBase
	{
		private int _PropertyId;
		[Newtonsoft.Json.JsonProperty]
		public int PropertyId
		{
			get
			{
				return this._PropertyId;
			}
			set
			{
				this._PropertyId = value;
				this._Property = null;
				this._Expression = null;
				this._Parameters = null;
				this._ValueType = null;

				this.Operator = this.Property.AllowOperators[0];
				this.NotifyOfPropertyChange(() => this.Property);
				this.NotifyOfPropertyChange(() => this.Operator);
				this.NotifyOfPropertyChange(() => this.ValueType);
			}
		}

		private int _OperatorId;
		[Newtonsoft.Json.JsonProperty]
		public int OperatorId
		{
			get
			{
				return this._OperatorId;
			}
			set
			{
				this._OperatorId = value;
				this._Operator = null;
				this._Expression = null;
				this._ValueType = null;
				this._Parameters = null;

				this.NotifyOfPropertyChange(() => this.Operator);
				this.NotifyOfPropertyChange(() => this.ValueType);
			}
		}

		[Newtonsoft.Json.JsonProperty]
		public bool LeftBracket { get; set; }

		[Newtonsoft.Json.JsonProperty]
		public bool RightBracket { get; set; }

		[Newtonsoft.Json.JsonProperty]
		public LogicalConnective? Connective { get; set; }

		private string _StringValue;
		[Newtonsoft.Json.JsonProperty]
		public string StringValue
		{
			get
			{
				return this._StringValue;
			}
			set
			{
				this._StringValue = value;
				this._RegularExpression = null;
				this.NotifyOfPropertyChange(() => this.StringValue);
			}
		}

		[Newtonsoft.Json.JsonProperty]
		public long LongValue { get; set; }

		[Newtonsoft.Json.JsonProperty]
		public System.DateTime DateTimeValue { get; set; } = System.DateTime.Today;

		public object Value
		{
			get
			{
				if (this.ValueType == PropertyTypes.String)
					return this.StringValue;
				if (this.ValueType == PropertyTypes.Int64)
					return this.LongValue;
				if (this.ValueType == PropertyTypes.DateTime)
					return this.DateTimeValue;
				throw new System.TypeUnloadedException($"The value type “{this.ValueType}” is unrecognized.");
			}
		}

		private System.Type _ValueType;
		public System.Type ValueType
		{
			get
			{
				if (this._ValueType == null)
					if (this.Operator == Operator.RegularExpressionOperator)
						this._ValueType = PropertyTypes.String;
					else this._ValueType = this.Property.Type;
				return this._ValueType;
			}
		}

		private System.Text.RegularExpressions.Regex _RegularExpression;
		public System.Text.RegularExpressions.Regex RegularExpression
		{
			get
			{
				if (this._RegularExpression == null)
					this._RegularExpression = new System.Text.RegularExpressions.Regex(this.Value.ToString());
				return this._RegularExpression;
			}
		}

		private Property _Property;
		public Property Property
		{
			get
			{
				if (this._Property == null)
					this._Property = Property.Properties.FirstOrDefault(p => p.PropertyId == this.PropertyId);
				return this._Property;
			}
			set                                       
			{
				this._PropertyId = value.PropertyId;
				this._Property = value;
				this._Expression = null;
				this._Parameters = null;
				this._ValueType = null;

				this.Operator = value.AllowOperators[0];
				this.NotifyOfPropertyChange(() => this.Property);
				this.NotifyOfPropertyChange(() => this.Operator);
				this.NotifyOfPropertyChange(() => this.ValueType);
			}
		}

		private Operator _Operator;
		public Operator Operator
		{
			get
			{
				if (this._Operator == null)
					this._Operator = Operator.AllOperators.FirstOrDefault(p => p.OperatorId == this.OperatorId);
				return this._Operator;
			}
			set
			{
				this._OperatorId = value.OperatorId;
				this._Operator = value;
				this._Expression = null;
				this._ValueType = null;
				this._Parameters = null;

				this.NotifyOfPropertyChange(() => this.Operator);
				this.NotifyOfPropertyChange(() => this.ValueType);
			}
		}

		protected static int VariableIndex;

		protected readonly string PropertyVariableName = "P" + VariableIndex++;

		protected readonly string ValueVariableName = "V" + VariableIndex++;

		protected static System.Collections.Generic.Dictionary<LogicalConnective, string> Connectives = new Dictionary<LogicalConnective, string>() { { LogicalConnective.And, "&&" }, { LogicalConnective.Or, "||" } };

		private string _Expression;
		public string Expression
		{
			get
			{
				if (this._Expression == null)
				{
					if (this.Connective != null)
						this._Expression = Connectives[this.Connective.Value];
					if (this.LeftBracket)
						this._Expression += "(";
					this._Expression += string.Format(this.Operator.Expression, this.PropertyVariableName, this.ValueVariableName);
					if (this.RightBracket)
						this._Expression += ")";
				}
				return this._Expression;
			}
		}

		private DynamicExpresso.Parameter[] _Parameters;
		public DynamicExpresso.Parameter[] Parameters
		{
			get
			{
				if (this._Parameters == null)
					if (this.Operator.IsRegularExpressionOperator)
						this._Parameters = new DynamicExpresso.Parameter[] { new DynamicExpresso.Parameter(this.PropertyVariableName, typeof(string)), new DynamicExpresso.Parameter(this.ValueVariableName, typeof(System.Text.RegularExpressions.Regex))};
					else this._Parameters = new DynamicExpresso.Parameter[] { new DynamicExpresso.Parameter(this.PropertyVariableName, this.Property.Type), new DynamicExpresso.Parameter(this.ValueVariableName, this.Property.Type)};
				return this._Parameters;
			}
		}

		private Models.DataErrorInfos.Condition _DataErrorInfo;
		public Models.DataErrorInfos.Condition DataErrorInfo
		{
			get
			{
				return this._DataErrorInfo;
			}
			protected set
			{
				this._DataErrorInfo = value;
				this.NotifyOfPropertyChange(() => this.DataErrorInfo);
			}
		}

		public object[] GetValues(System.IO.FileInfo fileInfo)
		{
			if (this.Operator.IsRegularExpressionOperator)
				return new object[] { this.Property.GetValue(fileInfo).ToString(), this.RegularExpression };
			return new object[] { this.Property.GetValue(fileInfo), this.Value };

			//System.Collections.Generic.List<int> integerList = new System.Collections.Generic.List<int>();
			//System.Console.WriteLine("请输入数组的元素，每行一个：");
			//for(int i=0;i<=10;i++)
			//{
			//	string s = System.Console.ReadLine();
			//	int item;
			//	if(!int.TryParse(s,out item))
			//	{
			//		System.Console.WriteLine("输入格式不正确，请输入非负整数！");
			//		i--;
			//		continue;
			//	}
			//	if(item<0)
			//	{
			//		System.Console.WriteLine("遇到负数，输入结束");
			//		break;
			//	}
			//	integerList.Add(item);
			//	if(integerList.Count==10)
			//		System.Console.WriteLine("已经输入10个非负整数，输入结束！");
			//}
			////输入结果
			//int[] integers = integerList.ToArray();
		}


		public bool ValidateData(Localization localization)
		{
			this.DataErrorInfo = new DataErrorInfos.Condition();
			//bool result = true;

			if(this.ValueType==PropertyTypes.String)
				if(string.IsNullOrWhiteSpace(this.StringValue))
				{
					this.DataErrorInfo.StringValue = string.Format(localization.ValidationError[ValidationError.Required], localization.Value);
					return false;
				}

			if(this.Operator==Operator.RegularExpressionOperator)
				try
				{
					this.RegularExpression.Match("");
				}
				catch(System.Exception exception)
				{
						this.DataErrorInfo.StringValue = localization.ValidationError[ValidationError.InvalidRegularExpression] + exception.Message;
					return false;
				}

			return true;
			//return result;
		}

	}
}