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

				this.NotifyOfPropertyChange(() => this.Operator);
				this.NotifyOfPropertyChange(() => this.ValueType);
			}
		}

		public bool LeftBracket { get; set; }

		public bool RightBracket { get; set; }

		public LogicalConnective? Connective { get; set; }

		private object _Value;
		public object Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				if (this.Property.Type == PropertyTypes.DateTime && value is string)
					this._Value = Newtonsoft.Json.JsonConvert.DeserializeObject<System.DateTime>(value.ToString());
					//this._Value = NetJSON.NetJSON.Deserialize<System.DateTime>(value.ToString());
				else this._Value = value;

				this._RegularExpression = null;
			}
		}

		private System.Type _ValueType;
		[Newtonsoft.Json.JsonIgnore]
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
		[Newtonsoft.Json.JsonIgnore]
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
		[Newtonsoft.Json.JsonIgnore]
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
		[Newtonsoft.Json.JsonIgnore]
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

				this.NotifyOfPropertyChange(() => this.Operator);
				this.NotifyOfPropertyChange(() => this.ValueType);
			}
		}

		protected static int VariableIndex;

		protected readonly string PropertyVariableName = "P" + VariableIndex++;

		protected readonly string ValueVariableName = "V" + VariableIndex++;

		protected static System.Collections.Generic.Dictionary<LogicalConnective, string> Connectives = new Dictionary<LogicalConnective, string>() { { LogicalConnective.And, "&&" }, { LogicalConnective.Or, "||" } };

		private string _Expression;
		[Newtonsoft.Json.JsonIgnore]
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
		[Newtonsoft.Json.JsonIgnore]
		public DynamicExpresso.Parameter[] Parameters
		{
			get
			{
				if (this._Parameters == null)
					if (this.Operator.IsRegularExpressionOperator)
						this._Parameters = new DynamicExpresso.Parameter[] { new DynamicExpresso.Parameter(this.PropertyVariableName, typeof(string)), new DynamicExpresso.Parameter(this.PropertyVariableName, typeof(string))};
					else this._Parameters = new DynamicExpresso.Parameter[] { new DynamicExpresso.Parameter(this.PropertyVariableName, this.Property.Type), new DynamicExpresso.Parameter(this.PropertyVariableName, this.Property.Type)};
				return this._Parameters;
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
	}
}