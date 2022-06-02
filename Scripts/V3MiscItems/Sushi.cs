using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class Sushi : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Sushi";
            string resourceName = "katmod/Resources/V3MiscItems/sushi";
            GameObject obj = new GameObject();
            Sushi item = obj.AddComponent<Sushi>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Rapidfire";
            string longDesc = "Infinite ammo for 6 seconds upon entering combat";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.S;
            item.PlaceItemInAmmonomiconAfterItemById(411);
        }

        public override void Pickup(PlayerController player)
        {
            player.OnEnteredCombat += OnRoomStart;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnEnteredCombat -= OnRoomStart;
            return base.Drop(player);
        }

        private void OnRoomStart()
        {
            Owner.InfiniteAmmo.SetOverride("GunWarrant", true);
            //Owner.OnlyFinalProjectiles.SetOverride("GunWarrant", true, null);
            StartCoroutine(InfiniteAmmo(Owner));
        }

        public IEnumerator InfiniteAmmo(PlayerController player)
        {
            yield return new WaitForSeconds(player.HasMTGConsoleID("psm:cat_snack") ? 8 : 6);
            Owner.InfiniteAmmo.RemoveOverride("GunWarrant");
            //Owner.OnlyFinalProjectiles.RemoveOverride("GunWarrant");
            yield break;
        }
    }
}
