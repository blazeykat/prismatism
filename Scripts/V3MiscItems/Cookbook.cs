using Dungeonator;
using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
	class Cookbook : PlayerItem
	{

		public static void Init()
		{
			string name = "Cookbook";
			string resourcePath = "katmod/Resources/V3MiscItems/whatthefuck";
			GameObject gameObject = new GameObject(name);
			Cookbook item = gameObject.AddComponent<Cookbook>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Spawns Bombs";
			string longDesc = "Prismatism is now (probably) responsible for placing nevernamed on an FBI watchlist.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = ItemQuality.C;
			item.PlaceItemInAmmonomiconAfterItemById(487);
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 300f);
		}

        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
			base.StartCoroutine(BombsAway());
			base.StartCoroutine(ItemBuilder.HandleDuration(this, 15, user, null));
        }

		private IEnumerator BombsAway()
        {
			for (int i = 0; i < 60; i++)
            {
				if (LastOwner != null)
                {
					if (LastOwner.CurrentRoom != null)
					{
						Vector3 position = LastOwner.CurrentRoom.GetRandomVisibleClearSpot(1, 1).ToVector3();
						PlaceBomb(position);
					}
					yield return new WaitForSeconds(0.25f);
				}
			}
			yield break;
        }

		public void PlaceBomb(Vector3 position)
        {
			SpawnObjectPlayerItem playerItem = PickupObjectDatabase.GetById(108).GetComponent<SpawnObjectPlayerItem>();
			GameObject bombPrefab = playerItem.objectToSpawn.gameObject;
			GameObject BombObject = UnityEngine.Object.Instantiate<GameObject>(bombPrefab, position, Quaternion.identity);
			tk2dBaseSprite bombSprite = BombObject.GetComponent<tk2dBaseSprite>();
			if (bombSprite)
			{
				bombSprite.PlaceAtPositionByAnchor(position, tk2dBaseSprite.Anchor.MiddleCenter);
			}
		}
    }
}
