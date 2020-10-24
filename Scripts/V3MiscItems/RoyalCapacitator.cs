using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class RoyalCapacitator : PlayerItem
	{
		public static void Init()
		{
			string name = "Royal Capacitor";
			string resourcePath = "katmod/Resources/V3MiscItems/royalcapactior";
			GameObject gameObject = new GameObject(name);
			RoyalCapacitator item = gameObject.AddComponent<RoyalCapacitator>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "THUNDERSTRUCK";
			string longDesc = "Channel the heavens.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = ItemQuality.S;
			item.PlaceItemInAmmonomiconAfterItemById(298);
			item.SetupUnlockOnFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_FORGE, true);
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 750f);

			GameObject target = new GameObject("capacitor_ui");
			ItemBuilder.AddSpriteToObject("capacitor_ui", "katmod/Resources/V3MiscItems/royalcapacitatorui", target);
            targetPrefab = target;

			GameObject lightning = ItemBuilder.AddSpriteToObject("capacitor_lightning", "katmod/Resources/V3MiscItems/Lightning/ligthning_001");
			FakePrefab.MarkAsFakePrefab(lightning);
			UnityEngine.Object.DontDestroyOnLoad(lightning);
			tk2dSpriteAnimator animator = lightning.AddComponent<tk2dSpriteAnimator>();
			tk2dSpriteAnimationClip animationClip = new tk2dSpriteAnimationClip();
			animationClip.fps = 14;
			animationClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
			animationClip.name = "start";
			GameObject spriteObject = new GameObject("spriteObject");
			ItemBuilder.AddSpriteToObject("spriteObject", $"katmod/Resources/V3MiscItems/Lightning/ligthning_001", spriteObject);
			tk2dSpriteAnimationFrame starterFrame = new tk2dSpriteAnimationFrame();
			starterFrame.spriteId = spriteObject.GetComponent<tk2dSprite>().spriteId;
			starterFrame.spriteCollection = spriteObject.GetComponent<tk2dSprite>().Collection;
			tk2dSpriteAnimationFrame[] frameArray = new tk2dSpriteAnimationFrame[]
			{
				starterFrame
			};
			animationClip.frames = frameArray;
			for (int i = 2; i < 8; i++)
			{
				GameObject spriteForObject = new GameObject("spriteForObject");
				ItemBuilder.AddSpriteToObject("spriteForObject", $"katmod/Resources/V3MiscItems/Lightning/ligthning_00{i}", spriteForObject);
				tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
				frame.spriteId = spriteForObject.GetComponent<tk2dBaseSprite>().spriteId;
				frame.spriteCollection = spriteForObject.GetComponent<tk2dBaseSprite>().Collection;
				animationClip.frames = animationClip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
			}
			animator.Library = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
			animator.Library.clips = new tk2dSpriteAnimationClip[] { animationClip };
			animator.DefaultClipId = animator.GetClipIdByName("start");
			animator.playAutomatically = true;
			lightningPrefab = lightning;
		}

        public override bool CanBeUsed(PlayerController user)
		{
			AIActor actor = user.CurrentRoom.GetNearestEnemy(Camera.main.ScreenToWorldPoint(Input.mousePosition), out float distance);
			if (actor && actor.healthHaver && actor.healthHaver.IsVulnerable && distance < 3)
            {
				return base.CanBeUsed(user);
            }
			return false;
        }

        public override void Update()
        {
			try
            {
	           base.Update();
				if (LastOwner != null  && LastOwner.CurrentRoom != null)
			    {
					AIActor actor = LastOwner.CurrentRoom.GetNearestEnemy(Camera.main.ScreenToWorldPoint(Input.mousePosition), out float distance);
					if (actor && actor.healthHaver && actor.healthHaver.IsVulnerable && distance < 3 && !IsOnCooldown)
					{
						if (actor != StoredEnemy || target == null)
				        {
							StoredEnemy = actor;
							if (RoyalCapacitator.target)
							{
								Destroy(RoyalCapacitator.target);
							}
							GameObject target = GameObject.Instantiate(targetPrefab, actor.specRigidbody.UnitCenter, Quaternion.identity, actor.transform);
							target.transform.position.WithZ(transform.position.z + 99999);
							target.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(StoredEnemy.CenterPosition, tk2dBaseSprite.Anchor.MiddleCenter);
							AkSoundEngine.PostEvent("Play_UI_menu_select_01", gameObject);
							RoyalCapacitator.target = target;
						}
					}
					else if (target)
					{
						Destroy(RoyalCapacitator.target);
					}
				}

			} catch (Exception exc)
            {
				exc.ToString().Log();
            }

		}

        protected override void DoEffect(PlayerController user)
        {
			try
            {
			    base.DoEffect(user);
				AIActor actor = user.CurrentRoom.GetNearestEnemy(Camera.main.ScreenToWorldPoint(Input.mousePosition), out _);
				if (actor && actor.healthHaver)
				{
					GameObject lightning = actor.PlayEffectOnActor(lightningPrefab, new Vector3(0f, -1f, 0f));
					lightning.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(StoredEnemy.CenterPosition, tk2dBaseSprite.Anchor.LowerCenter);
					lightning.transform.position.WithZ(transform.position.z + 99999);
					lightning.GetComponent<tk2dSpriteAnimator>().Play();
					StartCoroutine(DelayedKill(actor.healthHaver, lightning));
				}
			} catch (Exception error)
            {
				error.ToString().Log();
            }
		}

		private IEnumerator DelayedKill(HealthHaver haverofhealth, GameObject lightning)
        {
			yield return new WaitForSeconds(0.1f);
			if (haverofhealth)
			{
				haverofhealth.ApplyDamage(400, Vector2.zero, "THUNDERSTRUCK");
			}
			AkSoundEngine.PostEvent("Play_OBJ_lightning_flash_01", gameObject);
			yield return new WaitForSeconds(0.5f);
			if (lightning)
			{
				Destroy(lightning);
			}
			yield break;
		}

		private static GameObject target;

		private static GameObject targetPrefab;

		public static AIActor StoredEnemy;

		private static GameObject lightningPrefab;

	}
}