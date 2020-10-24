using Dungeonator;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace katmod
{
    class ParrotsFeather : PlayerItem
	{
		public static void Init()
		{
			string name = "Parrot's Feather";
			string resourcePath = "katmod/Resources/V3MiscItems/parrotsfeather";
			GameObject gameObject = new GameObject(name);
			ParrotsFeather item = gameObject.AddComponent<ParrotsFeather>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Traveller";
			string longDesc = "Permanently charm 1 non-boss enemy. The charmed dies on room clear.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = ItemQuality.D;
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 150f);
			item.PlaceItemInAmmonomiconAfterItemById(307);
			item.SetupUnlockOnFlag(GungeonFlags.ACHIEVEMENT_ACCESS_ABBEY, true);

			GameObject target = new GameObject("parrot_ui");
			ItemBuilder.AddSpriteToObject("parrot_ui", "katmod/Resources/V3MiscItems/featherui", target);
			targetPrefab = target;
		}

		public override bool CanBeUsed(PlayerController user)
		{
			AIActor actor = user.CurrentRoom.GetNearestEnemy(Camera.main.ScreenToWorldPoint(Input.mousePosition), out float distance);
			if (actor && actor.healthHaver && actor.healthHaver.IsVulnerable && distance < 3 && !actor.healthHaver.IsBoss)
			{
				return base.CanBeUsed(user);
			}
			return false;
		}

		public override void Update()
		{
			base.Update();
			if (LastOwner != null && LastOwner.CurrentRoom != null)
			{
				AIActor actor = LastOwner.CurrentRoom.GetNearestEnemy(Camera.main.ScreenToWorldPoint(Input.mousePosition), out float distance);
				if (actor && actor.healthHaver && actor.healthHaver.IsVulnerable && distance < 3 && !IsOnCooldown && !actor.healthHaver.IsBoss)
				{
					if (actor != StoredEnemy || target == null)
					{
						StoredEnemy = actor;
						if (ParrotsFeather.target)
						{
							Destroy(ParrotsFeather.target);
						}
						GameObject target = GameObject.Instantiate(targetPrefab, StoredEnemy.specRigidbody.UnitBottomCenter, Quaternion.identity, actor.transform);
						target.transform.position.WithZ(transform.position.z + 99999);
						target.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(StoredEnemy.specRigidbody.UnitBottomCenter, tk2dBaseSprite.Anchor.LowerCenter);
						ParrotsFeather.target = target;
					}
				}
				else if (target)
				{
					Destroy(ParrotsFeather.target);
				}
			}
		}

		protected override void DoEffect(PlayerController user)
		{
			base.DoEffect(user);
			AIActor actor = user.CurrentRoom.GetNearestEnemy(Camera.main.ScreenToWorldPoint(Input.mousePosition), out _);
			if (actor && actor.healthHaver && !actor.healthHaver.IsBoss)
			{
				BoxOTools.AddPermanentCharm(actor);
				if (user.PlayerHasActiveSynergy("Flock Together") && user.CurrentRoom != null)
				{
					List<AIActor> synergyActors = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
					for (int i = 0; i < synergyActors.Count; i++)
                    {
						if (Vector2.Distance(synergyActors[i].specRigidbody.UnitCenter, actor.specRigidbody.UnitCenter) < 5)
                        {
							BoxOTools.AddPermanentCharm(synergyActors[i]);
						}
                    }
				}
			}
			if (user.PlayerHasActiveSynergy("Birds of a Feather") && user.CurrentRoom != null)
            {
				AIActor bird = BoxOTools.SummonAtRandomPosition("ed37fa13e0fa4fcf8239643957c51293", user);
				BoxOTools.AddPermanentCharm(bird);
            }
		}

		private static GameObject target;

		private static GameObject targetPrefab;

		public static AIActor StoredEnemy;

	}
}