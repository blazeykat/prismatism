using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace katmod
{
    class DamageReductionItem : PassiveItem
    {
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.specRigidbody.OnPreRigidbodyCollision += ReductionEvent;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.specRigidbody.OnPreRigidbodyCollision -= ReductionEvent;
            return base.Drop(player);
        }

        public void ReductionEvent(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (otherRigidbody.projectile && otherRigidbody.projectile.IsBlackBullet)
            {
                otherRigidbody.projectile.ReturnFromBlackBullet();
            }
        }
    }
}
