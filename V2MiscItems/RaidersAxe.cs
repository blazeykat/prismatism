using Dungeonator;
using ItemAPI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace katmod
{
    internal class RaidersAxe : PassiveItem
    {
        public static void Init()
        {
            string name = "Raiders Axe";
            string resourcePath = "katmod/Resources/V2MiscItems/raidersaxe.png";
            GameObject gameObject = new GameObject(name);
            RaidersAxe item = gameObject.AddComponent<RaidersAxe>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Viking Waraxe";
            string longDesc = "Increases damage by 0.00055 for each enemy killed.\n\nNothing lasts for ever after all.\nI'll love you, till the ocean takes us all.";
            item.SetupItem(shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.A;
            item.PlaceItemInAmmonomiconAfterItemById(165);
        }

        protected override void Update()
        {
            base.Update();
            if (Owner)
                this.BlankAmount();
        }

        private void BlankAmount()
        {
            this.armor = this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All).Count;
            bool flag = this.armor == this.lastArmor;
            if (!flag)
            {
                this.RemoveStat(PlayerStats.StatType.Damage);
                this.AddStat(PlayerStats.StatType.Damage, this.m_owner.KillsThisRun * 0.00055f, StatModifier.ModifyMethod.ADDITIVE);
                base.Owner.stats.RecalculateStats(base.Owner, true, false);
                this.lastArmor = this.armor;
            }
        }

        private int armor;

        private float lastArmor = 0f;
    }
}
