using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{
    class TrickOTreater : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Trick O' Treater";
            string resourceName = "katmod/Resources/Candies/jackolantern.png";
            GameObject obj = new GameObject();
            TrickOTreater item = obj.AddComponent<TrickOTreater>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Spirit of Spookiness";
            string longDesc = "Enemies have a chance to drop a random candies.\n\nHalloween is a fictional holiday celebrated at the end of October, in order to scare off spirits and get free candy.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.A;
            item.SetupUnlockOnStat(TrackedStats.TIMES_CLEARED_CATACOMBS, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN, 15);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Clear the Hollow 15 times");
            item.PlaceItemInAmmonomiconAfterItemById(110);
        }

        private void OnEnemyDamaged(PlayerController player)
        {
            if (player && player.CurrentRoom != null)
            {
                if (BoxOTools.BasicRandom(0.9f) && !player.CurrentRoom.PlayerHasTakenDamageInThisRoom)
                {
                    PickupObject candy = Gungeon.Game.Items["psm:candy"];
                    IntVector2 candyLocation = player.CurrentRoom.GetBestRewardLocation(IntVector2.One);
                    LootEngine.SpawnItem(candy.gameObject, candyLocation.ToVector3(), Vector2.zero, 1f, true, true, false);
                    /*if (Owner.PlayerHasActiveSynergy("Monster Candy"))
                    {
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(MonsterCandy.MonsterCandyID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, true, true, true);
                    } else {
                    System.Random rando = new System.Random();
                        switch (rando.Next(1, 7))
                        {
                            default:
                            case 1:
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(GreenCandy.GreenCandyID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, true, true, true);
                                break;
                            case 2:
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(BlueCandy.BlueCandyID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, true, true, true);
                                break;
                            case 3:
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(GoldenCandy.GoldenCandyID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, true, true, true);
                                break;
                            case 4:
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(KeyCandy.GreyCandyID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, true, true, true);
                                break;
                            case 5:
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(RedCandy.RedCandyID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, true, true, true);
                                break;
                            case 6:
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(HeartCandy.HeartCandyID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, true, true, true);
                                break;
                        }
                    }*/   
                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnRoomClearEvent += OnEnemyDamaged;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnRoomClearEvent -= OnEnemyDamaged;
            return base.Drop(player);
        }

    }

}
