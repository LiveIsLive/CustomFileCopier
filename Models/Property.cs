using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public class Property<T>: IProperty
	{
		public int PropertyId { get; protected set; }

		public string Name { get; protected set; }

		[Newtonsoft.Json.JsonIgnore]
		public System.Func<System.IO.FileInfo, T> GetValue { get; protected set; }

		//private System.Func<System.IO.FileInfo, object> _GetPropertyValue;
		//public System.Func<System.IO.FileInfo, object> GetPropertyValue
		//{
		//	get
		//	{
		//		if (this._GetPropertyValue == null)
		//			this._GetPropertyValue = fileInfo => this.GetValue((System.IO.FileInfo)fileInfo);
		//		return this._GetPropertyValue;
		//	}
		//}

		private System.Type _Type;
		[Newtonsoft.Json.JsonIgnore]
		public System.Type Type
		{
			get
			{
				if (this._Type == null)
					this._Type = typeof(T);
					//this._Type = this.GetValue(ExecutableFileInfo).GetType();
				return this._Type;
			}
		}

		protected static readonly System.IO.FileInfo ExecutableFileInfo = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

		private string _PropertyVariableName;
		public string PropertyVariableName
		{
			get
			{
				if (this._PropertyVariableName == null)
					this._PropertyVariableName = this.Name + "P";
				return this._PropertyVariableName;
			}
		}

		private string _ValueVariableName;
		public string ValueVariableName
		{
			get
			{
				if (this._ValueVariableName == null)
					this._ValueVariableName = this.Name + "V";
				return this._ValueVariableName;
			}
		}

		//private DynamicExpresso.Parameter _Parameter;
		//public DynamicExpresso.Parameter Parameter
		//{
		//	get
		//	{
		//		if (this._Parameter == null)
		//			this._Parameter = new DynamicExpresso.Parameter(this.ValueVariableName, this.Type);
		//		return this._Parameter;
		//	}
		//}

		private IOperator[] _AllowOperators;
		public IOperator[] AllowOperators
		{
			get
			{
				if (this._AllowOperators == null)
					this._AllowOperators = Operators.TypedOperators[this.Type];
				return this._AllowOperators;
			}
		}

		public Property(int propertyId, string name, Func<FileInfo, T> getValue)
		{
			this.PropertyId = propertyId;
			this.Name = name;
			this.GetValue = getValue;
		}
	}
}