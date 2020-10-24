using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class HeartyKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Hearty Key";
            string resourceName = "katmod/Resources/V3MiscItems/heartykey";
            GameObject obj = new GameObject();
            HeartyKey item = obj.AddComponent<HeartyKey>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Healthey";
            string longDesc = "Opening chests gives you hearts.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.C;
            item.PlaceItemInAmmonomiconAfterItemById(166);
        }

        public override void Pickup(PlayerController player)
        {
            ETGMod.Chest.OnPostOpen += Chnest;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            ETGMod.Chest.OnPostOpen -= Chnest;
            return base.Drop(player);
        }

        public void Chnest(Chest chest, PlayerController player)
        {
            if (player.healthHaver)
            {
                AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", base.gameObject);
                player.healthHaver.ApplyHealing(1f);
                player.BloopItemAboveHead(this.sprite);
            }
        }
    }
}
