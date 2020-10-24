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
    class UnluckyKey : PassiveItem
    {
        public static void Init()
        {
            string name = "Unlucky Key";
            string resource = "katmod/Resources/V3MiscItems/shadowkey";
            GameObject obj = new GameObject(name);
            UnluckyKey item = obj.AddComponent<UnluckyKey>();
            ItemBuilder.AddSpriteToObject(name, resource, obj);
            string shortd = "Failure";
            string longd = "Keys are now scared of you.\n\nbecause youre scary";
            ItemBuilder.SetupItem(item, shortd, longd);
            item.quality = ItemQuality.SPECIAL;
            itemator = item;
            new Hook(typeof(KeyBulletPickup).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic), typeof(UnluckyKey).GetMethod("DoubleKeys"));
        }

        public static void DoubleKeys(Action<KeyBulletPickup> acshon, KeyBulletPickup key)
        {
            acshon(key);
            foreach (PassiveItem passives in GameManager.Instance.GetActivePlayerClosestToPoint(key.specRigidbody.UnitCenter, true).passiveItems)
            {
                if (passives is UnluckyKey)
                {
                    key.specRigidbody.Velocity = BraveMathCollege.DegreesToVector(Vector2.Angle(key.specRigidbody.UnitCenter, GameManager.Instance.GetActivePlayerClosestToPoint(key.specRigidbody.UnitCenter, true).specRigidbody.UnitCenter) + 180);
                    key.specRigidbody.CollideWithTileMap = true;
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
