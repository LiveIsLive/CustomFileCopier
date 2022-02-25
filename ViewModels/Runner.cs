using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.SmartFileCopier.ViewModels
{
	public class Runner:Screen
	{
		public Models.Task Task { get; protected set; }

		public string Title { get; protected set; }

		public Runner(Models.Task task, string fileName)
		{
			this.Task = task;
			this.Title = $"{this.Localization.RunTask} - {fileName}";
		}

		public void Run()
		{
			System.Threading.Tasks.Task.Run(() => this.Task.Run(), this.CancellationTokenSource.Token);
		}

		public void Stop()
		{
			this.CancellationTokenSource.Cancel();
			//this.Task.Status = Models.TaskStatus.Standby;
			//this.TryClose();
		}
	}
}