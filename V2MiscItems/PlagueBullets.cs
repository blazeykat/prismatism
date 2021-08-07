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
			item.quality = PickupObject.ItemQuality.C;
			item.PlaceItemInAmmonomiconAfterItemById(640);
		}

		private void PostProcessProjectile(Projectile projectile, float Chance)
		{
			if (BoxOTools.BasicRandom(0.85f) && !CoolAsIce)
			{
				CoolAsIce = true;
				StartCoroutine(StartCooldown());
				if (Owner.PlayerHasActiveSynergy("Plaguebringer Goliath"))
				{
					for (int counter = 0; counter < 3; counter++)
					{
						Projectile component = Owner.HandleProjectile(20f, 12f, 207, true, false, UnityEngine.Random.Range(0.0f, 360.0f), 500);
						BounceProjModifier bouncy = component.gameObject.AddComponent<BounceProjModifier>();
						bouncy.numberOfBounces = 10;
						bouncy.onlyBounceOffTiles = true;
						PierceProjModifier spook = component.gameObject.AddComponent<PierceProjModifier>();
						spook.penetration = 5;
					}
				} else
                {
					Projectile component = Owner.HandleProjectile(20f, 12f, 207, true, false, UnityEngine.Random.Range(0.0f, 360.0f), 500);
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
			if (BoxOTools.BasicRandom(0.7f) && !CoolAsIce)
			{
				CoolAsIce = true;
				StartCoroutine(StartCooldown());
				if (Owner.PlayerHasActiveSynergy("Plaguebringer Goliath"))
				{
					for (int counter = 0; counter < 3; counter++)
					{
						Projectile component = Owner.HandleProjectile(20f, 12f, 207, true, false, UnityEngine.Random.Range(0.0f, 360.0f), 500);
						BounceProjModifier bouncy = component.gameObject.AddComponent<BounceProjModifier>();
						bouncy.numberOfBounces = 10;
						bouncy.onlyBounceOffTiles = true;
						PierceProjModifier spook = component.gameObject.AddComponent<PierceProjModifier>();
						spook.penetration = 5;
					}
				}
				else
				{
					Projectile component = Owner.HandleProjectile(20f, 12f, 207, true, false, UnityEngine.Random.Range(0.0f, 360.0f), 500);
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

		public override DebrisObject Drop(PlayerController player)
		{
			player.PostProcessProjectile -= this.PostProcessProjectile;
			player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
			return base.Drop(player);
		}

		private bool CoolAsIce = false;

	}
}
