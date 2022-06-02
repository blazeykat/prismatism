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
            string longd = "Doubles some keys you pick up.\n\nAn alliance with the shadows could give you what you want, but it's a deadly mistake.";
            ItemBuilder.SetupItem(item, shortd, longd);
            item.quality = ItemQuality.A;
            itemator = item;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            item.AddPassiveStatModifier(PlayerStats.StatType.Curse, 2);
            new Hook(typeof(KeyBulletPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public), typeof(KeyDoubler).GetMethod("DoubleKeys"));
            item.PlaceItemInAmmonomiconAfterItemById(166);
            GameManager.Instance.RainbowRunForceExcludedIDs.Add(item.PickupObjectId);
        }

        public static void DoubleKeys(Action<KeyBulletPickup, PlayerController> acshon, KeyBulletPickup key, PlayerController player)
        {
            acshon(key, player);
            foreach (PassiveItem passives in player.passiveItems)
            {
                if (passives is KeyDoubler && BoxOTools.BasicRandom(0.5f))
                {
                    player.carriedConsumables.KeyBullets += 1;
                    player.BloopItemAboveHead(itemator.sprite);


                    if (player.PlayerHasActiveSynergy("Twice the Pride"))
                    {
                        if (player.characterIdentity != PlayableCharacters.Robot) { player.healthHaver.ApplyHealing(1f); } else { player.healthHaver.Armor += 1; }
                        AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", passives.gameObject);
                        player.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero);
                    }
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
