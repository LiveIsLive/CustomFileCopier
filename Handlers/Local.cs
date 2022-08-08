using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Handlers
{
	public class Local : BaseHandler
	{
		public override string Name
		{
			get
			{
				if (System.Threading.Thread.CurrentThread.CurrentUICulture == ChineseCulture)
					return "本地目录";
				else return "Local Directory";
			}
		}

		public override bool Remote => false;

		public override void Execute()
		{
			foreach (Models.File sourceFile in this.Job.SourceFiles)
			{
				if (sourceFile.Result != Models.CopyResult.Success)
				{
					sourceFile.Result = Models.CopyResult.Copying;
					string targetFilePath = this.Job.GetTargetAbsoluteFilePath(sourceFile.Path);
					string targetDirectory = System.IO.Path.GetDirectoryName(targetFilePath);
					if (!System.IO.Directory.Exists(targetDirectory))
						try
						{
							System.IO.Directory.CreateDirectory(targetDirectory);
						}
						catch (System.Exception exception)
						{
							sourceFile.Result = Models.CopyResult.Failure;
							sourceFile.Error = exception.Message;
							//return exception.Message;
							continue;
						}
					try
					{
						System.IO.File.Copy(sourceFile.Path, targetFilePath, true);
					}
					catch (System.Exception exception)
					{
						sourceFile.Result = Models.CopyResult.Failure;
						sourceFile.Error = exception.Message;
						//return exception.Message;
						continue;
					}
				}
				sourceFile.Result = Models.CopyResult.Success;
				this.Job.Task.CopiedFileCount++;
				this.Job.Task.CopiedFileSize += sourceFile.FileInfo.Length;
			}
		}
	}
}