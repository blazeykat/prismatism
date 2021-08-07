using ItemAPI;
using UnityEngine;

namespace katmod
{
    class ParasiticFungus : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Parasitic Fungus";
            string resourceName = "katmod/Resources/V3MiscItems/parasiticmushroom";
            GameObject obj = new GameObject();
            ParasiticFungus item = obj.AddComponent<ParasiticFungus>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Mushy Buddy";
            string longDesc = "Increases damage for each heart lost, start each floor with half a heart.\n\nA Fungun, in the earliest stages of growth.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.B;
        }

        private float LastHPMissing;

        private float LastArmorMissing;

        private float MostArmorGotThisRun;

        protected override void Update()  
        {
            base.Update();
            if (Owner && Owner.healthHaver)
            {
                if (Owner.characterIdentity != PlayableCharacters.Robot)
                {
                    float HPMissing = Owner.healthHaver.GetMaxHealth() - Owner.healthHaver.GetCurrentHealth();
                    if (HPMissing != LastHPMissing)
                    {
                        LastHPMissing = HPMissing;
                        CalculateMissingHP(LastHPMissing);
                    }
                }
                else
                {
                    float ArmorGot = Owner.healthHaver.Armor;
                    if (ArmorGot > MostArmorGotThisRun)
                    {
                        MostArmorGotThisRun = ArmorGot;
                    }
                    float ArmorMissing = MostArmorGotThisRun - ArmorGot;
                    if (ArmorMissing != LastArmorMissing)
                    {
                        LastArmorMissing = ArmorMissing;
                        CalculateMissingHP(LastArmorMissing / 2);
                    }
                }
            }
        }

        public void CalculateMissingHP(float HPMissing)
        {
            this.RemoveStat(PlayerStats.StatType.Damage);
            this.AddStat(PlayerStats.StatType.Damage, 0.1f * HPMissing);
            Owner.stats.RecalculateStats(Owner);
        }

        public override void Pickup(PlayerController player)
        {
            player.OnNewFloorLoaded += FloorLoaded;
            MostArmorGotThisRun = 6;
            base.Pickup(player);
        }

        private void FloorLoaded(PlayerController obj)
        {
            if (obj && obj.healthHaver)
            { 
                if (obj.characterIdentity != PlayableCharacters.Robot)
                {
                    obj.healthHaver.ForceSetCurrentHealth(0.5f);
                } else
                {
                    obj.healthHaver.Armor = 1;
                }
            }
        }

        /*private void ModifyHealing(HealthHaver arg1, HealthHaver.ModifyHealingEventArgs arg2)
        {
            if (arg2 != EventArgs.Empty)
            {
                arg2.ModifiedHealing *= 2;
            }
        }*/

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnNewFloorLoaded -= FloorLoaded;
            return base.Drop(player);
        }
    }
}
