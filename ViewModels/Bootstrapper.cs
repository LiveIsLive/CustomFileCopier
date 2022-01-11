using System.Linq;

using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace ColdShineSoft.SmartFileCopier.ViewModels
{
	public class Bootstrapper: Caliburn.Micro.BootstrapperBase
	{
		public Bootstrapper()
		{
			this.Initialize();
		}

		protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
		{
			var config = new Caliburn.Micro.TypeMappingConfiguration
			{
				UseNameSuffixesInMappings = false,
				DefaultSubNamespaceForViewModels = typeof(Main).Namespace,
				DefaultSubNamespaceForViews = "ColdShineSoft.SmartFileCopier.Views"
			};

			Caliburn.Micro.ViewLocator.ConfigureTypeMappings(config);
			Caliburn.Micro.ViewModelLocator.ConfigureTypeMappings(config);
			base.OnStartup(sender, e);

			this.DisplayRootViewFor<Main>();
		}

		//private System.ComponentModel.Composition.Hosting.CompositionContainer container;
		//protected override void Configure()
		//{
		//	container = new System.ComponentModel.Composition.Hosting.CompositionContainer(new System.ComponentModel.Composition.Hosting.AggregateCatalog(Caliburn.Micro.AssemblySource.Instance.Select(x => new System.ComponentModel.Composition.Hosting.AssemblyCatalog(x)).OfType<System.ComponentModel.Composition.Primitives.ComposablePartCatalog>()));

		//	System.ComponentModel.Composition.Hosting.CompositionBatch batch = new System.ComponentModel.Composition.Hosting.CompositionBatch();

		//	batch.AddExportedValue<Caliburn.Micro.IWindowManager>(new Caliburn.Micro.WindowManager());
		//	batch.AddExportedValue<Caliburn.Micro.IEventAggregator>(new Caliburn.Micro.EventAggregator());
		//	batch.AddExportedValue(container);

		//	container.Compose(batch);
		//}


		//protected override object GetInstance(System.Type serviceType, string key)
		//{
		//	string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
		//	var exports = container.GetExportedValues<object>(contract);

		//	if (exports.Count() > 0)
		//	{
		//		return exports.First();
		//	}

		//	throw new System.Exception(string.Format("Could not locate any instances of contract {0}.", contract));
		//}

		protected override System.Collections.Generic.IEnumerable<System.Reflection.Assembly> SelectAssemblies()
		{
			return new[] { System.Reflection.Assembly.GetExecutingAssembly(),System.Reflection.Assembly.GetEntryAssembly() };
		}
	}
}