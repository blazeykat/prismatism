using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;

namespace katmod
{

	internal class PlagueBullets : GunVolleyModificationItem
	{

		public static void Init()
		{
			string name = "Plague Bullets";
			string resourcePath = "katmod/Resources/V2MiscItems/plaguebullets";
			GameObject gameObject = new GameObject(name);
			PlagueBullets item = gameObject.AddComponent<PlagueBullets>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Green Death";
			string longDesc = "Has a chance to shoot a bouncing plague bullet on fire.\n\nTheir cure is most efficient.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.B;
			item.PlaceItemInAmmonomiconAfterItemById(640);
			List<string> mandatoryConsoleIDs = new List<string>
			{
				"psm:plague_bullets",
				"plague_pistol"
			};
			CustomSynergies.Add("Thank god for me.", mandatoryConsoleIDs, null, true);
		}

		private void PostProcessProjectile(Projectile projectile, float Chance)
		{
			PlayerController player = base.Owner;
			if (Utilities.BasicRandom(player, 0.7f, 50) && !CoolAsIce)
			{
				CoolAsIce = true;
				StartCoroutine(StartCooldown());
				if (player.HasGun(207))
				{
					for (int counter = 0; counter < 3; counter++)
					{
						Projectile component = player.HandleProjectileAimed(20f, 12f, 207, UnityEngine.Random.Range(0.0f, 360.0f), 500);
						BounceProjModifier bouncy = component.gameObject.AddComponent<BounceProjModifier>();
						bouncy.numberOfBounces = 10;
						bouncy.onlyBounceOffTiles = true;
						PierceProjModifier spook = component.gameObject.AddComponent<PierceProjModifier>();
						spook.penetration = 5;
					}
				} else
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


		private void PostProcessBeamChanceTick(BeamController beamController)
		{
			PlayerController player = base.Owner;
			if (Utilities.BasicRandom(player, 0.7f, 50) && !CoolAsIce)
			{
				CoolAsIce = true;
				StartCoroutine(StartCooldown());
				if (player.HasGun(207))
				{
					for (int counter = 0; counter <= 3; counter++)
					{
						Projectile component = player.HandleProjectileAimed(20f, 12f, 207, UnityEngine.Random.Range(0.0f, 360.0f), 500);
						BounceProjModifier bouncy = component.gameObject.AddComponent<BounceProjModifier>();
						bouncy.numberOfBounces = 10;
						bouncy.onlyBounceOffTiles = true;
						PierceProjModifier spook = component.gameObject.AddComponent<PierceProjModifier>();
						spook.penetration = 5;
					}
				}
				else
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


		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(1f);
			this.CoolAsIce = false;
			yield break;
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile;
			player.PostProcessBeamChanceTick += this.PostProcessBeamChanceTick;
		}
		protected new PlayerController m_owner;
		public override DebrisObject Drop(PlayerController player)
		{
			player.PostProcessProjectile -= this.PostProcessProjectile;
			player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
			return base.Drop(player);
		}

		private bool CoolAsIce = false;

	}
}
