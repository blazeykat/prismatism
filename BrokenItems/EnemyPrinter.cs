using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using Gungeon;
using UnityEngine;

namespace katmod
{
    class EnemyPrinter : PassiveItem
    {
        public static void Init()
        {
            string name = "Enemy Printer";
            string resourcePath = "katmod/Resources/WyrmSet/spogrechamp.png";
            //grabs texture for enemy (most likely furry porn)
            GameObject gameObject = new GameObject(name);
            //creates said object (again, probably furry porn)
            EnemyPrinter item = gameObject.AddComponent<EnemyPrinter>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "testinator";
            //describes the item accurately (testicle terminator)
            string longDesc = "prints guids of enemies in the console\n\ngo die.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.EXCLUDED;
            //describes the item quality (this item was created in china)
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            try
            {
                Projectile projectile = sourceBeam.projectile;
                //holy shit is that source? valve source engine? gaben from valve source gmod half life 3?? csgo rush b comrade terrorist??
                projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.OnHitEnemy));
                //combine from half life?? half life reference?? steam valve gaben artifact card game?? half life episode 3 when??
            }
            catch (Exception ex)
            //catch deez nuts
            {
                global::ETGModConsole.Log(ex.Message, false);
                //https://preview.redd.it/jmy1dl88cvf71.gif?width=220&format=mp4&s=c413637ac94b5b276878649904adcabcf32b934f
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
                //i have no idea what this does but it probably prints child porn on the screen
            }
        }
        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        //makes hit effects spawn like the ones that appear when i hit my wife
        {
            if (arg2 != null && arg2.aiActor != null && Owner != null) //we all know ai can't act shut up
            {
                ETGModConsole.Log(arg2.aiActor.EnemyGuid);
            }
        }

        public override void Pickup(PlayerController player) //unsure whether this is an xbox or playstation controller
        {
            base.Pickup(player); //your titties look heavy, let me pick them up for you
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            //makes the player spawn suspicious white fluid
            try
            {
                player.PostProcessBeam -= this.PostProcessBeam;
                player.PostProcessProjectile -= this.PostProcessProjectile;
            }
            catch (Exception ex)
            {
                ETGModConsole.Log($"damn,\n {ex}");
            }
            debrisObject.GetComponent<EnemyPrinter>().m_pickedUpThisRun = true;
            return debrisObject; //returns the suspicious white fluid to player
        }

        public AIActorDebuffEffect EnemyDebuff = new AIActorDebuffEffect //debuffs the player's fertility as punishment for playing this terrible game
        {
            HealthMultiplier = 0.7f, //decreases health because fuck you that's why
            CooldownMultiplier = 0.5f,
            OverheadVFX = overheadder,
            duration = 10, //holy shit i love fucking 10 year olds as well
        };

        public static UnityEngine.GameObject overheadder = ResourceCache.Acquire("Global VFX/VFX_Debuff_Status") as GameObject; //god i hate unity engine
    }
}
