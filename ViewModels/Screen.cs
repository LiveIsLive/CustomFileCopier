﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.SmartFileCopier.ViewModels
{
	public class Screen : Caliburn.Micro.Screen
	{
		protected static readonly string LocalizationDirectory = System.AppDomain.CurrentDomain.BaseDirectory + @"Localization\";

		private Models.Setting _Setting;
		public Models.Setting Setting
		{
			get
			{
				if (this._Setting == null)
				{
					this._Setting = Models.Setting.Instance;
					if(string.IsNullOrWhiteSpace(this.Setting.SelectedCultureName))
					{
						string baseDirectory = LocalizationDirectory;
						string filePath = baseDirectory + System.Globalization.CultureInfo.CurrentUICulture.Name + ".json";
						if (System.IO.File.Exists(filePath))
							this._Setting.SelectedCultureName = System.Globalization.CultureInfo.CurrentUICulture.Name;
						else
						{
							filePath = baseDirectory + System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName + ".json";
							if (System.IO.File.Exists(filePath))
								this._Setting.SelectedCultureName = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
							else this._Setting.SelectedCultureName = "en";
						}
					}
					else
					{
						System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo(this.Setting.SelectedCultureName);
						System.Threading.Thread.CurrentThread.CurrentCulture= culture;
						System.Threading.Thread.CurrentThread.CurrentUICulture= culture;
					}
				}
				return this._Setting;
			}
		}

		private Models.Localization _Localization;
		public Models.Localization Localization
		{
			get
			{
				if (this._Localization == null)
					//if (Caliburn.Micro.Execute.InDesignMode)
					//{
					//	System.IO.StreamReader reader = new System.IO.StreamReader(System.Windows.Application.GetResourceStream(new Uri("/ColdShineSoft.SmartFileCopier.Views;component/Localization/zh-CN.json", System.UriKind.Relative)).Stream);
					//	this._Localization = NetJSON.NetJSON.Deserialize<Models.Localization>(reader);
					//	reader.Close();
					//}
					//else
					{
						System.IO.StreamReader reader = new System.IO.StreamReader(LocalizationDirectory + this.Setting.SelectedCultureName + ".json");
						//this._Localization = NetJSON.NetJSON.Deserialize<Models.Localization>(reader);
						this._Localization = new Newtonsoft.Json.JsonSerializer().Deserialize<Models.Localization>(new Newtonsoft.Json.JsonTextReader(reader));
						reader.Close();
					}
				return this._Localization;
			}
			set
			{
				this._Localization = value;
				this.NotifyOfPropertyChange(() => this.Localization);
			}
		}
	}
}