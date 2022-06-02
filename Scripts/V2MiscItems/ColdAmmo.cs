using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class ColdAmmo : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Cool Ammo";
            string resourceName = "katmod/Resources/V2MiscItems/coolammo";
            GameObject obj = new GameObject();
            ColdAmmo item = obj.AddComponent<ColdAmmo>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Passive Ammo";
            string longDesc = "Slowly gives you ammo while in combat.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.A;
            item.PlaceItemInAmmonomiconAfterItemById(170);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat += this.OnEnterRoom;
            player.GunChanged += this.GgBool;
        }

        private void OnEnterRoom()
        {
            ammoCount = 0;
        }

        private void GgBool (Gun g1, Gun g2, bool yes)
        {
            StopCoroutine(Holdup());
        }

        protected override void Update()
        {
            base.Update();
            if (base.Owner != null && base.Owner.CurrentGun != null)
            {
                if (base.Owner.CurrentGun != this.lastGun)
                {
                    StopCoroutine(Holdup());
                }
                if (ammoCount < (base.Owner.CurrentGun.GetBaseMaxAmmo() / (Owner.HasPassiveItem(170) ? 8.5f : 10)) && !base.Owner.CurrentGun.InfiniteAmmo && base.Owner.CurrentGun.GetBaseMaxAmmo() != base.Owner.CurrentGun.ammo && base.Owner.IsInCombat && 1 < (base.Owner.CurrentGun.GetBaseMaxAmmo() / 10))
                {
                    if (!hotDown)
                    {
                        hotDown = true;
                        lastGun = base.Owner.CurrentGun;
                        StartCoroutine(Holdup());
                    }
                }
            }
        }

        private IEnumerator Holdup()
        {
            yield return new WaitForSeconds(2);
            if (lastGun == base.Owner.CurrentGun)
            {
                base.Owner.CurrentGun.GainAmmo(1);
                ammoCount++;
            }
            hotDown = false;
            yield break;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnEnteredCombat -= this.OnEnterRoom;
            player.GunChanged -= this.GgBool;
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            if (Owner)
            {
                Owner.OnEnteredCombat += this.OnEnterRoom;
                Owner.GunChanged += this.GgBool;
            }
            base.OnDestroy();
        }

        float ammoCount = 0;

        bool hotDown = false;

        public Gun lastGun;
    }
}
