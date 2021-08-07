using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class BlueAlbum : PassiveItem
    {
        public static void Init()
        {
            string name = "Weezer";
            string resourcePath = "katmod/Resources/V3MiscItems/weezer2.png";
            GameObject gameObject = new GameObject(name);
            BlueAlbum item = gameObject.AddComponent<BlueAlbum>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Undone";
            string longDesc = "Unweaves your enemies, increasing damage done.\n\nThe vinyl of a masterpiece.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(119);
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.WEEZER_FLAG, true);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Buy it");
            item.AddItemToDougMetaShop(20);

            GameObject vfx = new GameObject("weezer_ui");
            ItemBuilder.AddSpriteToObject("weezer_ui", "katmod/Resources/V3MiscItems/ifyouwanttodestroymysweater", vfx);
            vfxPrefab = vfx;

            weezer = new GameActorUnweaveEffect()
            {
                duration = 15,
                OverheadVFX = vfxPrefab,
                effectIdentifier = "weezer",
                initalAmount = .012f,
                increments = .08f,
                maxAmount = 4,
                counter = new CountingComponent
                {
                    TimesApplied = 0
                }
            };
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            try
            {
                Projectile projectile = sourceBeam.projectile;
                projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.OnHitEnemy));
            }
            catch (Exception ex)
            {
                ETGModConsole.Log(ex.Message, false);
            }
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.OnHitEnemy));
            }
            catch (Exception ex)
            {
                global::ETGModConsole.Log(ex.Message, false);
            }
        }
        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            try
            {
            if (arg2 != null && arg2.aiActor != null && Owner != null)
            {
                if (BoxOTools.BasicRandom(.5f))
                {
                    if (arg2.aiActor.GetEffect("weezer") != null)
                    {
                        GameActorUnweaveEffect unweave = arg2.aiActor.GetEffect("weezer") as GameActorUnweaveEffect;
                        unweave.duration += 5;
                        unweave.IncreaseIfAmountNotHit(arg2.aiActor);
                    } else
                    {
                        GameActorUnweaveEffect unweave = weezer;
                        arg2.aiActor.ApplyEffect(unweave);
                    }
                }
                }

            } catch (Exception eror)
            {
                eror.ToString().Log();
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessBeam -= this.PostProcessBeam;
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return debrisObject;
        }

        public static GameActorUnweaveEffect weezer = new GameActorUnweaveEffect();

        public static GameObject vfxPrefab;
    }
}
