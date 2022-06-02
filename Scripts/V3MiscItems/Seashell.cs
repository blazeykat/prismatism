using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class Seashell : PassiveItem
	{
		public static void Init()
		{
			string name = "Sea Shell";
			string resourcePath = "katmod/Resources/V2MiscItems/rusticer2.png";
			GameObject gameObject = new GameObject(name);
			Seashell item = gameObject.AddComponent<Seashell>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Those that she sells";
			string longDesc = "Actives activate automatically whilst in combat.";
			item.SetupItem(shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.EXCLUDED;
			item.RemovePickupFromLootTables();
			item.AddPassiveStatModifier(PlayerStats.StatType.Coolness, 7);
		}

        protected override void Update()
        {
            base.Update();
			if (base.Owner != null)
            {
				for (int i = 0; i < base.Owner.activeItems.Count; i++)
				{
					if (base.Owner.activeItems[i] && base.Owner.activeItems[i].CanBeUsed(base.Owner) && base.Owner.IsInCombat)
					{
                        base.Owner.activeItems[i].Use(base.Owner, out float IDunnoWhatThisDoes);
                    }
				}
            }
        }
    }
}
