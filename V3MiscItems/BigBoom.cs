using ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace katmod
{
    class BigBoom : PassiveItem
	{
		public static void Init()
		{
			string name = "Big Boom";
			string resourcePath = "katmod/Resources/V3MiscItems/bigboom.png";
			GameObject gameObject = new GameObject(name);
			BigBoom item = gameObject.AddComponent<BigBoom>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Big Boom";
			string longDesc = "Big Boom";
			item.SetupItem(shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.S;
			item.PlaceItemInAmmonomiconAfterItemById(312);
		}

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += Boom;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= Boom;
            return base.Drop(player);
        }

        private void Boom(Projectile projectile, float arg2)
        {
            if (balancer)
            {
                balancer = false;
                StartCoroutine(Balanced());
                projectile.OnDestruction += Boomber;
            }
        }

        private void Boomber(Projectile obj)
        {
            if (obj.sprite)
            {
                Exploder.DoDefaultExplosion(obj.sprite.WorldCenter, default);
            }
        }

        public IEnumerator Balanced()
        {
            yield return new WaitForSeconds(3);
            balancer = true;
            yield break;
        }

        public bool balancer = true;
    }
}
