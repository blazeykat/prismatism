using System;
using System.Linq;
using Dungeonator;
using ItemAPI;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using MonoMod.RuntimeDetour;
using System.Collections;

namespace katmod
{
    class IncubusStartingActive : LabelablePlayerItem
    {
        public static void Init()
        {
            string itemName = "Road Train";
            string resourceName = "katmod/Resources/V3MiscItems/jarfullofsouls";
            GameObject obj = new GameObject(itemName);
            IncubusStartingActive item = obj.gameObject.AddComponent<IncubusStartingActive>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Haunting";
            string longDesc = "Duplicate every living enemy as a ghost, which fights for you.\n\nScreams and wailing emanate from the jar. Best not to think about it.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(558);
            item.SetCooldownType(ItemBuilder.CooldownType.Damage, 500);
            item.consumable = false;
            item.CanBeDropped = false;

            /*Projectile projectile2 = (PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0];
            Projectile proj1 = UnityEngine.Object.Instantiate<Projectile>(projectile2);
            {
                proj1.baseData.damage = 25;
                proj1.SetProjectileSpriteRight("incubus_placeholder_001", 44, 22);
                proj1.shouldRotate = true;
                proj1.gameObject.SetActive(false);
                proj1.baseData.speed = 7;
                UnityEngine.Object.DontDestroyOnLoad(proj1);
                FakePrefab.MarkAsFakePrefab(proj1.gameObject);
            }
            demonProjectile = proj1;*/
            Hook hook = new Hook(typeof(PlayerItem).GetMethod("Use", BindingFlags.Public | BindingFlags.Instance), typeof(IncubusStartingActive).GetMethod("OverrideOnUse"));
			Hook hook2 = new Hook(typeof(GameUIItemController).GetMethod("UpdateItemSprite", BindingFlags.Instance | BindingFlags.NonPublic), typeof(IncubusStartingActive).GetMethod("UpdateItemSprite"));
		}

