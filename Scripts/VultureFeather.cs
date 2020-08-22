using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{
    class VultureFeather : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Magic Vulture Feather";
            string resourceName = "katmod/Resources/vulturefeather.png";
            GameObject obj = new GameObject();
            VultureFeather item = obj.AddComponent<VultureFeather>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Scavenger";
            string longDesc = "Enemies have a chance to drop a random item.\n\nWhile vulture feathers are naturally gray, these ones are purple. It's not for a magic reason, they're just dyed that way.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.S;
            item.SetupUnlockOnFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_FORGE, true);
            item.PlaceItemInAmmonomiconAfterItemById(307);
        }

        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            if (enemy.specRigidbody != null && enemy.aiActor != null && base.Owner != null)
            {
                if (fatal)
                {
                    if (Utilities.UncoolRandom(.985f))
                    {
                        GameManager.Instance.RewardManager.SpawnTotallyRandomItem(enemy.specRigidbody.UnitCenter, ItemQuality.D, ItemQuality.S);
                    }
                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage += this.OnEnemyDamaged;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            if (Owner != null)
            {
                base.Owner.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            }
            base.OnDestroy();
        }

    }

}
