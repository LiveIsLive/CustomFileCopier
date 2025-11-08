using ColdShineSoft.CustomFileCopier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Handlers
{
	public class Local : Models.ResultHandler
	{
		public override string Name
		{
			get
			{
				if (System.Threading.Thread.CurrentThread.CurrentUICulture.LCID == ChineseCulture?.LCID)
					return "本地目录";
				else return "Local Directory";
			}
		}

		public override bool Remote => false;

		public override string CheckTargetDirectoryValid(Job job)
		{
			try
			{
				if (System.IO.Directory.Exists(job.TargetDirectoryPath))
					return null;
				System.IO.Directory.CreateDirectory(job.TargetDirectoryPath);
				return null;
			}
			catch (System.Exception exception)
			{
				return exception.Message;
			}
		}

		public override System.Threading.Tasks.Task<bool> TargetDirectoryEmpty(Job job)
		{
			return System.Threading.Tasks.Task<bool>.Run(() => System.IO.Directory.EnumerateFileSystemEntries(job.TargetDirectoryPath).FirstOrDefault() == null);
		}

		public override async System.Threading.Tasks.Task Execute(Models.Job job)
		{
			foreach (Models.File sourceFile in job.SourceFiles)
			{
				//if (sourceFile.Result != Models.CopyResult.Success)
				//{
				sourceFile.Result = Models.CopyResult.Copying;
				string targetFilePath = job.GetTargetAbsoluteFilePath(sourceFile.Path);
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
					System.IO.FileStream sourceStream = new System.IO.FileStream(sourceFile.Path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
					System.IO.FileStream targetStream = new System.IO.FileStream(targetFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
					await sourceStream.CopyToAsync(targetStream);
					try
					{
						sourceStream.Close();
					}
					catch { }
					targetStream.Close();
				}
				catch (System.Exception exception)
				{
					sourceFile.Result = Models.CopyResult.Failure;
					sourceFile.Error = exception?.InnerException?.Message ?? exception.Message;
					//return exception.Message;
					continue;
				}
				//}
				sourceFile.Result = Models.CopyResult.Success;
				job.Task.CopiedFileCount++;
				job.Task.CopiedFileSize += sourceFile.FileInfo.Length;
			}
		}
	}
}