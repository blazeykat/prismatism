using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Security.AccessControl;

namespace katmod
{
    class NuclearPlayerController : MonoBehaviour
    {
        public NuclearPlayerController()
        {
            Shells = 16;
        }

        protected void Awake()
        {
            if (GetComponent<PlayerController>() != null)
            {
                storedPlayer = GetComponent<PlayerController>();
                storedPlayer.OnKilledEnemyContext += AmmoInator;
            }
        }

        protected void Destroy()
        {
            storedPlayer.OnKilledEnemyContext -= AmmoInator;
        }

        public void AmmoInator(PlayerController player, HealthHaver healed)
        {
            if (healed && healed.specRigidbody && player)
            {
                if (BoxOTools.BasicRandom(0.825f) && Shells < Mathf.FloorToInt(55 * player.stats.GetStatValue(PlayerStats.StatType.AmmoCapacityMultiplier)) && player.HasGun(Gungeon.Game.Items["psm:wastelanders_shotgun"].PickupObjectId))
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(NuclearAmmoPickup.id).gameObject, healed.specRigidbody.UnitCenter, Vector2.zero, 1f, true, true);
                }
            }
        }

        public int Shells;

        private PlayerController storedPlayer;
    }
}
