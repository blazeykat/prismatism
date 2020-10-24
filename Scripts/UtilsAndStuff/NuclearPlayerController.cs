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
            Bolts = 16;
            ZapPower = 16;
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
            if (healed.specRigidbody && healed)
            {
                if (BoxOTools.BasicRandom(0.8f))
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(NuclearAmmoPickup.id).gameObject, healed.specRigidbody.UnitCenter, Vector2.zero, 1f, true, true);
                }
            }
        }

        public int Shells;

        public int Bolts;

        public int ZapPower;

        private PlayerController storedPlayer;
    }
}
