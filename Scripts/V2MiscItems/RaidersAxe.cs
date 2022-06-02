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
                this.HellIsFull();
        }

        private void HellIsFull()
        {
            this.killsThisRun = Owner.KillsThisRun;
            bool flag = this.killsThisRun == this.lastKillsThisRun;
            if (!flag)
            {
                this.RemoveStat(PlayerStats.StatType.Damage);
                this.AddStat(PlayerStats.StatType.Damage, this.m_owner.KillsThisRun * 0.00055f, StatModifier.ModifyMethod.ADDITIVE);
                base.Owner.stats.RecalculateStats(base.Owner, true, false);
                this.lastKillsThisRun = this.killsThisRun;
            }
        }

        private int killsThisRun;

        private float lastKillsThisRun;
    }
}
