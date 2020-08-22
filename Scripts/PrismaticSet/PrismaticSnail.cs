using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class PrismaticSnail : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Prismatic Snail";
            string resourceName = "katmod/Resources/V2MiscItems/coolammo";
            GameObject obj = new GameObject();
            PrismaticSnail item = obj.AddComponent<PrismaticSnail>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Passive Guons";
            string longDesc = "Slowly gives you ammo while in combat.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.EXCLUDED;
            item.PlaceItemInAmmonomiconAfterItemById(170);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.GunChanged += this.ggBool;
        }

        private void ggBool(Gun g1, Gun g2, bool yes)
        {
            StopCoroutine(Holdup());
        }

        protected override void Update()
        {
            base.Update();
            if (!hotDown && !GameManager.Instance.IsLoadingLevel && this.m_owner != null && this.Owner.Velocity.magnitude == 0 && !this.m_owner.IsFalling && this.m_owner.healthHaver)
            {
                hotDown = true;
                StartCoroutine(Holdup());
            }
        }

        private IEnumerator Holdup()
        {
            yield return new WaitForSeconds(0.75f);
            hotDown = false;
            if (!GameManager.Instance.IsLoadingLevel && this.m_owner != null && this.Owner.Velocity.magnitude == 0 && !this.m_owner.IsFalling && this.m_owner.healthHaver)
            {
                AkSoundEngine.PostEvent("Play_OBJ_crystal_shatter_01", base.gameObject);
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(PrismaticGuonStone.ID).gameObject, base.Owner);
            }
            yield break;
        }

        bool hotDown = false;
    }
}
