using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{
    class YellowKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Bronze Key";
            string resourceName = "katmod/Resources/V2MiscItems/yellowkey";
            GameObject obj = new GameObject();
            YellowKey item = obj.AddComponent<YellowKey>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "keybirth";
            string longDesc = "Enemies have a chance to be reborn as keys.\n\nThere's dried blood on the ridges.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.A;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 2f, StatModifier.ModifyMethod.ADDITIVE);
            List<string> mandatoryConsoleIDs = new List<string>
            {
                "psm:bronze_key",
                "yellow_chamber"
            };
            CustomSynergies.Add("keysight", mandatoryConsoleIDs, null, true);
            item.PlaceItemInAmmonomiconAfterItemById(166);
            item.SetupUnlockOnFlag(GungeonFlags.BOSSKILLED_DOORMIMIC, true);
            item.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
        }

        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            if (enemy.aiActor.EnemyGuid != "699cd24270af4cd183d671090d8323a1")
            {
                if (fatal)
                {
                    IntVector2? intVector = new IntVector2?(base.Owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1));
                    var BulletKin = EnemyDatabase.GetOrLoadByGuid("699cd24270af4cd183d671090d8323a1");
                    if (intVector != null && BulletKin != null)
                    {
                        if (Utilities.UncoolRandom(0.985f) && DoneKill)
                        {
                            try
                            {
                                AIActor aiactor = AIActor.Spawn(BulletKin.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Awaken, true);
                                if (base.Owner.HasPickupID(570))
                                {
                                    aiactor.IsHarmlessEnemy = true;
                                    aiactor.BecomeBlackPhantom();
                                }
                                if (aiactor.IsBlackPhantom && !base.Owner.HasPickupID(570))
                                {
                                    aiactor.UnbecomeBlackPhantom();
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

        bool DoneKill = true;
    }

}
