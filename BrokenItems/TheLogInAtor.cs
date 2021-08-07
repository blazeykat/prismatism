using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class TheLogInAtor : PlayerItem
	{
		public static void Init()
		{
			string name = "oggly boggly";
			string resourcePath = "katmod/Resources/V3MiscItems/batterybruv";
			GameObject gameObject = new GameObject(name);
			TheLogInAtor item = gameObject.AddComponent<TheLogInAtor>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "bzzzzzzzzzzzzzzz";
			string longDesc = "Halves your heart containers, converts them to shields. Gives 6 shields if you're already fully charged.\n\nUsed to charge large spaceships.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.C;
			item.PlaceItemInAmmonomiconAfterItemById(485);
			item.consumable = true;
		}

		protected override void DoEffect(PlayerController user)
		{
			GoopDefinition def = EnemyDatabase.GetOrLoadByGuid("98ca70157c364750a60f5e0084f9d3e2").bulletBank.GetComponent<GoopDoer>().goopDefinition;
			ETGModConsole.Log(def.AppliesCharm.ToString());
			ETGModConsole.Log(def.AppliesCheese.ToString());
			ETGModConsole.Log(def.AppliesDamageOverTime.ToString());
			ETGModConsole.Log(def.AppliesSpeedModifier.ToString());
			ETGModConsole.Log(def.AppliesSpeedModifierContinuously.ToString());
		}
	}
}
