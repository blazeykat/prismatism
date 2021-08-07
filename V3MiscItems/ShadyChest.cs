using Gungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace katmod
{
    class ShadyChest : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Calamity Box";
            string resourcePath = "katmod/Resources/V3MiscItems/calamitybox";
            GameObject obj = new GameObject(itemName);
            ShadyChest item = obj.AddComponent<ShadyChest>();
            ItemBuilder.AddSpriteToObject(itemName, resourcePath, obj);
            string shortDesc = "Shady chest";
            string longDesc = "A powerful chest, which uses curse to unlock.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            item.PlaceItemInAmmonomiconAfterItemById(490);
            GameManager.Instance.RainbowRunForceExcludedIDs.Add(item.PickupObjectId);
            item.consumable = true;
        }

        public override bool CanBeUsed(PlayerController user)
        {
            return user.stats.GetStatValue(PlayerStats.StatType.Curse) >= 9;
        }

        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            try
            {
                Chest chest = Chest.Spawn(GameManager.Instance.RewardManager.Rainbow_Chest, user.sprite.WorldCenter + Vector2.down, user.sprite.WorldCenter.GetAbsoluteRoom(), true);
                chest.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUberPhantom");
                /*foreach (PickupObject item in chest.contents)
                {
                    StatModifier removethisplease = null;
                    if (item is PassiveItem)
                    {
                        if (((PassiveItem)item).passiveStatModifiers != null)
                        {
                            foreach (StatModifier modifywhatever in ((PassiveItem)item).passiveStatModifiers)
                            {
                                if (modifywhatever.statToBoost == PlayerStats.StatType.Curse)
                                {
                                    removethisplease = modifywhatever;
                                }
                            }
                        }
                    }
                    else if (item is PlayerItem)
                    {
                        if (((PlayerItem)item).passiveStatModifiers != null)
                        {
                            foreach (StatModifier modifywhatever in ((PlayerItem)item).passiveStatModifiers)
                            {
                                if (modifywhatever.statToBoost == PlayerStats.StatType.Curse)
                                {
                                    removethisplease = modifywhatever;
                                }
                            }
                        }
                    }
                    else if (item is Gun)
                    {
                        if (((Gun)item).passiveStatModifiers != null)
                        {
                            foreach (StatModifier modifywhatever in ((Gun)item).passiveStatModifiers)
                            {
                                if (modifywhatever.statToBoost == PlayerStats.StatType.Curse)
                                {
                                    removethisplease = modifywhatever;
                                }
                            }
                        }
                    }

                    if (removethisplease != null)
                    {
                        item.RemovePassiveStatModifier(removethisplease);
                    }
                }*/
            } catch (Exception lerror)
            {
                ETGModConsole.Log(lerror.ToString());
            }
        }
    }
}
