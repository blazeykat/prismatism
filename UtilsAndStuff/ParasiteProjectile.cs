using katmod;
using System;
using System.Collections;
using UnityEngine;

public class ParasiteProjectile : MonoBehaviour
{

	public void Start()
	{
		try
		{
			base.GetComponent<Projectile>().OnHitEnemy += HitOnEnemy;
		}
		catch (Exception errex)
		{
			ETGModConsole.Log($"{errex}");
		}
	}


	public void HitOnEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
	{
		if (arg2.healthHaver != null)
		{
			if (arg2.healthHaver.gameObject.GetComponent<ParasiteDeathEffect>() == null)
			{
				ParasiteDeathEffect parasite = arg2.healthHaver.gameObject.AddComponent<ParasiteDeathEffect>();
				parasite.frogger = thingy;
			}
		}
	}

	public void EnemyDiesLol(Vector2 theposition)
	{
		try
		{
			for (int count = 0; count < 11; count++)
			{
				Projectile projectile2 = ((Gun)ETGMod.Databases.Items[51]).DefaultModule.projectiles[0];
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, theposition, Quaternion.Euler(0f, 0f, (UnityEngine.Random.Range(0, 360))), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				bool componentless = component != null;
				if (componentless)
				{
					component.Owner = thingy;
					component.Shooter = thingy.specRigidbody;
					component.baseData.speed = UnityEngine.Random.Range(3, 6);
					component.baseData.damage = 12;
					PierceProjModifier pp = component.gameObject.AddComponent<PierceProjModifier>();
					pp.penetration = 1;
				}
			}

		} catch (Exception errorinator)
        {
			ETGModConsole.Log($"{errorinator}");
        }
	}

	public PlayerController thingy;
}
