using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Models
{
	public abstract class FileFilter
	{
		public abstract System.Collections.Generic.IEnumerable<System.IO.FileInfo> GetFiles(string sourceDirectoryPath);
	}

	public partial class FileFilterTemplate
	{
		public Models.Job Job;
	}

	public class FilesAndDirectoriesFileFilter : FileFilter
	{
		public readonly System.Collections.Generic.IEnumerable<string> FilePaths;

		public readonly System.Collections.Generic.IEnumerable<string> DirectoryPaths;

		public FilesAndDirectoriesFileFilter(System.Collections.Generic.IEnumerable<string> filePaths, System.Collections.Generic.IEnumerable<string> directoryPaths)
		{
			FilePaths = filePaths;
			DirectoryPaths = directoryPaths;
		}

		public override IEnumerable<FileInfo> GetFiles(string sourceDirectoryPath)
		{
			foreach(string path in this.FilePaths)
				yield return new FileInfo(path);
			foreach(string directoryPath in this.DirectoryPaths)
				foreach(string filePath in System.IO.Directory.GetFiles(directoryPath,"*",System.IO.SearchOption.AllDirectories))
					yield return new FileInfo(filePath);
		}
	}
}