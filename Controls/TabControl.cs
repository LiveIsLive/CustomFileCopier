using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ColdShineSoft.SmartFileCopier.Controls
{
	public class TabControl:HandyControl.Controls.TabControl
	{
		public TabControl()
		{
			this.Style = new System.Windows.Style(typeof(HandyControl.Controls.TabControl), (System.Windows.Style)System.Windows.Application.Current.TryFindResource(typeof(HandyControl.Controls.TabControl)));
		}

		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			this.Focus();
			base.OnSelectionChanged(e);
		}
	}
}