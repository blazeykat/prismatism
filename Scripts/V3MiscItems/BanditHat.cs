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
            item.quality = PickupObject.ItemQuality.C;
            item.PlaceItemInAmmonomiconAfterItemById(485);
            item.AddItemToDougMetaShop(20);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.BANDITHAT_FLAG, true);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Buy it");
        }
        
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat += HotSpheroid;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnEnteredCombat -= HotSpheroid;
            return base.Drop(player);
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
            m_radialIndicator = ((GameObject)Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), base.Owner.CenterPosition.ToVector3ZisY(0), Quaternion.identity, Owner.transform)).GetComponent<HeatIndicatorController>();
            m_radialIndicator.CurrentColor = Color.red;
            m_radialIndicator.IsFire = true;
            m_radialIndicator.CurrentRadius = 4f;

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
