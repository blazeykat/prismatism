using System;
using ItemAPI;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

namespace katmod
{
    class AbyssalShield : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Abyssal Crest";
            string resourceName = "katmod/Resources/V3MiscItems/abyssalcrest";
            GameObject obj = new GameObject(itemName);
            AbyssalShield item = obj.gameObject.AddComponent<AbyssalShield>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Flesh eater";
            string longDesc = "Molds health into metal.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(573);
            item.SetCooldownType(ItemBuilder.CooldownType.Timed, 1);
            item.consumable = false;
        }

        public override bool CanBeUsed(PlayerController user)
        {
            if (user && user.healthHaver && user.stats.GetStatValue(PlayerStats.StatType.Health) > 1)
            return base.CanBeUsed(user);

            return false;
        }

        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            if (user && user.healthHaver)
            {
                user.ownerlessStatModifiers.Add(lessHP);
                user.stats.RecalculateStats(user);
                user.healthHaver.Armor += 5;
                user.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_item_pickup") as GameObject, Vector3.zero);
            }
        }

        StatModifier lessHP = new StatModifier()
        {
            amount = -1,
            statToBoost = PlayerStats.StatType.Health
        };
    }
}
