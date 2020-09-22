using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{
    class TrickOTreator : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Trick O' Treater";
            string resourceName = "katmod/Resources/Candies/jackolantern.png";
            GameObject obj = new GameObject();
            TrickOTreator item = obj.AddComponent<TrickOTreator>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Spirit of Spookiness";
            string longDesc = "Enemies have a chance to drop a random candies.\n\nHalloween is a fictional holiday celebrated at the end of October, in order to scare off spirits and get free candy.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.A;
            List<string> mandatoryConsoleIDs = new List<string>
            {
                "psm:trick_o'_treater",
                "monster_blood"
            };
            CustomSynergies.Add("Monster Candy", mandatoryConsoleIDs, null, true);
            item.SetupUnlockOnStat(TrackedStats.TIMES_CLEARED_CATACOMBS, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN, 14);
            item.PlaceItemInAmmonomiconAfterItemById(110);
        }

        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            bool flag2 = Owner.HasPickupID(313);
            if (fatal)
            {
                if (Utilities.BasicRandom(base.Owner, 0.94f, 100))
                {
                    if (flag2)
                    {
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(MonsterCandy.MonsterCandyID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, false, false);
                    } else {
                    System.Random rando = new System.Random();
                        switch (rando.Next(1, 7))
                        {
                            default:
                            case 1:
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(GreenCandy.GreenCandyID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, false, false);
                                break;
                            case 2:
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(BlueCandy.BlueCandyID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, false, false);
                                break;
                            case 3:
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(GoldenCandy.GoldenCandyID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, false, false);
                                break;
                            case 4:
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(KeyCandy.GreyCandyID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, false, false);
                                break;
                            case 5:
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(RedCandy.RedCandyID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, false, false);
                                break;
                            case 6:
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(HeartCandy.HeartCandyID).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, false, false);
                                break;
                        }
                    }   
                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage += OnEnemyDamaged;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage -= OnEnemyDamaged;
            return base.Drop(player);
        }

    }

}
