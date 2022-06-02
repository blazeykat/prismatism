using ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace katmod
{
    class AmmoEnhancer : PassiveItem
    {
        public static void Init()
        {
            string name = "Shade Ammo";
            string resource = "katmod/Resources/V3MiscItems/shadeammo";
            GameObject obj = new GameObject(name);
            AmmoEnhancer item = obj.AddComponent<AmmoEnhancer>();
            ItemBuilder.AddSpriteToObject(name, resource, obj);
            string shortd = "More bullets!!1!11!";
            string longd = "Gives a random gun full ammo upon picking up ammo.\n\nAn ammo box full of dark, mysterious liquid.";
            ItemBuilder.SetupItem(item, shortd, longd);
            item.quality = ItemQuality.A;
            itemator = item;
            new Hook(typeof(AmmoPickup).GetMethod("Pickup", BindingFlags.Instance | BindingFlags.Public), typeof(AmmoEnhancer).GetMethod("DoubleKeys"));
            item.PlaceItemInAmmonomiconAfterItemById(116);
        }

        public static void DoubleKeys(Action<AmmoPickup, PlayerController> acshon, AmmoPickup key, PlayerController player)
        {
            acshon(key, player);
            foreach (PassiveItem passives in player.passiveItems)
            {
                if (passives is AmmoEnhancer)
                {
                    bool VibeCheck = false;
                    foreach (Gun gunk in player.inventory.AllGuns)
                    {
                        if (gunk.ammo != gunk.AdjustedMaxAmmo && !(gunk.InfiniteAmmo) && gunk.CanGainAmmo)
                        {
                            VibeCheck = true;
                        }
                    }
                    if (VibeCheck)
                    {
                        int randomGun;
                        do
                        {
                            randomGun = UnityEngine.Random.Range(0, player.inventory.AllGuns.Count);
                        } while (player.inventory.AllGuns[randomGun].ammo == player.inventory.AllGuns[randomGun].AdjustedMaxAmmo && (player.inventory.AllGuns[randomGun].InfiniteAmmo) && player.inventory.AllGuns[randomGun].CanGainAmmo);
                        player.inventory.AllGuns[randomGun].GainAmmo(player.inventory.AllGuns[randomGun].AdjustedMaxAmmo - player.inventory.AllGuns[randomGun].ammo);
                        player.BloopItemAboveHead(itemator.sprite);
                    }
                }
            }
        }

        public static PickupObject itemator;
    }
}
