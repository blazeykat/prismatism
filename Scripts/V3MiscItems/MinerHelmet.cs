using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{
    class MinerHelmet : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Miner's Helmet";
            string resourceName = "katmod/Resources/V3MiscItems/minerhelmet";
            GameObject obj = new GameObject();
            MinerHelmet item = obj.AddComponent<MinerHelmet>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Caverns";
            string longDesc = "Get a random gun every floor.\n\nUsed by skeletal miners in a far off land.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.B;
            item.SetupUnlockOnFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_MINES, true);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Master the gunpowder mines' boss");
            item.PlaceItemInAmmonomiconAfterItemById(308);
        }


        private void AWholeNewWorld(PlayerController player)
        {
            ThanksGame = !ThanksGame;
            if (ThanksGame && player)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetRandomGun().gameObject, player);
            }
        }

        public override void Pickup(PlayerController player)
        {
            player.OnNewFloorLoaded += AWholeNewWorld;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnNewFloorLoaded -= AWholeNewWorld;
            return base.Drop(player);
        }

        private bool ThanksGame = true;
    }

}
