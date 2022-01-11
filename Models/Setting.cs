using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.SmartFileCopier.Models
{
	public class Setting
	{
		public string SelectedCultureName { get; set; }

		protected static readonly string SavePath = System.AppDomain.CurrentDomain.BaseDirectory + "Setting.json";

		private static Setting _Instance;
		public static Setting Instance
		{
			get
			{
				if (_Instance == null)
					if (System.IO.File.Exists(SavePath))
					{
						System.IO.StreamReader reader = new System.IO.StreamReader(SavePath);
						//_Instance = NetJSON.NetJSON.Deserialize<Setting>(reader);
						_Instance = new Newtonsoft.Json.JsonSerializer().Deserialize<Setting>(new Newtonsoft.Json.JsonTextReader(reader));
						reader.Close();
					}
					else _Instance = new Setting();
				return _Instance;
			}
		}

		public void Save()
		{
			System.IO.StreamWriter writer = new System.IO.StreamWriter(SavePath);
			//NetJSON.NetJSON.Serialize(this, writer);
			new Newtonsoft.Json.JsonSerializer().Serialize(writer, this);
			writer.Close();
		}
	}
}