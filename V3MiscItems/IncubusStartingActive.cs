using System;
using System.Linq;
using Dungeonator;
using ItemAPI;
using System.Collections.Generic;
using UnityEngine;

namespace katmod
{
    class IncubusStartingActive : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Road Train";
            string resourceName = "katmod/Resources/V3MiscItems/jarfullofsouls";
            GameObject obj = new GameObject(itemName);
            IncubusStartingActive item = obj.gameObject.AddComponent<IncubusStartingActive>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Haunting";
            string longDesc = "Duplicate every living enemy as a ghost, which fights for you.\n\nScreams and wailing emanate from the jar. Best not to think about it.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(558);
            item.SetCooldownType(ItemBuilder.CooldownType.Damage, 500);
            item.consumable = false;

            /*Projectile projectile2 = (PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0];
            Projectile proj1 = UnityEngine.Object.Instantiate<Projectile>(projectile2);
            {
                proj1.baseData.damage = 25;
                proj1.SetProjectileSpriteRight("incubus_placeholder_001", 44, 22);
                proj1.shouldRotate = true;
                proj1.gameObject.SetActive(false);
                proj1.baseData.speed = 7;
                UnityEngine.Object.DontDestroyOnLoad(proj1);
                FakePrefab.MarkAsFakePrefab(proj1.gameObject);
            }
            demonProjectile = proj1;*/
        }

        private static Projectile demonProjectile;

        protected override void DoEffect(PlayerController user)
        {
            DoEffect(user);
        }
    }
}
