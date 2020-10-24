using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class ItemIHaventThoughOfANameForYet : PassiveItem
	{
		public static void Init()
		{
			string name = "wagabagaboo";
			string resourcePath = "katmod/Resources/fishsnack.png";
			GameObject gameObject = new GameObject(name);
			ItemIHaventThoughOfANameForYet item = gameObject.AddComponent<ItemIHaventThoughOfANameForYet>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "stale";
			string longDesc = "i ate a chip and it was all stale and gross";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = ItemQuality.EXCLUDED;
		}

        public override void Pickup(PlayerController player)
        {
            player.OnItemPurchased += Player_OnItemPurchased;
            base.Pickup(player);
        }

        private void Player_OnItemPurchased(PlayerController arg1, ShopItemController arg2)
        {
            if (arg2.item is Gun)
            {
                StartCoroutine(GarbageWorkaround(arg2.item.PickupObjectId, arg1));
            }
        }

		public class MoreDamage : GunBehaviour
        {

            public override void PostProcessProjectile(Projectile projectile)
            {
                base.PostProcessProjectile(projectile);
                projectile.baseData.damage *= 2;
            }
        }

        IEnumerator GarbageWorkaround(int id, PlayerController player)
        {
            yield return new WaitForSeconds(0.1f);
            foreach (Gun gun in player.inventory.AllGuns)
            {
                if (gun.PickupObjectId == id)
                {
                    gun.gameObject.AddComponent<MoreDamage>();
                }
            }
        }
    }
}
