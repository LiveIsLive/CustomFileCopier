using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Handlers
{
	internal class Ftp : BaseHandler
	{
		public override string Name
		{
			get
			{
				return "FTP";
			}
		}

		public override bool Remote => true;

		public override void Execute()
		{
			throw new NotImplementedException();
		}
	}
}
