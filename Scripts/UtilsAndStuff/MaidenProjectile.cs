using System;
using UnityEngine;
using ItemAPI;

namespace katmod
{
	internal class MaidenProjectile : MonoBehaviour
	{
		public void Start()
		{
			this.projectile = base.GetComponent<Projectile>();
			this.player = (this.projectile.Owner as PlayerController);
			Projectile projectile = this.projectile;
			this.projectile.sprite.spriteId = this.projectile.sprite.GetSpriteIdByName("friendly_maiden_spear_001");
		}

		private Projectile projectile;

		private PlayerController player;
	}
}
