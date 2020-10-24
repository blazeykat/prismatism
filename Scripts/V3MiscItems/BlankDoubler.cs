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
    class BlankDoubler : PassiveItem
    {
        public static void Init()
        {
            string name = "Shady Blank";
            string resource = "katmod/Resources/V3MiscItems/shadyblank";
            GameObject obj = new GameObject(name);
            BlankDoubler item = obj.AddComponent<BlankDoubler>();
            ItemBuilder.AddSpriteToObject(name, resource, obj);
            string shortd = "Null, void and blank";
            string longd = "Doubles some blanks you pick up";
            ItemBuilder.SetupItem(item, shortd, longd);
            item.quality = ItemQuality.C;
            itemator = item;
            item.PlaceItemInAmmonomiconAfterItemById(499);
            new Hook(typeof(SilencerItem).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public), typeof(BlankDoubler).GetMethod("DoubleKeys"));
        }

        public static void DoubleKeys(Action<SilencerItem, PlayerController> acshon, SilencerItem key, PlayerController player)
        {
            acshon(key, player);
            foreach (PassiveItem passives in player.passiveItems)
            {
                if (passives is BlankDoubler)
                {
                    player.Blanks += 1;
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
