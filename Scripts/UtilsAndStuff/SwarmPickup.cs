using ItemAPI;
using System;
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
