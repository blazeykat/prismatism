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
			string resourcePath = "katmod/Resources/V3MiscItems/capacitor";
			GameObject gameObject = new GameObject(name);
			RoyalCapacitator item = gameObject.AddComponent<RoyalCapacitator>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "THUNDERSTRUCK";
			string longDesc = "Channel the heavens.\n\nA Calorum Powerlytic 3600DE capacitor, which has been refitted for combat.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = ItemQuality.A;
			item.PlaceItemInAmmonomiconAfterItemById(439);
			item.SetupUnlockOnFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_FORGE, true);
			Prismatism.Unlocks.Add(item.PickupObjectId, "Master the forge's boss");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 750f);

			GameObject target = new GameObject("capacitor_ui");
			ItemBuilder.AddSpriteToObject("capacitor_ui", "katmod/Resources/V3MiscItems/royalcapacitatorui", target);
            targetPrefab = target;
			GameObject lightning = new GameObject("capacitor_lightning");
			ItemBuilder.AddSpriteToObject("capacitor_lightning", "katmod/Resources/V3MiscItems/Lightning/ligthning_001", lightning);
			FakePrefab.MarkAsFakePrefab(lightning);
			UnityEngine.Object.DontDestroyOnLoad(lightning);
			tk2dSpriteAnimator animator = lightning.AddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimationClip animationClip = new tk2dSpriteAnimationClip
            {
                fps = 14,
                wrapMode = tk2dSpriteAnimationClip.WrapMode.Once
            };
            GameObject spriteObject = new GameObject("spriteObject");
			ItemBuilder.AddSpriteToObject("spriteObject", "katmod/Resources/V3MiscItems/Lightning/ligthning_001", spriteObject);
            tk2dSpriteAnimationFrame starterFrame = new tk2dSpriteAnimationFrame
            {
                spriteId = spriteObject.GetComponent<tk2dSprite>().spriteId,
                spriteCollection = spriteObject.GetComponent<tk2dSprite>().Collection
            };
            tk2dSpriteAnimationFrame[] frameArray = new tk2dSpriteAnimationFrame[]
			{
				starterFrame
			};
			animationClip.frames = frameArray;
			for (int i = 2; i < 8; i++)
			{
				GameObject spriteForObject = new GameObject("spriteForObject");
				ItemBuilder.AddSpriteToObject("spriteForObject", $"katmod/Resources/V3MiscItems/Lightning/ligthning_00{i}", spriteForObject);
                tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame
                {
                    spriteId = spriteForObject.GetComponent<tk2dBaseSprite>().spriteId,
                    spriteCollection = spriteForObject.GetComponent<tk2dBaseSprite>().Collection
                };
                animationClip.frames = animationClip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
			}
			animator.Library = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
			animationClip.name = "start";
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
					if (actor && actor.healthHaver && actor.healthHaver.IsVulnerable && distance < 3 && !IsOnCooldown && actor.sprite)
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
							actor.sprite.AttachRenderer(target.GetComponent<tk2dBaseSprite>());
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
					GameObject lightning = UnityEngine.Object.Instantiate(lightningPrefab, actor.transform);
					lightning.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject();
					lightning.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(StoredEnemy.CenterPosition, tk2dBaseSprite.Anchor.LowerCenter);
					lightning.transform.position.WithZ(transform.position.z + 99999);
					DaveAPIManager.SetFlag(CustomDungeonFlags.ENEMY_STRUCK_WITH_LIGHTNING, true);
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
				haverofhealth.ApplyDamage(400 * LastOwner.stats.GetStatValue(PlayerStats.StatType.Damage), Vector2.zero, "THUNDERSTRUCK");
			}
			AkSoundEngine.PostEvent("Play_OBJ_lightning_flash_01", gameObject);
			yield break;
		}

		private static GameObject target;

		private static GameObject targetPrefab;

		public static AIActor StoredEnemy;

		private static GameObject lightningPrefab;

	}
}