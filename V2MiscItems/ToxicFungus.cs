using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace katmod
{
	internal class ToxicFungus : PassiveItem
	{
		public static void Init()
		{
			string name = "Toxic Fungus";
			string resourcePath = "katmod/Resources/V2MiscItems/fungus.png";
			GameObject gameObject = new GameObject(name);
			ToxicFungus item = gameObject.AddComponent<ToxicFungus>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Plague";
			string longDesc = "Creates a toxic aura when standing still\n\nA previously pacifistic fungus genetically modified to kill.";
			item.SetupItem(shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.D;
			item.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			item.PlaceItemInAmmonomiconAfterItemById(258);
		}

		public override void Pickup(PlayerController player)
		{
			bool pickedUp = this.m_pickedUp;
			if (!pickedUp)
			{
				base.Pickup(player);
			}
		}

		protected override void Update()
		{
			base.Update();
			if (this.m_pickedUp && !GameManager.Instance.IsLoadingLevel && this.m_owner != null && this.Owner.Velocity.magnitude == 0 && !this.m_owner.IsFalling && this.m_owner.healthHaver && !this.m_owner.healthHaver.IsDead)
			{
				if (WalkFlag)
				{
					if (!Active) 
					{
						this.ShockRing();
						this.Active = true;
					}
					List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
					Vector2 centerPosition = base.Owner.CenterPosition;
					foreach (AIActor aiactor in activeEnemies)
					{
						bool flag = aiactor != null && aiactor.specRigidbody != null && Vector2.Distance(aiactor.CenterPosition, centerPosition) < 4f && aiactor.healthHaver.GetMaxHealth() > 0f &&base.Owner != null;
						if (flag)
						{
							aiactor.ApplyEffect(Gungeon.Game.Items["irradiated_lead"].GetComponent<BulletStatusEffectItem>().HealthModifierEffect);
						}
					}
				} else
                {
					StartCoroutine(StartCooldown());
                }
			}
			else if (Active)
			{
				StopCoroutine(StartCooldown());
				WalkFlag = false;
				this.Active = false;
				this.m_radialIndicator.EndEffect();
			}
		}

		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(1.5f);
			WalkFlag = true;
			yield break;
		}

		private void ShockRing()
		{
			try
            {

			this.m_radialIndicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), base.Owner.CenterPosition.ToVector3ZisY(0), Quaternion.identity, base.Owner.transform)).GetComponent<HeatIndicatorController>();
			this.m_radialIndicator.CurrentColor = Color.green;
			this.m_radialIndicator.IsFire = false;
			this.m_radialIndicator.CurrentRadius = 4f;
			} catch (Exception e)
            {
				e.ToString().Log();
            }
		}

		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			debrisObject.GetComponent<ToxicFungus>().m_pickedUpThisRun = true;
			return debrisObject;
		}

		private HeatIndicatorController m_radialIndicator;

		private bool Active = false;

		private bool WalkFlag;
	}
}
