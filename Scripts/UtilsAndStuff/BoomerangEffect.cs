using System;
using System.Collections;
using UnityEngine;

public class BoomerangEffect : MonoBehaviour
{
	public BoomerangEffect()
	{
		m_speed = 0.01f;
		m_range = .1f;
		m_damage = 0.2f;
		startingDamage = 0;
	}

	public void Start()
	{
		try
        {
		this.m_projectile = base.GetComponent<Projectile>();
		this.m_projectile.specRigidbody.UpdateCollidersOnScale = true;
		this.m_projectile.OnPostUpdate += this.HandlePostUpdate;
		} catch (Exception errex)
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
				if (m_projectile.baseData.damage < startingDamage * 1.7)
				{
					this.m_projectile.baseData.damage += m_damage;
				}
			}
			if ((m_projectile.Speed <= 0.1) && (m_projectile.Speed >= -0.1f))
            {
				StartCoroutine(HandleCooldown());
            }
		}
		catch (Exception errex)
		{
			ETGModConsole.Log($"{errex}");
		}
	}

	public IEnumerator HandleCooldown()
    {
		yield return new WaitForSeconds(m_range);
		if ((m_projectile.Speed <= 0.1) && (m_projectile.Speed >= -0.1f))
		{
			this.m_projectile.Speed -= m_speed;
		}
		yield break;
    }

	private Projectile m_projectile;

	private float m_lastElapsedDistance = 0;

	public float m_speed;

	public float m_range;

	public float m_damage;

	public float startingDamage;
}
