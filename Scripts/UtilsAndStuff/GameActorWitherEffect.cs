using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace katmod
{
    class GameActorWitherEffect : GameActorHealthEffect
	{
		public GameActorWitherEffect()
		{
			this.flameNumPerSquareUnit = 10;
			this.flameBuffer = new Vector2(0.0625f, 0.0625f);
			this.flameFpsVariation = 0.5f;
			this.flameMoveChance = 0.2f;
			effectIdentifier = "wither";
		}
		public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			base.EffectTick(actor, effectData);
			if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH && effectData.actor && effectData.actor.specRigidbody.HitboxPixelCollider != null)
			{
				Vector2 unitBottomLeft = effectData.actor.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
				Vector2 unitTopRight = effectData.actor.specRigidbody.HitboxPixelCollider.UnitTopRight;
				this.m_emberCounter += 30f * BraveTime.DeltaTime;
				if (this.m_emberCounter > 1f)
				{
					int num = Mathf.FloorToInt(this.m_emberCounter);
					this.m_emberCounter -= (float)num;
					GlobalSparksDoer.DoRandomParticleBurst(num, unitBottomLeft, unitTopRight, new Vector3(1f, 1f, 0f), 120f, 0.75f, null, null, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
				}
			}
			if (actor && actor.specRigidbody)
			{
				Vector2 unitDimensions = actor.specRigidbody.HitboxPixelCollider.UnitDimensions;
				Vector2 a = unitDimensions / 2f;
				int num2 = Mathf.RoundToInt((float)this.flameNumPerSquareUnit * 0.5f * Mathf.Min(30f, Mathf.Min(new float[]
				{
				unitDimensions.x * unitDimensions.y
				})));
				this.m_particleTimer += BraveTime.DeltaTime * (float)num2;
				if (this.m_particleTimer > 1f)
				{
					int num3 = Mathf.FloorToInt(this.m_particleTimer);
					Vector2 vector = actor.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
					Vector2 vector2 = actor.specRigidbody.HitboxPixelCollider.UnitTopRight;
					PixelCollider pixelCollider = actor.specRigidbody.GetPixelCollider(ColliderType.Ground);
					if (pixelCollider != null && pixelCollider.ColliderGenerationMode == PixelCollider.PixelColliderGeneration.Manual)
					{
						vector = Vector2.Min(vector, pixelCollider.UnitBottomLeft);
						vector2 = Vector2.Max(vector2, pixelCollider.UnitTopRight);
					}
					vector += Vector2.Min(a * 0.15f, new Vector2(0.25f, 0.25f));
					vector2 -= Vector2.Min(a * 0.15f, new Vector2(0.25f, 0.25f));
					vector2.y -= Mathf.Min(a.y * 0.1f, 0.1f);
					GlobalSparksDoer.DoRandomParticleBurst(num3, vector, vector2, Vector3.zero, 0f, 0f, null, null, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
					this.m_particleTimer -= Mathf.Floor(this.m_particleTimer);
				}
			}
			if (actor.IsGone)
			{
				effectData.elapsed = 10000f;
			}
			if ((actor.IsFalling || actor.IsGone) && effectData.vfxObjects != null && effectData.vfxObjects.Count > 0)
			{
				GameActorFireEffect.DestroyFlames(effectData);
			}
		}

		public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f)
		{
			base.OnEffectApplied(actor, effectData, partialAmount);
			effectData.OnActorPreDeath = delegate (Vector2 dir)
			{
				GameActorWitherEffect.DestroyFlames(effectData);
			};
			actor.healthHaver.OnPreDeath += effectData.OnActorPreDeath;
			{
				if (effectData.vfxObjects == null)
				{
					effectData.vfxObjects = new List<Tuple<GameObject, float>>();
				}
				effectData.OnFlameAnimationCompleted = delegate (tk2dSpriteAnimator spriteAnimator, tk2dSpriteAnimationClip clip)
				{
					if (effectData.destroyVfx || !actor)
					{
						spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(spriteAnimator.AnimationCompleted, effectData.OnFlameAnimationCompleted);
						UnityEngine.Object.Destroy(spriteAnimator.gameObject);
						return;
					}
					if (UnityEngine.Random.value < this.flameMoveChance)
					{
						Vector2 a = actor.specRigidbody.HitboxPixelCollider.UnitDimensions / 2f;
						Vector2 b = BraveUtility.RandomVector2(-a + this.flameBuffer, a - this.flameBuffer);
						Vector2 v = actor.specRigidbody.HitboxPixelCollider.UnitCenter + b;
						spriteAnimator.transform.position = v;
					}
					spriteAnimator.Play(clip, 0f, clip.fps * UnityEngine.Random.Range(1f - this.flameFpsVariation, 1f + this.flameFpsVariation), false);
				};
			}
		}

		public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			base.OnEffectRemoved(actor, effectData);
			actor.healthHaver.OnPreDeath -= effectData.OnActorPreDeath;
			GameActorFireEffect.DestroyFlames(effectData);
		}
		
		public static void DestroyFlames(RuntimeGameActorEffectData effectData)
		{
			if (effectData.vfxObjects == null)
			{
				return;
			}
			if (!effectData.actor.IsFrozen)
			{
				for (int i = 0; i < effectData.vfxObjects.Count; i++)
				{
					GameObject first = effectData.vfxObjects[i].First;
					if (first)
					{
						first.transform.parent = SpawnManager.Instance.VFX;
					}
				}
			}
			effectData.vfxObjects.Clear();
			effectData.destroyVfx = true;
			if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH && effectData.actor && effectData.actor.healthHaver && effectData.actor.healthHaver.GetCurrentHealth() <= 0f && effectData.actor.specRigidbody.HitboxPixelCollider != null)
			{
				Vector2 unitBottomLeft = effectData.actor.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
				Vector2 unitTopRight = effectData.actor.specRigidbody.HitboxPixelCollider.UnitTopRight;
				float num = (unitTopRight.x - unitBottomLeft.x) * (unitTopRight.y - unitBottomLeft.y);
				GlobalSparksDoer.DoRandomParticleBurst(Mathf.Max(1, (int)(75f * num)), unitBottomLeft, unitTopRight, new Vector3(1f, 1f, 0f), 120f, 0.75f, null, null, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
			}
		}

		public const float BossMinResistance = 0.25f;

		public const float BossMaxResistance = 0.75f;

		public const float BossResistanceDelta = 0.025f;

		public int flameNumPerSquareUnit;

		public Vector2 flameBuffer;

		public float flameFpsVariation;

		public float flameMoveChance;

		private float m_particleTimer;

		private float m_emberCounter;
	}
}
