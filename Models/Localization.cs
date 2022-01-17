using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.SmartFileCopier.Models
{
	public class Localization
	{
		public string Save { get; set; }
		public string SaveAs { get; set; }
		public string Open { get; set; }
		public string RecentFiles { get; set; }
		public string Language { get; set; }
		public string FileList { get; set; }
		public string AddJob { get; set; }
		public string NewJob { get; set; }

		public string Name { get; set; }
		public string SourceDirectory { get; set; }
		public string TargetDirectory { get; set; }

		public string AddCondition { get; set; }
		public string Property { get; set; }
		public string Operator { get; set; }
		public string LeftBracket { get; set; }
		public string RightBracket { get; set; }
		public string Connective { get; set; }
		public string Value { get; set; }

		public string Path { get; set; }
		public string Directory { get; set; }
		public string Result { get; set; }
		public string Error { get; set; }


		public string OpenFileDialog { get; set; }

		public System.Collections.Generic.Dictionary<string,string> Properties { get; set; }

		public System.Collections.Generic.Dictionary<string,string> Operators { get; set; }

		public System.Collections.Generic.Dictionary<LogicalConnective, string> LogicalConnective { get; set; }

		public System.Collections.Generic.Dictionary<CopyResult, string> CopyResult { get; set; }
	}
}