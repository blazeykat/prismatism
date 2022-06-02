using ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using UnityEngine;

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
            string longd = "Doubles some blanks you pick up\n\nShadows, in the form of a blank. Used to symbolise a contract with the void.";
            ItemBuilder.SetupItem(item, shortd, longd);
            item.quality = ItemQuality.C;
            itemator = item;
            item.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);
            item.PlaceItemInAmmonomiconAfterItemById(499);
            new Hook(typeof(SilencerItem).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public), typeof(BlankDoubler).GetMethod("DoubleKeys"));
        }

        public static void DoubleKeys(Action<SilencerItem, PlayerController> acshon, SilencerItem key, PlayerController player)
        {
            acshon(key, player);
            foreach (PassiveItem passives in player.passiveItems)
            {
                if (passives is BlankDoubler && BoxOTools.BasicRandom(0.25f))
                {
                    player.Blanks += 1;
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
