using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace katmod
{

	internal class MaidenPlating : PassiveItem
	{

		public static void Init()
		{
			string name = "Maiden Plating";
			string resourcePath = "katmod/Resources/V2MiscItems/maidenplating";
			GameObject gameObject = new GameObject(name);
			MaidenPlating item = gameObject.AddComponent<MaidenPlating>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Utter Garbage";
			string longDesc = "Shoots 8 lead maiden projectiles upon taking damage.\n\nNow *you* can be the game ruiner!";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.A;
			item.PlaceItemInAmmonomiconAfterItemById(564);
		}

		private void OnPlayerHit(PlayerController player)
		{
			for (int counter = 0; counter < 8; counter++) 
			{
				Projectile maidenProjectile = base.Owner.HandleProjectileAimed(5, 25, 58, (45 * counter), 300);
				maidenProjectile.gameObject.AddComponent<MaidenProjectile>();
				HomingModifier homing = maidenProjectile.gameObject.AddComponent<HomingModifier>();
				homing.HomingRadius = 500;
				homing.AngularVelocity = 500;
			}
		}


		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnReceivedDamage += this.OnPlayerHit;
		}
		protected new PlayerController m_owner;
		public override DebrisObject Drop(PlayerController player)
		{
			player.OnReceivedDamage -= this.OnPlayerHit;

			return base.Drop(player);
		}


	}
}
