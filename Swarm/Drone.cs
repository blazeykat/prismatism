using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class Drone : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Drone Bee";
            string resourceName = "katmod/Resources/Swarm/beeswarm";
            GameObject obj = new GameObject();
            Drone item = obj.AddComponent<Drone>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Swarm";
            string longDesc = "Enemies have a chance to drop an ability token";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.A;
        }

        public override void Pickup(PlayerController player)
        {
            if (!m_pickedUpThisRun)
            {
                Level = 0;
                XP = 0;
            }
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage += OnEnemyDamaged;
        }

        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            XP += (int)damage;
            if (XP >= 1000 && Level < 5)
            {
                XP = 0;
                BoxOTools.Notify("LEVEL UP!", $"{Level + 1} => {Level + 2}", "katmod/Resources/Swarm/beeswarm", UINotificationController.NotificationColor.PURPLE);
                Level++;
            }
            if (enemy.specRigidbody != null && enemy.aiActor != null && base.Owner != null)
            {
                if (fatal)
                {
                    //if (BoxOTools.BasicRandom(.9f - (Level / 100)))
                    {
                        int pick = UnityEngine.Random.Range(1, 4 + Drone.Level);
                        switch (pick)
                        {
                            default:
                                ETGModConsole.Log($"Unknown number {pick} appeared at {Level}, oopsy daisies");
                                LootEngine.SpawnItem(Gungeon.Game.Items["psm:whitesplode"].gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f);
                                break;
                            case 1:
                                LootEngine.SpawnItem(Gungeon.Game.Items["psm:whitesplode"].gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f);
                                break;
                            case 2:
                                LootEngine.SpawnItem(Gungeon.Game.Items["psm:redsplode"].gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f);
                                break;
                            case 3:
                                LootEngine.SpawnItem(Gungeon.Game.Items["psm:bluesplode"].gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f);
                                break;
                            case 4:
                                LootEngine.SpawnItem(Gungeon.Game.Items["psm:beesneak"].gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f);
                                break;
                            case 5:
                                LootEngine.SpawnItem(Gungeon.Game.Items["psm:whiteup"].gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f);
                                break;
                            case 6:
                                LootEngine.SpawnItem(Gungeon.Game.Items["psm:yvswarm"].gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f);
                                break;
                        }
                    }
                }
            }
        }

        public static int XP;

        public static int Level;
    }
}
