using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class BanditHat : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Bandits Hat";
            string resourceName = "katmod/Resources/V3MiscItems/bandit";
            GameObject obj = new GameObject();
            BanditHat item = obj.AddComponent<BanditHat>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Cacti";
            string longDesc = "Creates a heat ring upon entering the room.\n\nTaken from the leafy corpse of an infamous train robber.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(485);
        }

        
        
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat += HotSpheroid;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnEnteredCombat -= HotSpheroid;
            return debrisObject;
        }

        protected override void Update()
        {
            base.Update();
            if (this.m_pickedUp && m_owner)
            {
                if (Active)
                {
                    List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                    Vector2 centerPosition = base.Owner.CenterPosition;
                    foreach (AIActor aiactor in activeEnemies)
                    {
                        bool flag = Vector2.Distance(aiactor.CenterPosition, centerPosition) < 4f && aiactor.healthHaver.GetMaxHealth() > 0f && aiactor != null && aiactor.specRigidbody != null && base.Owner != null;
                        if (flag)
                        {
                            aiactor.ApplyEffect(Gungeon.Game.Items["hot_lead"].GetComponent<BulletStatusEffectItem>().FireModifierEffect);
                        }
                    }
                }
            }
        }

        private void HotSpheroid()
        {
            HeatUp();
            StartCoroutine(UnCoolDown());
        }

        private void HeatUp()
        {
            Active = true;
            this.m_radialIndicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), base.Owner.CenterPosition, Quaternion.identity, base.transform)).GetComponent<HeatIndicatorController>();
            this.m_radialIndicator.CurrentColor = Color.red;
            this.m_radialIndicator.IsFire = true;
            this.m_radialIndicator.CurrentRadius = 4f;

        }

        private IEnumerator UnCoolDown()
        {
            yield return new WaitForSeconds(8);
            this.Active = false;
            this.m_radialIndicator.EndEffect();
            yield break;
        }

        private HeatIndicatorController m_radialIndicator;

        private bool Active = false;
    }
}
