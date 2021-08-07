using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class DormantStabiliser : PlayerItem
    {
        public static void Init()
        {
            string name = "Dormant Stabiliser";
            string resourcePath = "katmod/Resources/V3MiscItems/dormantStabiliser.png";
            GameObject gameObject = new GameObject(name);
            DormantStabiliser item = gameObject.AddComponent<DormantStabiliser>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Stabilises";
            string longDesc = "Upon use, turns into a passive. The passive makes it so your health cannot go above how much HP you had when you used it. Increases damage the less HP you have. The passive can be dropped.\n\nAn ancient failsafe, used to prevent dangerous rifts from forming.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.consumable = true;
            item.quality = ItemQuality.B;

            string name2 = "Stabiliser";
            string resourcePath2 = "katmod/Resources/V3MiscItems/stabiliserBunny.png";
            GameObject gameObject2 = new GameObject(name2);
            ActiveStabiliser item2 = gameObject2.AddComponent<ActiveStabiliser>();
            ItemBuilder.AddSpriteToObject(name2, resourcePath2, gameObject2);
            string shortDesc2 = "Stabilises";
            string longDesc2 = "Your HP can't go above a certain amount. Increases in damage the lower the amount.";
            ItemBuilder.SetupItem(item2, shortDesc2, longDesc2, "psm");
            item2.quality = ItemQuality.EXCLUDED;
            item2.RemovePickupFromLootTables();
        }

        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            LootEngine.GivePrefabToPlayer(Gungeon.Game.Items["psm:stabiliser"].gameObject, user);
        }
    }

    class ActiveStabiliser : PassiveItem
    {
        public override void Pickup(PlayerController player)
        {
            if (!m_pickedUpThisRun)
            {
                HP = player.healthHaver.GetCurrentHealth();
                Armor = player.healthHaver.Armor;
            }
            base.Pickup(player);
        }

        protected override void Update()
        {
            base.Update();
            if (Owner )
            {
                if (HP == 0)
                {
                    HP = 0.5f;
                }
                if (Armor <= 0 && Owner.healthHaver.GetMaxHealth() <= 0)
                {
                    Armor = 1;
                }
                if (HP < Owner.healthHaver.GetCurrentHealth())
                {
                    Owner.healthHaver.ForceSetCurrentHealth(HP);
                }
                if (Armor < Owner.healthHaver.Armor)
                {
                    Owner.healthHaver.Armor = Armor;
                }
                if (Owner.healthHaver.GetMaxHealth() > 0)
                {
                    float newHPercent = HP / Owner.healthHaver.GetMaxHealth();
                    bool v = (HPercent == newHPercent);
                    if (!v)
                    {
                        HPercent = newHPercent;
                        this.RemoveStat(PlayerStats.StatType.Damage);
                        if (Owner.characterIdentity != PlayableCharacters.Robot)
                        {
                            float damageToAdd = (1 - HPercent) * 0.8f;
                            if (Armor > 0)
                            {
                                damageToAdd += (1 - Armor / 4) * 0.2f;
                            }
                            else
                            {
                                damageToAdd += 0.25f;
                            }
                            this.AddStat(PlayerStats.StatType.Damage, damageToAdd);
                        }
                        else
                        {
                            this.AddStat(PlayerStats.StatType.Damage, (1 - Armor / 10) * 0.8f);
                        }
                        base.Owner.stats.RecalculateStats(base.Owner, true, false);
                    }
                } else if (MakeThisCodeGoodBeforeYouReleaseTheUpdateYouBitch != Armor)
                {
                    MakeThisCodeGoodBeforeYouReleaseTheUpdateYouBitch = Armor;
                    this.RemoveStat(PlayerStats.StatType.Damage);
                    this.AddStat(PlayerStats.StatType.Damage, (1 - Armor / 10) * 0.8f);
                }
            }
        }

        public float HPercent;

        public float HP;

        public float Armor;

        public float MakeThisCodeGoodBeforeYouReleaseTheUpdateYouBitch;
    }
}
