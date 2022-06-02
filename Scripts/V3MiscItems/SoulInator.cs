using Dungeonator;
using ItemAPI;
using System.Collections.Generic;
using UnityEngine;

namespace katmod
{
    class SoulInator : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Jar O' Souls";
            string resourceName = "katmod/Resources/V3MiscItems/jarfullofsouls";
            GameObject obj = new GameObject(itemName);
            SoulInator item = obj.gameObject.AddComponent<SoulInator>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "I am a motherf#%$cking ghost!";
            string longDesc = "Duplicate every living enemy as a ghost, which fights for you.\n\nScreams and wailing emanate from the jar. Best not to think about it.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(558);
            item.SetCooldownType(ItemBuilder.CooldownType.Damage, 500);
            item.consumable = false;
        }

        public override bool CanBeUsed(PlayerController user)
        {
            if (user && user.CurrentRoom != null)
            {
                List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies.Count != 0)
                {
                    return base.CanBeUsed(user);
                }
            }
            return false;
        }

        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            if (user.CurrentRoom != null && user)
            {
                List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (activeEnemies.Count != 0 && activeEnemies != null)
                {
                    for (int counter = 0; counter < activeEnemies.Count; counter++)
                    {
                        AIActor actor = activeEnemies[counter];
                        if (actor && actor.healthHaver && !actor.healthHaver.IsBoss && actor.gameObject.GetComponent<GhostAllyComponent>() == null && actor.healthHaver.IsVulnerable && actor.IsNormalEnemy && !actor.IsMimicEnemy && actor.GetResistanceForEffectType(EffectResistanceType.Charm) != 1)
                        {
                            List<int> Colors = new List<int>
                            {
                                7,
                                141,
                                222
                            };
                            GameActorCharmEffect ghostCharm = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect;
                            ghostCharm.OverheadVFX = null;
                            ghostCharm.TintColor = new Color(Colors[0], Colors[1], Colors[2]);
                            ghostCharm.TintColor.a /= 100;
                            string guid = actor.EnemyGuid;
                            AIActor aiactor = BoxOTools.SummonAtRandomPosition(guid, user);
                            aiactor.AddPermanentCharm(ghostCharm);
                            aiactor.gameObject.AddComponent<GhostAllyComponent>();
                            aiactor.HandleReinforcementFallIntoRoom(counter / 10);
                        }
                    }
                }
            }
        }

        private class GhostAllyComponent : MonoBehaviour
        {
            public void Start()
            {
                aiactor = gameObject.GetComponent<AIActor>();
            }

            private AIActor aiactor;

            public void Update()
            {
                if (timer > 2)
                {
                    if (aiactor != null && aiactor.healthHaver != null)
                    {
                        timer = 0;
                        aiactor.healthHaver.ApplyDamage(1, Vector2.zero, "I should go to bed");
                    }
                }
                timer += BraveTime.DeltaTime;
            }

            private float timer = 0;
        }
    }
}
