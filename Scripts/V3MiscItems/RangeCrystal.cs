using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace katmod
{
    class RangeCrystal : PassiveItem
    {
        public static void Init()
		{
			string name = "Laser Crystal";
			string resourcePath = "katmod/Resources/V3MiscItems/focuscrystal.png";
			GameObject gameObject = new GameObject(name);
			RangeCrystal item = gameObject.AddComponent<RangeCrystal>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Focused";
			string longDesc = "Does more damage at close range.\n\nThis crystal is powered by my hatred for Never \"Prismatism is a virus\" Named.";
			item.SetupItem(shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(273);
			item.sprite.IsPerpendicular = true;
		}

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                sourceProjectile.specRigidbody.OnPreRigidbodyCollision += OnHitEnemy;
            }
            catch (Exception ex)
            {
                global::ETGModConsole.Log(ex.Message, false);
            }
        }
        private void OnHitEnemy(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (otherRigidbody != null && otherRigidbody.aiActor != null && myRigidbody != null && myRigidbody.projectile && otherRigidbody.aiActor.healthHaver)
            {
                float distance = Vector3.Distance(otherRigidbody.UnitCenter, myRigidbody.UnitCenter);
                if (distance < 5)
                {
                    myRigidbody.projectile.baseData.damage *= 1 + (0.5f - (distance / 10));
                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return base.Drop(player);
        }
    }
}
