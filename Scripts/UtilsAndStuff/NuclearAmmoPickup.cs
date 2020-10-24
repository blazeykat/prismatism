using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class NuclearAmmoPickup : PickupObject
    {
        public static int id;
        public static void Init()
        {
            string name = "Box O' Ammo";
            string resourcePath = "katmod/Resources/V3MiscItems/ammocrate.png";
            GameObject gameObject = new GameObject(name);
            NuclearAmmoPickup item = gameObject.AddComponent<NuclearAmmoPickup>();
            SpeculativeRigidbody specRig = gameObject.AddComponent<SpeculativeRigidbody>();
            PixelCollider collide = new PixelCollider
            {
                IsTrigger = true,
                ManualWidth = 13,
                ManualHeight = 16,
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                CollisionLayer = CollisionLayer.PlayerBlocker,
                ManualOffsetX = 0,
                ManualOffsetY = 0
            };
            specRig.PixelColliders = new List<PixelCollider>
            {
                collide
            };
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Lucky Shot";
            string longDesc = "Some ammo, in a box!";
            item.SetupItem(shortDesc, longDesc);
            item.quality = PickupObject.ItemQuality.EXCLUDED;
            NuclearAmmoPickup.id = item.PickupObjectId;
        }
        

        public override void Pickup(PlayerController player)
        {
            if (player.GetComponent<NuclearPlayerController>())
            {
                NuclearPlayerController heSucks = player.GetComponent<NuclearPlayerController>();
                if (heSucks.Shells + 8 > 55)
                {
                    BoxOTools.Notify("AMMO BOX", "Max Shells", "katmod/Resources/V3MiscItems/ammocrate");
                    heSucks.Shells = 55;
                } else
                {
                    BoxOTools.Notify("AMMO BOX", "+8 Shells", "katmod/Resources/V3MiscItems/ammocrate");
                    heSucks.Shells += 8;
                }
            }
            player.BloopItemAboveHead(this.sprite);
            AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", base.gameObject);
            UnityEngine.Object.Destroy(base.gameObject);
        }

        protected void Start()
        {
            try
            {
                this.storedBody = base.gameObject.GetComponent<SpeculativeRigidbody>();
                SpeculativeRigidbody srb = this.storedBody;
                srb.OnTriggerCollision += OnPreCollision;
            } catch (Exception er)
            {
                er.ToString().Log();
            }
        }

        private void OnPreCollision(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody source, CollisionData collisionData)
        {
            if (this.m_hasBeenPickedUp)
            {
                return;
            }
            PlayerController component = otherRigidbody.GetComponent<PlayerController>();
            if (component != null)
            {
                this.m_hasBeenPickedUp = true;
                this.Pickup(component);
            }
        }

        public SpeculativeRigidbody storedBody;

        private bool m_hasBeenPickedUp;
    }
}