        public static bool OverrideOnUse(Func<PlayerItem, PlayerController, float, bool> orig, PlayerItem self, PlayerController user, out float destroyTime)
        {
			Type itemType = typeof(PlayerItem);
			FieldInfo _isDestroyed = itemType.GetField("m_isDestroyed", BindingFlags.NonPublic | BindingFlags.Instance);
			destroyTime = -1f;
			if ((bool)_isDestroyed.GetValue(self))
			{
				return false;
			}
			if (!self.CanBeUsed(user))
			{
				return false;
			}
			MethodInfo _UseConsumableStack = itemType.GetMethod("UseConsumableStack", BindingFlags.NonPublic | BindingFlags.Instance);
			FieldInfo _baseSpriteID = itemType.GetField("m_baseSpriteID", BindingFlags.NonPublic | BindingFlags.Instance);
			if (self.IsCurrentlyActive)
			{
				MethodInfo _DoActiveEffect = itemType.GetMethod("DoActiveEffect", BindingFlags.NonPublic | BindingFlags.Instance);
				_DoActiveEffect.Invoke(self, new object[] { user });
				if (self.consumable && self.consumableOnActiveUse)
				{
					bool flag = (bool)_UseConsumableStack.Invoke(self, null);
					if (flag)
					{
						return true;
					}
				}
				if (!string.IsNullOrEmpty(self.OnActivatedSprite) && self.sprite.spriteId != (int)_baseSpriteID.GetValue(self))
				{
					self.sprite.SetSprite((int)_baseSpriteID.GetValue(self));
				}
				return false;
			}
			bool DoCooldown = true;
			if (self.IsOnCooldown && !(self is IncubusStartingActive && ((IncubusStartingActive)self).armour > 0))
			{
				ETGModConsole.Log($"failed the check, {((IncubusStartingActive)self).armour}");
				if (self is IncubusStartingActive)
                {
					ETGModConsole.Log("did it wrong");
                }
				if (((IncubusStartingActive)self).armour < 0) {
					ETGModConsole.Log("terrible");
                }
				MethodInfo _DoOnCooldownEffect = itemType.GetMethod("DoOnCooldownEffect", BindingFlags.NonPublic | BindingFlags.Instance);
				_DoOnCooldownEffect.Invoke(self, new object[] { user });
				if (self.consumable && self.consumableOnCooldownUse)
				{
					bool flag2 = (bool)_UseConsumableStack.Invoke(self, null);
					if (flag2)
					{
						return true;
					}
				}
				if (!string.IsNullOrEmpty(self.OnCooldownSprite) && self.sprite.spriteId != (int)_baseSpriteID.GetValue(self))
				{
					self.sprite.SetSprite((int)_baseSpriteID.GetValue(self));
				}
				return false;
			} else if (self.IsOnCooldown && self is IncubusStartingActive && ((IncubusStartingActive)self).armour > 0)
			{
				DoCooldown = false;
				ETGModConsole.Log("super organism");
				foreach (PassiveItem item in user.passiveItems)
				{
					if (item is IncubusStartingPassive passive)
					{
						passive.savedArmor -= 1;
						ETGModConsole.Log("automation");
					}
				}
			}
			MethodInfo _DoEffect = itemType.GetMethod("DoEffect", BindingFlags.NonPublic | BindingFlags.Instance);
			_DoEffect.Invoke(self, new object[] { user });
			if (!string.IsNullOrEmpty(self.useAnimation))
			{
				tk2dSpriteAnimationClip clipByName = self.spriteAnimator.GetClipByName(self.useAnimation);
				self.spriteAnimator.Play(clipByName);
				destroyTime = (float)clipByName.frames.Length / clipByName.fps;
			}
			if (self.consumable && !self.consumableOnCooldownUse && !self.consumableOnActiveUse)
			{
				bool flag3 = (bool)_UseConsumableStack.Invoke(self, null);
				if (self.consumableHandlesOwnDuration)
				{
					destroyTime = self.customDestroyTime;
				}
				if (flag3)
				{
					return true;
				}
			}
			else if (self.UsesNumberOfUsesBeforeCooldown)
			{
				self.numberOfUses--;
			}
			if (destroyTime >= 0f)
			{
				MethodInfo _HandleAnimationReset = itemType.GetMethod("HandleAnimationReset", BindingFlags.NonPublic | BindingFlags.Instance);
				self.StartCoroutine((IEnumerator)_HandleAnimationReset.Invoke(self, new object[] { destroyTime }));
			}
			if (!self.UsesNumberOfUsesBeforeCooldown || self.numberOfUses <= 0)
			{
				if (self.UsesNumberOfUsesBeforeCooldown)
				{
					FieldInfo _cachedNumberOfUses = itemType.GetField("m_cachedNumberOfUses", BindingFlags.NonPublic | BindingFlags.Instance);
					self.numberOfUses = (int)_cachedNumberOfUses.GetValue(self);
				}
				if (DoCooldown)
                {
					MethodInfo _ApplyCooldown = itemType.GetMethod("ApplyCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
					MethodInfo _AfterCooldownApplied = itemType.GetMethod("AfterCooldownApplied", BindingFlags.NonPublic | BindingFlags.Instance);
					_ApplyCooldown.Invoke(self, new object[] { user });
					_AfterCooldownApplied.Invoke(self, new object[] { user });
				}
			}
			return false;
		}

        //private static Projectile demonProjectile;

        protected override void DoEffect(PlayerController user)
		{ 
			base.DoEffect(user);
			if (user == null && user.specRigidbody == null)
            {
				return;
            }
			Projectile ghostProjectile = user.HandleProjectile(11, 20, 15, false, user.specRigidbody.UnitCenter, true, user.CurrentGun != null ? user.CurrentGun.CurrentAngle + UnityEngine.Random.Range(-90f, 90f) : 0, 9);
			HomingModifier homing = ghostProjectile.gameObject.AddComponent<HomingModifier>();
			homing.HomingRadius = float.MaxValue;
			homing.AngularVelocity = 180;
			ghostProjectile.OnDestruction += delegate (Projectile spooky)
			{
				GameObject silencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
				AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", base.gameObject);
				GameObject gameObject = new GameObject("silencer");
				SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
				silencerInstance.TriggerSilencer(spooky.specRigidbody.UnitCenter, 20f, 3, silencerVFX, 0f, 3f, 3f, 3f, 30f, 3f, 0.25f, user, false, false);
			};
		}

		public static void UpdateItemSprite(GameUIItemController self, PlayerItem newItem, int itemShift)
		{
			tk2dSprite component = newItem.GetComponent<tk2dSprite>();
			Type type = typeof(GameUIItemController);
			FieldInfo _cachedItem = type.GetField("m_cachedItem", BindingFlags.NonPublic | BindingFlags.Instance);
			if (newItem != (PlayerItem)_cachedItem.GetValue(self))
			{
				MethodInfo _DoItemCardFlip = type.GetMethod("DoItemCardFlip", BindingFlags.NonPublic | BindingFlags.Instance);
				_DoItemCardFlip.Invoke(self, new object[] { newItem, itemShift });
			}
			MethodInfo _UpdateItemSpriteScale = type.GetMethod("UpdateItemSpriteScale", BindingFlags.NonPublic | BindingFlags.Instance);
			_UpdateItemSpriteScale.Invoke(self, null);
			FieldInfo _deferCurrentItemSwap = type.GetField("m_deferCurrentItemSwap", BindingFlags.NonPublic | BindingFlags.Instance);
			if (!(bool)_deferCurrentItemSwap.GetValue(self))
			{
				if (!self.itemSprite.renderer.enabled)
				{
					self.ToggleRenderers(true);
				}
				if (self.itemSprite.spriteId != component.spriteId || self.itemSprite.Collection != component.Collection)
				{
					FieldInfo _outlineSprites = type.GetField("outlineSprites", BindingFlags.NonPublic | BindingFlags.Instance);
					self.itemSprite.SetSprite(component.Collection, component.spriteId);
					tk2dSprite[] outlineSprites = ((tk2dSprite[])_outlineSprites.GetValue(self));
					for (int i = 0; i < outlineSprites.Length; i++)
					{
						outlineSprites[i].SetSprite(component.Collection, component.spriteId);
						SpriteOutlineManager.ForceUpdateOutlineMaterial(outlineSprites[i], component);
					}
				}
			}
			Vector3 center = self.ItemBoxSprite.GetCenter();
			FieldInfo _isCurrentlyFlipping = type.GetField("m_isCurrentlyFlipping", BindingFlags.NonPublic | BindingFlags.Instance);
			bool m_isCurrentlyFlipping = (bool)_isCurrentlyFlipping.GetValue(self);
			self.itemSprite.transform.position = center + self.GetOffsetVectorForItem(newItem, m_isCurrentlyFlipping);
			self.itemSprite.transform.position = self.itemSprite.transform.position.Quantize(self.ItemBoxSprite.PixelsToUnits() * 3f);
			if (newItem.PreventCooldownBar || (!newItem.IsActive && !newItem.IsOnCooldown) || m_isCurrentlyFlipping)
			{
				self.ItemBoxFillSprite.IsVisible = false;
				self.ItemBoxFGSprite.IsVisible = false;
				self.ItemBoxSprite.SpriteName = "weapon_box_02";
			}
			else
			{
				self.ItemBoxFillSprite.IsVisible = true;
				self.ItemBoxFGSprite.IsVisible = true;
				self.ItemBoxSprite.SpriteName = "weapon_box_02_cd";
			}
			if (newItem.IsActive)
			{
				self.ItemBoxFillSprite.FillAmount = 1f - newItem.ActivePercentage;
			}
			else
			{
				self.ItemBoxFillSprite.FillAmount = 1f - newItem.CooldownPercentage;
			}
			PlayerController user = GameManager.Instance.PrimaryPlayer;
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && self.IsRightAligned)
			{
				user = GameManager.Instance.SecondaryPlayer;
			}
			FieldInfo _itemSpriteMaterial = type.GetField("itemSpriteMaterial", BindingFlags.NonPublic | BindingFlags.Instance);
			Material itemSpriteMaterial = (Material)_itemSpriteMaterial.GetValue(self);
			if ((newItem.IsOnCooldown && !(newItem is IncubusStartingActive && ((IncubusStartingActive)newItem).armour > 0)) || !newItem.CanBeUsed(user))
			{
				Color color = itemSpriteMaterial.GetColor("_OverrideColor");
				Color color2 = new Color(0f, 0f, 0f, 0.8f);
				if (color != color2)
				{

					itemSpriteMaterial.SetColor("_OverrideColor", color2);
					tk2dSprite[] array = SpriteOutlineManager.GetOutlineSprites(self.itemSprite);
					Color value = new Color(0.4f, 0.4f, 0.4f, 1f);
					for (int j = 0; j < array.Length; j++)
					{
						array[j].renderer.material.SetColor("_OverrideColor", value);
					}
				}
			}
			else
			{
				Color color3 = itemSpriteMaterial.GetColor("_OverrideColor");
				Color color4 = new Color(0f, 0f, 0f, 0f);
				if (color3 != color4)
				{
					itemSpriteMaterial.SetColor("_OverrideColor", color4);
					tk2dSprite[] array2 = SpriteOutlineManager.GetOutlineSprites(self.itemSprite);
					Color white = Color.white;
					for (int k = 0; k < array2.Length; k++)
					{
						array2[k].renderer.material.SetColor("_OverrideColor", white);
					}
				}
			}
		}

		public float armour;
    }
}
