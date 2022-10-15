using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using KMod;
using TUNING;

namespace ONIMod.IceBox
{
	// 此段代码来自 Psyko.OniUtils
	// this code frrom Psyko.OniUtils
	public static class Utils
	{
		public static void AddBuildingToTech(string tech, string buildID)
		{
			Db.Get().Techs.Get(tech).unlockedItemIDs.Add(buildID);
		}

		public static void AddPlan(HashedString category, string subcategory, string buildID, string addAfter = null)
		{
			string str = "Adding ";
			string str2 = " to category ";
			HashedString hashedString = category;
			Debug.Log(str + buildID + str2 + hashedString.ToString());
			foreach (PlanScreen.PlanInfo planInfo in BUILDINGS.PLANORDER)
			{
				if (planInfo.category == category)
				{
					Utils.AddPlanToCategory(planInfo, subcategory, buildID, addAfter);
					return;
				}
			}
			Debug.Log(string.Format("Unknown build menu category: ${0}", category));
		}

		private static void AddPlanToCategory(PlanScreen.PlanInfo menu, string subcategory, string buildID, string addAfter = null)
		{
			List<KeyValuePair<string, string>> buildingAndSubcategoryData = menu.buildingAndSubcategoryData;
			if (buildingAndSubcategoryData != null)
			{
				if (addAfter == null)
				{
					buildingAndSubcategoryData.Add(new KeyValuePair<string, string>(buildID, subcategory));
					return;
				}
				int num = buildingAndSubcategoryData.IndexOf(new KeyValuePair<string, string>(addAfter, subcategory));
				if (num == -1)
				{
					Debug.Log(string.Concat(new string[]
					{
						"Could not find building ",
						subcategory,
						"/",
						addAfter,
						" to add ",
						buildID,
						" after. Adding at the end !"
					}));
					buildingAndSubcategoryData.Add(new KeyValuePair<string, string>(buildID, subcategory));
					return;
				}
				buildingAndSubcategoryData.Insert(num + 1, new KeyValuePair<string, string>(buildID, subcategory));
			}
		}

		public static string GetModPath()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}

		public static void Translate(Type root)
		{
			Localization.RegisterForTranslation(root);
			Utils.LoadStrings();
			LocString.CreateLocStringKeys(root, null);
			Localization.GenerateStringsTemplate(root, Path.Combine(Manager.GetDirectory(), "strings_templates"));
		}

		private static void LoadStrings()
		{
			string modPath = Utils.GetModPath();
			string path = "translations";
			Localization.Locale locale = Localization.GetLocale();
			string text = Path.Combine(modPath, path, ((locale != null) ? locale.Code : null) + ".po");
			if (File.Exists(text))
			{
				Localization.OverloadStrings(Localization.LoadStringsFile(text, false));
			}
		}
	}
}
