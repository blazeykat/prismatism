using katmod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ItemAPI
{
    public static class Utilities
    {
        public static void PlaceItemInAmmonomiconAfterItemById(this PickupObject item, int id) //from SpAPI's items
        {
            item.ForcedPositionInAmmonomicon = PickupObjectDatabase.GetById(id).ForcedPositionInAmmonomicon;
        }

		public static void SetupUnlockOnFlag(this PickupObject self, GungeonFlags flag, bool requiredFlagValue) //thanks SpAPI
		{
			EncounterTrackable encounterTrackable = self.encounterTrackable;
			bool flag2 = encounterTrackable.prerequisites == null;
			if (flag2)
			{
				encounterTrackable.prerequisites = new DungeonPrerequisite[1];
				encounterTrackable.prerequisites[0] = new DungeonPrerequisite
				{
					prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
					requireFlag = requiredFlagValue,
					saveFlagToCheck = flag
				};
			}
			else
			{
				encounterTrackable.prerequisites = encounterTrackable.prerequisites.Concat(new DungeonPrerequisite[]
				{
					new DungeonPrerequisite
					{
						prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
						requireFlag = requiredFlagValue,
						saveFlagToCheck = flag
					}
				}).ToArray<DungeonPrerequisite>();
			}
			EncounterDatabaseEntry entry = EncounterDatabase.GetEntry(encounterTrackable.EncounterGuid);
			bool flag3 = !string.IsNullOrEmpty(entry.ProxyEncounterGuid);
			if (flag3)
			{
				entry.ProxyEncounterGuid = "";
			}
			bool flag4 = entry.prerequisites == null;
			if (flag4)
			{
				entry.prerequisites = new DungeonPrerequisite[1];
				entry.prerequisites[0] = new DungeonPrerequisite
				{
					prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
					requireFlag = requiredFlagValue,
					saveFlagToCheck = flag
				};
			}
			else
			{
				entry.prerequisites = entry.prerequisites.Concat(new DungeonPrerequisite[]
				{
					new DungeonPrerequisite
					{
						prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
						requireFlag = requiredFlagValue,
						saveFlagToCheck = flag
					}
				}).ToArray<DungeonPrerequisite>();
			}
		}

		public static void SetupUnlockOnStat(this PickupObject self, TrackedStats stat, DungeonPrerequisite.PrerequisiteOperation operation, int comparisonValue)
		{
			EncounterTrackable encounterTrackable = self.encounterTrackable;
			bool flag = encounterTrackable.prerequisites == null;
			if (flag)
			{
				encounterTrackable.prerequisites = new DungeonPrerequisite[1];
				encounterTrackable.prerequisites[0] = new DungeonPrerequisite
				{
					prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
					prerequisiteOperation = operation,
					statToCheck = stat,
					comparisonValue = (float)comparisonValue
				};
			}
			else
			{
				encounterTrackable.prerequisites = encounterTrackable.prerequisites.Concat(new DungeonPrerequisite[]
				{
					new DungeonPrerequisite
					{
						prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
						prerequisiteOperation = operation,
						statToCheck = stat,
						comparisonValue = (float)comparisonValue
					}
				}).ToArray<DungeonPrerequisite>();
			}
			EncounterDatabaseEntry entry = EncounterDatabase.GetEntry(encounterTrackable.EncounterGuid);
			bool flag2 = !string.IsNullOrEmpty(entry.ProxyEncounterGuid);
			if (flag2)
			{
				entry.ProxyEncounterGuid = "";
			}
			bool flag3 = entry.prerequisites == null;
			if (flag3)
			{
				entry.prerequisites = new DungeonPrerequisite[1];
				entry.prerequisites[0] = new DungeonPrerequisite
				{
					prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
					prerequisiteOperation = operation,
					statToCheck = stat,
					comparisonValue = (float)comparisonValue
				};
			}
			else
			{
				entry.prerequisites = entry.prerequisites.Concat(new DungeonPrerequisite[]
				{
					new DungeonPrerequisite
					{
						prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
						prerequisiteOperation = operation,
						statToCheck = stat,
						comparisonValue = (float)comparisonValue
					}
				}).ToArray<DungeonPrerequisite>();
			}
		}

		public static void Kill(this HealthHaver health)
        {
			health.ApplyDamage(1E+07f, Vector2.zero, "dying, lmao", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
		}

		/// <summary>
		/// unstoppable and imminent death lol
		/// </summary>
		public static void Obliterate(this HealthHaver health)
		{
			health.ApplyDamage(1E+07f, Vector2.zero, "dying, lmao", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, true);
		}

		public static Projectile HandleProjectile(this PlayerController player, float speed, float damage, int ProjectileID)
        {
			Projectile projectile2 = ((Gun)ETGMod.Databases.Items[ProjectileID]).DefaultModule.projectiles[0];
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, player.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (player.CurrentGun == null) ? 0f : player.CurrentGun.CurrentAngle), true);
			Projectile component = gameObject.GetComponent<Projectile>();
			bool componentless = component != null;
			if (componentless)
			{
				component.Owner = player;
				component.Shooter = player.specRigidbody;
				component.baseData.speed = speed;
				component.baseData.damage = damage;
			}
			return component;
		}

		public static Projectile HandleChargeProjectile(this PlayerController player, float speed, float damage, int ProjectileID)
		{
			Projectile projectile2 = ((Gun)ETGMod.Databases.Items[ProjectileID]).DefaultModule.chargeProjectiles[0].Projectile;
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, player.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (player.CurrentGun == null) ? 0f : player.CurrentGun.CurrentAngle), true);
			Projectile component = gameObject.GetComponent<Projectile>();
			bool componentless = component != null;
			if (componentless)
			{
				component.Owner = player;
				component.Shooter = player.specRigidbody;
				component.baseData.speed = speed;
				component.baseData.damage = damage;
			}
			return component;
		}

		public static Projectile HandleProjectileAimed(this PlayerController player, float speed, float damage, int ProjectileID, float aim, float range)
		{
			Projectile projectile2 = ((Gun)ETGMod.Databases.Items[ProjectileID]).DefaultModule.projectiles[0];
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, player.sprite.WorldCenter, Quaternion.Euler(0f, 0f, aim), true);
			Projectile component = gameObject.GetComponent<Projectile>();
			bool componentless = component != null;
			if (componentless)
			{
				component.Owner = player;
				component.Shooter = player.specRigidbody;
				component.baseData.speed = speed;
				component.baseData.damage = damage;
				component.baseData.range = range;
			}
			return component;
		}

		public static Projectile HandleChargedProjectileAimed(this PlayerController player, float speed, float damage, int ProjectileID, float aim, float range)
		{
			Projectile projectile2 = ((Gun)ETGMod.Databases.Items[ProjectileID]).DefaultModule.chargeProjectiles[0].Projectile;
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, player.sprite.WorldCenter, Quaternion.Euler(0f, 0f, aim), true);
			Projectile component = gameObject.GetComponent<Projectile>();
			bool componentless = component != null;
			if (componentless)
			{
				component.Owner = player;
				component.Shooter = player.specRigidbody;
				component.baseData.speed = speed;
				component.baseData.damage = damage;
				component.baseData.range = range;
			}
			return component;
		}

		/// <summary>
		/// The basic random value, with coolness and stuff
		/// </summary>
		public static bool BasicRandom(PlayerController player, float value, float divider)
        {
			if (UnityEngine.Random.value > value - (player.stats.GetStatValue(PlayerStats.StatType.Coolness) / divider))
            {
				return true;
            } else
            {
				return false;
            }
        }
		public static bool UncoolRandom(float value)
		{
			if (UnityEngine.Random.value > value)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static AIActor SummonAtRandomPosition(string guid, PlayerController owner)
        {
			AIActor aiactor = AIActor.Spawn(EnemyDatabase.GetOrLoadByGuid(guid), new IntVector2?(owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1)).Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(new IntVector2?(owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1)).Value), true, AIActor.AwakenAnimationType.Awaken, true);
			return aiactor;
		}

		/// <summary>
		/// Ask neighborino when this goes wrong
		/// </summary>
		public static void SetProjectileSpriteRight(this Projectile proj, string name, int pixelWidth, int pixelHeight, int? overrideColliderPixelWidth = null, int? overrideColliderPixelHeight = null)
		{
			try
			{
				bool flag = overrideColliderPixelWidth == null;
				if (flag)
				{
					overrideColliderPixelWidth = new int?(pixelWidth);
				}
				bool flag2 = overrideColliderPixelHeight == null;
				if (flag2)
				{
					overrideColliderPixelHeight = new int?(pixelHeight);
				}
				float num = (float)pixelWidth / 16f;
				float num2 = (float)pixelHeight / 16f;
				float x = (float)overrideColliderPixelWidth.Value / 16f;
				float y = (float)overrideColliderPixelHeight.Value / 16f;
				proj.GetAnySprite().spriteId = ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteIdByName(name);
				tk2dSpriteDefinition tk2dSpriteDefinition = ETGMod.Databases.Items.ProjectileCollection.inst.spriteDefinitions[(PickupObjectDatabase.GetById(12) as Gun).DefaultModule.projectiles[0].GetAnySprite().spriteId].CopyDefinitionFrom();
				tk2dSpriteDefinition.boundsDataCenter = new Vector3(num / 2f, num2 / 2f, 0f);
				tk2dSpriteDefinition.boundsDataExtents = new Vector3(num, num2, 0f);
				tk2dSpriteDefinition.untrimmedBoundsDataCenter = new Vector3(num / 2f, num2 / 2f, 0f);
				tk2dSpriteDefinition.untrimmedBoundsDataExtents = new Vector3(num, num2, 0f);
				tk2dSpriteDefinition.position0 = new Vector3(0f, 0f, 0f);
				tk2dSpriteDefinition.position1 = new Vector3(0f + num, 0f, 0f);
				tk2dSpriteDefinition.position2 = new Vector3(0f, 0f + num2, 0f);
				tk2dSpriteDefinition.position3 = new Vector3(0f + num, 0f + num2, 0f);
				tk2dSpriteDefinition.colliderVertices[1].x = x;
				tk2dSpriteDefinition.colliderVertices[1].y = y;
				tk2dSpriteDefinition.name = name;
				ETGMod.Databases.Items.ProjectileCollection.inst.spriteDefinitions[proj.GetAnySprite().spriteId] = tk2dSpriteDefinition;
				proj.baseData.force = 0f;
			}
			catch (Exception ex)
			{
				ETGModConsole.Log("Ooops! Seems like something got very, Very, VERY wrong. Here's the exception:", false);
				ETGModConsole.Log(ex.ToString(), false);
			}
		}

		public static tk2dSpriteDefinition CopyDefinitionFrom(this tk2dSpriteDefinition other)
		{
			tk2dSpriteDefinition result = new tk2dSpriteDefinition
			{
				boundsDataCenter = new Vector3
				{
					x = other.boundsDataCenter.x,
					y = other.boundsDataCenter.y,
					z = other.boundsDataCenter.z
				},
				boundsDataExtents = new Vector3
				{
					x = other.boundsDataExtents.x,
					y = other.boundsDataExtents.y,
					z = other.boundsDataExtents.z
				},
				colliderConvex = other.colliderConvex,
				colliderSmoothSphereCollisions = other.colliderSmoothSphereCollisions,
				colliderType = other.colliderType,
				colliderVertices = other.colliderVertices,
				collisionLayer = other.collisionLayer,
				complexGeometry = other.complexGeometry,
				extractRegion = other.extractRegion,
				flipped = other.flipped,
				indices = other.indices,
				material = new Material(other.material),
				materialId = other.materialId,
				materialInst = new Material(other.materialInst),
				metadata = other.metadata,
				name = other.name,
				normals = other.normals,
				physicsEngine = other.physicsEngine,
				position0 = new Vector3
				{
					x = other.position0.x,
					y = other.position0.y,
					z = other.position0.z
				},
				position1 = new Vector3
				{
					x = other.position1.x,
					y = other.position1.y,
					z = other.position1.z
				},
				position2 = new Vector3
				{
					x = other.position2.x,
					y = other.position2.y,
					z = other.position2.z
				},
				position3 = new Vector3
				{
					x = other.position3.x,
					y = other.position3.y,
					z = other.position3.z
				},
				regionH = other.regionH,
				regionW = other.regionW,
				regionX = other.regionX,
				regionY = other.regionY,
				tangents = other.tangents,
				texelSize = new Vector2
				{
					x = other.texelSize.x,
					y = other.texelSize.y
				},
				untrimmedBoundsDataCenter = new Vector3
				{
					x = other.untrimmedBoundsDataCenter.x,
					y = other.untrimmedBoundsDataCenter.y,
					z = other.untrimmedBoundsDataCenter.z
				},
				untrimmedBoundsDataExtents = new Vector3
				{
					x = other.untrimmedBoundsDataExtents.x,
					y = other.untrimmedBoundsDataExtents.y,
					z = other.untrimmedBoundsDataExtents.z
				}
			};
			List<Vector2> uvs = new List<Vector2>();
			foreach (Vector2 vector in other.uvs)
			{
				uvs.Add(new Vector2
				{
					x = vector.x,
					y = vector.y
				});
			}
			result.uvs = uvs.ToArray();
			List<Vector3> colliderVertices = new List<Vector3>();
			foreach (Vector3 vector in other.colliderVertices)
			{
				colliderVertices.Add(new Vector3
				{
					x = vector.x,
					y = vector.y,
					z = vector.z
				});
			}
			result.colliderVertices = colliderVertices.ToArray();
			return result;
		}

		public static HomingModifier AddHoming(this Projectile projecto, float radius = 360, float angle = 270)
        {
			HomingModifier h = projecto.gameObject.AddComponent<HomingModifier>();
			h.HomingRadius = radius;
			h.AngularVelocity = angle;
			return h;
        }

		/// <summary>
		/// Increases damage, makes it white, does the projectile stuff
		/// </summary>
		public static void MakeCrit(this Projectile projec, PlayerController player)
        {
			projec.baseData.damage *= 1.6f;
			projec.DefaultTintColor = Color.white;
			projec.HasDefaultTint = true;
			DoProjectile(player);
		}

		/// <summary>
		/// Handles the projectiles for all critting stuff
		/// </summary>
		public static void DoProjectile(PlayerController player)
		{
			if (player.HasPassiveItem(SaplingBullets.ID))
			{
				Projectile component = player.HandleChargeProjectile(20f, 7f, 620);
				component.AddHoming();
			}
			if (player.HasPassiveItem(CatSnack.ID))
			{
				if (Utilities.UncoolRandom(0.97f))
				{
					player.HandleChargeProjectile(25f, 95f, 359);
				}
				else
				{
					player.HandleProjectile(20f, (player.HasGun(7)) ? 20 : 10, 7);
				}
			}
		}

		public static PassiveItem GetRandomPassive()
        {
			System.Random rando = new System.Random();
			PassiveItem.ItemQuality quality = GetQualityFromChances(0.3f, 0.25f, 0.2f, 0.15f, 0.1f);
			return (PassiveItem)LootEngine.GetItemOfTypeAndQuality<PickupObject>(quality, GameManager.Instance.RewardManager.ItemsLootTable, false);
        }

		public static PickupObject.ItemQuality GetQualityFromChances(float dChance, float cChance, float bChance, float aChance, float sChance)
		{
			float num = UnityEngine.Random.value;
			if (num < dChance)
			{
				return PickupObject.ItemQuality.D;
			}
			if (num < dChance + cChance)
			{
				return PickupObject.ItemQuality.C;
			}
			if (num < dChance + cChance + bChance)
			{
				return PickupObject.ItemQuality.B;
			}
			if (num < dChance + cChance + bChance + aChance)
			{
				return PickupObject.ItemQuality.A;
			}
			return PickupObject.ItemQuality.S;
		}
	}
}
