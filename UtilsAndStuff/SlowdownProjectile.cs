using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;

namespace katmod
{
    class SlowdownProjectile : MonoBehaviour
    {
		public SlowdownProjectile()
		{
			m_speed = 0.01f;
			m_range = .1f;
			DieAfterStop = false;
			DeathTime = 0.5f;
			Cooldown = false;
		}

		public void Start()
		{
			try
			{
				this.m_projectile = base.GetComponent<Projectile>();
				this.m_projectile.specRigidbody.UpdateCollidersOnScale = true;
				this.m_projectile.OnPostUpdate += this.HandlePostUpdate;
			}
			catch (Exception errex)
			{
				ETGModConsole.Log($"{errex}");
			}
		}

		public virtual void OnDespawned()
		{
			UnityEngine.Object.Destroy(this);
		}

		private void HandlePostUpdate(Projectile proj)
		{
			try
			{
				if (!proj)
				{
					return;
				}
				float elapsedDistance = proj.GetElapsedDistance();
				if (elapsedDistance - m_lastElapsedDistance > m_range)
				{
					this.m_lastElapsedDistance = elapsedDistance;
					m_projectile.Speed -= m_speed;
				}
				if ((m_projectile.Speed <= 0.1) && (m_projectile.Speed >= -0.1f) && DieAfterStop && !Cooldown)
				{
					Cooldown = true;
					StartCoroutine(DeathTimer());
				}
			}
			catch (Exception errex)
			{
				ETGModConsole.Log($"{errex}");
			}
		}

		private IEnumerator DeathTimer()
        {
			yield return new WaitForSeconds(DeathTime);
			m_projectile.DieInAir();
			yield break;
        }

		private Projectile m_projectile;

		private float m_lastElapsedDistance = 0;

		public float m_speed;

		public float m_range;

		public bool DieAfterStop;

		public float DeathTime;

		private bool Cooldown;
	}
}
