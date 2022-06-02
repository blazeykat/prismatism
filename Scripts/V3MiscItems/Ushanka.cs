﻿using Dungeonator;
using ItemAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace katmod
{
    class Ushanka : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Magic Snowball";
            string resourceName = "katmod/Resources/V3MiscItems/snoball";
            GameObject obj = new GameObject();
            Ushanka item = obj.AddComponent<Ushanka>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Cold as Ice";
            string longDesc = "Creates a wintery ring upon killing enemies. Additional kills increase radius.\n\nCreated by a winter loving wizard, so he could bring the winter wherever he could go";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.A;
            item.PlaceItemInAmmonomiconAfterItemById(364);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnKilledEnemy += Colden;
        }

        private void Colden(PlayerController obj)
        {
            Kills += 1;
            StartCoroutine(Heatdown());
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnKilledEnemy -= Colden;
            return debrisObject;
        }

        protected override void Update()
        {
            base.Update();
            if (this.m_pickedUp && m_owner)
            {
                if (Active)
                {
                    if (Kills == 0)
                    {
                        Active = false;
                        this.m_radialIndicator.EndEffect();
                    }

                    if (StoredKills != Kills)
                    {
                        StoredKills = Kills;
                        this.m_radialIndicator.CurrentRadius = 3f + Kills;
                    }
                    List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                    Vector2 centerPosition = base.Owner.CenterPosition;
                    foreach (AIActor aiactor in activeEnemies)
                    {
                        if (aiactor.GetComponent<CooldownComponent>() == null)
                        {
                            aiactor.gameObject.AddComponent<CooldownComponent>();
                        }
                        bool flag = Vector2.Distance(aiactor.CenterPosition, centerPosition) < this.m_radialIndicator.CurrentRadius && aiactor.healthHaver.GetMaxHealth() > 0f && aiactor != null && aiactor.specRigidbody != null && base.Owner != null;
                        if (flag && aiactor.gameObject.AddComponent<CooldownComponent>().ReadyToIce && aiactor.healthHaver.IsVulnerable && aiactor.isActiveAndEnabled)
                        {
                            GameActorFreezeEffect freeze = Gungeon.Game.Items["frost_bullets"].GetComponent<BulletStatusEffectItem>().FreezeModifierEffect;
                            freeze.FreezeAmount = 10;
                            aiactor.ApplyEffect(freeze);
                            aiactor.gameObject.GetComponent<CooldownComponent>().HandleCoolestdown();
                        }
                    }
                }
                else if (Kills > 0)
                {
                    CoolUp();
                }
            }
        }

        private void CoolUp()
        {
            Active = true;
            m_radialIndicator = ((GameObject)Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), Owner.CenterPosition.ToVector3ZisY(0), Quaternion.identity)).GetComponent<HeatIndicatorController>();
            m_radialIndicator.transform.parent = Owner.transform;
            m_radialIndicator.CurrentColor = Color.cyan;
            m_radialIndicator.IsFire = false;
            m_radialIndicator.CurrentRadius = 3f + Kills;
        }

        private IEnumerator Heatdown()
        {
            yield return new WaitForSeconds(5);
            Kills -= 1;
            yield break;
        }

        private HeatIndicatorController m_radialIndicator;

        private bool Active = false;

        private int Kills = 0;

        private int StoredKills = 0;
    }

    class CooldownComponent : MonoBehaviour
    {
        public bool ReadyToIce;

        public CooldownComponent()
        {
            ReadyToIce = true;
        }

        public void HandleCoolestdown()
        {
            ReadyToIce = false;
            StartCoroutine(numerate());
        }

        public IEnumerator numerate()
        {
            yield return new WaitForSeconds(0.1f);
            this.ReadyToIce = true;
            yield break;
        }
    }
}
