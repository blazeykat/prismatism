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
            player.healthHaver.ModifyDamage += IdiotDumbDumb;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.healthHaver.ModifyDamage -= IdiotDumbDumb;
            return base.Drop(player);
        }

        public void IdiotDumbDumb(HealthHaver health, HealthHaver.ModifyDamageEventArgs damageArgs)
        {
            if (damageArgs != EventArgs.Empty)
            {
                if (damageArgs.InitialDamage <= 2)
                {
                    damageArgs.ModifiedDamage = 0.5f;
                }
            }
        }

        /*public void ReductionEvent(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (otherRigidbody.projectile && otherRigidbody.projectile.IsBlackBullet)
            {
                otherRigidbody.projectile.ReturnFromBlackBullet();
            }
        }*/
    }
}
