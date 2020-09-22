using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace katmod
{

	internal class ToxicHoneycomb : PassiveItem
	{

		public static void Init()
		{
			string name = "Toxic Honeycomb";
			string resourcePath = "katmod/Resources/V2MiscItems/greencomb";
			GameObject gameObject = new GameObject(name);
			ToxicHoneycomb item = gameObject.AddComponent<ToxicHoneycomb>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Toxic Coffee";
			string longDesc = "Releases poisonous bees upon damage.\n\nBestLife Inc. cannot be held responsible for any damage done to you from radiation poisoning.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.C;
			item.PlaceItemInAmmonomiconAfterItemById(138);
		}

		private void OnPlayerHit(PlayerController player)
		{
			for (int counter = 0; counter < (player.HasPassiveItem(138) || player.HasPickupID(432) || player.HasGun(14) ? 16 : 8); counter++)
			{
				Projectile maidenProjectile = base.Owner.HandleProjectileAimed(9, 5, 14, ((player.HasPassiveItem(138) || player.HasPickupID(432) || player.HasGun(14) ? 22.5f : 45) * counter), 300);
				maidenProjectile.DefaultTintColor = Color.green;
				maidenProjectile.HasDefaultTint = true;
				maidenProjectile.statusEffectsToApply = poison;
				if (player.HasGun(207) || player.HasGun(151) || player.HasGun(513))
				{
					Projectile component = player.HandleProjectileAimed(20f, 12f, 207, UnityEngine.Random.Range(0.0f, 360.0f), 500);
					BounceProjModifier bouncy = component.gameObject.AddComponent<BounceProjModifier>();
					bouncy.numberOfBounces = 10;
					bouncy.onlyBounceOffTiles = true;
					PierceProjModifier spook = component.gameObject.AddComponent<PierceProjModifier>();
					spook.penetration = 5;
				}
			}
		}


		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnReceivedDamage += this.OnPlayerHit;
		}
		public override DebrisObject Drop(PlayerController player)
		{
			player.OnReceivedDamage -= this.OnPlayerHit;

			return base.Drop(player);
		}

		public List<GameActorEffect> poison = new List<GameActorEffect>() 
		{
			Gungeon.Game.Items["irradiated_lead"].GetComponent<BulletStatusEffectItem>().HealthModifierEffect
		};
	}
}
