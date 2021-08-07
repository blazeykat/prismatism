using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{
    class Depthmeter : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Depthometer";
            string resourceName = "katmod/Resources/V3MiscItems/depthometer";
            GameObject obj = new GameObject();
            Depthmeter item = obj.AddComponent<Depthmeter>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Underground";
            string longDesc = "Get a passive delivery at the first room every floor.\n\nCarried by the rare nymphs of the underground.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.S;
            item.SetupUnlockOnFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_MINES, true);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Master the gunpowder mines' boss");
            item.PlaceItemInAmmonomiconAfterItemById(308);
        }

        private void AWholeNewWorld(PlayerController player)
        {
            ThanksGame = !ThanksGame;
            if (ThanksGame && player)
            {
                LootEngine.GivePrefabToPlayer(LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.B, GameManager.Instance.RewardManager.ItemsLootTable, true).gameObject, player);
            }
        }

        public override void Pickup(PlayerController player)
        {
            player.OnNewFloorLoaded += AWholeNewWorld;
            player.OnEnteredCombat += MineSynergy;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnNewFloorLoaded -= AWholeNewWorld;
            player.OnEnteredCombat -= MineSynergy;
            return base.Drop(player);
        }

        private bool ThanksGame = true;

        public void MineSynergy()
        {
            if (Owner.PlayerHasActiveSynergy("Into the Depths"))
            {
                if (Owner.CurrentRoom != null)
                {
                    AIActor actor = BoxOTools.SummonAtRandomPosition("3cadf10c489b461f9fb8814abc1a09c1", Owner);
                    actor.AddPermanentCharm();
                }
            }
        }
    }
}
