using Dungeonator;
using ItemAPI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace katmod
{
    internal class Bravery : PassiveItem
    {
        public static void Init()
        {
            string name = "Medal of Bravery";
            string resourcePath = "katmod/Resources/V2MiscItems/medalofbravery.png";
            GameObject gameObject = new GameObject(name);
            Bravery uten = gameObject.AddComponent<Bravery>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Fearless Warrior.";
            string longDesc = "Increases damage for each enemy in the room.";
            uten.SetupItem(shortDesc, longDesc, "psm");
            uten.quality = PickupObject.ItemQuality.B;
            uten.PlaceItemInAmmonomiconAfterItemById(451);
        }

        protected override void Update()
        {
            base.Update();
            if (base.Owner && Owner.CurrentRoom != null)
            {
                this.EnemiesCheck();
            }
        }

        private void EnemiesCheck()
        {
            this.enemies = this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All).Count;
            bool flag = this.enemies == this.lastEnemies;
            if (!flag)
            {
                this.RemoveStat(PlayerStats.StatType.Damage);
                this.AddStat(PlayerStats.StatType.Damage, (Owner.HasPassiveItem(494) ? this.enemies * 0.1f : this.enemies * 0.05f), StatModifier.ModifyMethod.ADDITIVE);
                base.Owner.stats.RecalculateStats(base.Owner, true, false);
                this.lastEnemies = this.enemies;
            }
        }

        private int enemies;

        private float lastEnemies;
    }
}
