using MultiplayerBasicExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class ParasiteDeathEffect : OnDeathBehavior
    {
		protected override void OnTrigger(Vector2 dirVec)
		{
			if (base.enabled)
			{
				for (int count = 0; count < 21; count++)
				{
					Projectile projectile2 = ((Gun)ETGMod.Databases.Items[51]).DefaultModule.projectiles[0];
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, base.specRigidbody.GetUnitCenter(ColliderType.HitBox), Quaternion.Euler(0f, 0f, (UnityEngine.Random.Range(0, 360))), true);
					Projectile component = gameObject.GetComponent<Projectile>();
					bool componentless = component != null;
					if (componentless)
					{
						component.Owner = frogger;
						component.Shooter = frogger.specRigidbody;
						component.baseData.speed = UnityEngine.Random.Range(6, 8);
						component.baseData.damage = 12;
						PierceProjModifier pp = component.gameObject.AddComponent<PierceProjModifier>();
						pp.penetration = 1;
					}
				}
			}
		}
		public PlayerController frogger;
	}

}
