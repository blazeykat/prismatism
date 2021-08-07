using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{
    class BronzeKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Bronze Key";
            string resourceName = "katmod/Resources/V2MiscItems/yellowkey";
            GameObject obj = new GameObject();
            BronzeKey item = obj.AddComponent<BronzeKey>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "keybirth";
            string longDesc = "Enemies have a chance to be reborn as keys.\n\nThere's dried blood on the ridges.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.A;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 2f, StatModifier.ModifyMethod.ADDITIVE);
            item.PlaceItemInAmmonomiconAfterItemById(166);
            item.SetupUnlockOnFlag(GungeonFlags.BOSSKILLED_DOORMIMIC, true);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Kill Door Lord");
            item.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
            GameManager.Instance.RainbowRunForceExcludedIDs.Add(item.PickupObjectId);
        }

        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            if (!Owner.PlayerHasActiveSynergy("REANIMATE"))
            {
                if (fatal && enemy && enemy.aiActor && enemy.aiActor.EnemyGuid != "699cd24270af4cd183d671090d8323a1")
                {
                    if (Owner.CurrentRoom != null)
                    {
                        if (BoxOTools.BasicRandom(0.975f) && DoneKill)
                        {
                            try
                            {
                                AIActor aiactor = BoxOTools.SummonAtRandomPosition("699cd24270af4cd183d671090d8323a1", Owner);
                                if (base.Owner.PlayerHasActiveSynergy("keysight"))
                                {
                                    aiactor.IsHarmlessEnemy = true;
                                    aiactor.BecomeBlackPhantom();
                                }
                                if (!base.Owner.PlayerHasActiveSynergy("keysight"))
                                {
                                    aiactor.PreventBlackPhantom = true;
                                }
                                aiactor.HandleReinforcementFallIntoRoom(0f);
                                DoneKill = false;
                            } catch (Exception Error)
                            {
                                ETGModConsole.Log(Error.Message);
                            }
                        }
                    }
                }
            } else
            {
                if (BoxOTools.BasicRandom(0.8f) && enemy && enemy.aiActor && Owner.CurrentRoom != null && fatal)
                {
                    if (BoxOTools.BasicRandom(0.3f) || base.Owner.PlayerHasActiveSynergy("keysight"))
                    {
                        AIActor aiactor = BoxOTools.SummonAtRandomPosition("336190e29e8a4f75ab7486595b700d4a", Owner);
                        BoxOTools.AddPermanentCharm(aiactor);
                        aiactor.HandleReinforcementFallIntoRoom(0f);
                    } else
                    {
                        AIActor aiactor = BoxOTools.SummonAtRandomPosition("95ec774b5a75467a9ab05fa230c0c143", Owner);
                        BoxOTools.AddPermanentCharm(aiactor);
                        aiactor.HandleReinforcementFallIntoRoom(0f);
                    }
                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
            player.OnEnteredCombat += resetBool;
        }

        public void resetBool()
        {
            DoneKill = true;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
            player.OnEnteredCombat -= resetBool;
            return base.Drop(player);
        }

        static bool DoneKill = true;
    }

}
