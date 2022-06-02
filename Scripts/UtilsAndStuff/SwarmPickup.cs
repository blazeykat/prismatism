using ItemAPI;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace katmod
{
    class SwarmPickup : PickupObject
    {
        public override void Pickup(PlayerController player)
        {
            UnityEngine.Object.Destroy(base.gameObject);
            player.BloopItemAboveHead(this.sprite);
            storedPlayer = player;
        }

        protected void Start()
        {
            try
            {
                this.storedBody = base.gameObject.GetComponent<SpeculativeRigidbody>();
                SpeculativeRigidbody srb = this.storedBody;
                srb.OnTriggerCollision += OnPreCollision;
            }
            catch (Exception er)
            {
                er.ToString().Log();
            }
        }

        public void OnPreCollision(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody source, CollisionData collisionData)
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

/*        private bool DoYouHearTheClockStop = true;

        private void Update()
        {
            if (preDisappear > preDuration)
            {
                if (Disappear > duration && gameObject)
                {
                    GameObject.Destroy(gameObject);
                }
                else
                {
                    if (DoYouHearTheClockStop)
                    {
                        sprite.usesOverrideMaterial = true;
                        sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitBlendUber");
                        sprite.renderer.material.SetFloat("_VertexColor", 1f);
                        DoYouHearTheClockStop = false;
                    }
                    //sprite.color.a = 255 - (Disappear / duration) * 255;
                    sprite.color = sprite.color.WithAlpha(1 - ((Disappear / duration) * 1));
                    Disappear += BraveTime.DeltaTime;
                }
            }
            else
            {
                preDisappear += BraveTime.DeltaTime;
            }
        }

        private float preDisappear;

        private float Disappear;

        private float preDuration = 4;

        private float duration = 5;*/

        public SpeculativeRigidbody storedBody;

        private bool m_hasBeenPickedUp;

        private PlayerController storedPlayer;

        public PlayerController StoredPlayer
        {
            get
            {
                return storedPlayer;
            }
        }
    }
}
