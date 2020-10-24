using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using InControl;

namespace katmod
{
    class KeyDoubler : PassiveItem
    {
        public static void Init()
        {
            string name = "Shady Key";
            string resource = "katmod/Resources/V3MiscItems/shadowkey";
            GameObject obj = new GameObject(name);
            KeyDoubler item = obj.AddComponent<KeyDoubler>();
            ItemBuilder.AddSpriteToObject(name, resource, obj);
            string shortd = "Spookey";
            string longd = "Doubles some keys you pick up";
            ItemBuilder.SetupItem(item, shortd, longd);
            item.quality = ItemQuality.B;
            itemator = item;
            new Hook(typeof(KeyBulletPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public), typeof(KeyDoubler).GetMethod("DoubleKeys"));
            item.PlaceItemInAmmonomiconAfterItemById(166);
        }

        public static void DoubleKeys(Action<KeyBulletPickup, PlayerController> acshon, KeyBulletPickup key, PlayerController player)
        {
            acshon(key, player);
            foreach (PassiveItem passives in player.passiveItems)
            {
                if (passives is KeyDoubler)
                {
                    player.carriedConsumables.KeyBullets += 1;
                    player.BloopItemAboveHead(itemator.sprite);

                }

            }
        }

        public static PickupObject itemator;

       /* protected override void Update()
        {
            base.Update();
            if (Owner.carriedConsumables.KeyBullets != keys && keys < Owner.carriedConsumables.KeyBullets)
            {
                keys = Owner.carriedConsumables.KeyBullets += 1;
            }
        }

        int keys;*/
    }
}
