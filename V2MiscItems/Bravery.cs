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
            uten.sprite.IsPerpendicular = true;
            uten.PlaceItemInAmmonomiconAfterItemById(451);
        }

        protected override void Update()
        {
            base.Update();
            if (base.Owner != null)
            {
                this.BlankAmount();
            }
        }

        private void BlankAmount()
        {
            this.armor = this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All).Count;
            bool flag = this.armor == this.lastArmor;
            if (!flag)
            {
                this.RemoveStat(PlayerStats.StatType.Damage);
                this.AddStat(PlayerStats.StatType.Damage, (Owner.HasPassiveItem(494) ? this.armor * 0.1f : this.armor * 0.05f), StatModifier.ModifyMethod.ADDITIVE);
                base.Owner.stats.RecalculateStats(base.Owner, true, false);
                this.lastArmor = this.armor;
            }
        }

        private int armor;

        private float lastArmor = 0f;
    }
}
