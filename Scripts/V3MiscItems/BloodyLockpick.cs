using Dungeonator;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class BloodyLockpick : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Bloody Lockpick";
            string resourcePath = "katmod/Resources/V3MiscItems/bloodylockpick";
            GameObject obj = new GameObject(itemName);
            BloodyLockpick item = obj.AddComponent<BloodyLockpick>();
            ItemBuilder.AddSpriteToObject(itemName, resourcePath, obj);
            string shortDesc = "pain";
            string longDesc = "Use hearts as keys.\n\nMade of bronze.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.B;
            item.SetCooldownType(ItemBuilder.CooldownType.Timed, 5);
            item.PlaceItemInAmmonomiconAfterItemById(356);
            item.consumable = false;
        }

        public override bool CanBeUsed(PlayerController user)
        {
            if (!user || user.CurrentRoom == null || user.CurrentRoom.CompletelyPreventLeaving)
            {
                return false;
            }
            IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
            if (nearestInteractable is InteractableLock || nearestInteractable is Chest || nearestInteractable is DungeonDoorController)
            {
                if (nearestInteractable is InteractableLock)
                {
                    InteractableLock interactableLock = nearestInteractable as InteractableLock;
                    if (interactableLock && !interactableLock.IsBusted && interactableLock.transform.position.GetAbsoluteRoom() == user.CurrentRoom && interactableLock.IsLocked && !interactableLock.HasBeenPicked && interactableLock.lockMode == InteractableLock.InteractableLockMode.NORMAL)
                    {
                        return base.CanBeUsed(user);
                    }
                }
                else if (nearestInteractable is DungeonDoorController)
                {
                    DungeonDoorController dungeonDoorController = nearestInteractable as DungeonDoorController;
                    if (dungeonDoorController != null && dungeonDoorController.Mode == DungeonDoorController.DungeonDoorMode.COMPLEX && dungeonDoorController.isLocked && !dungeonDoorController.lockIsBusted)
                    {
                        return base.CanBeUsed(user);
                    }
                }
                else if (nearestInteractable is Chest)
                {
                    Chest chest = nearestInteractable as Chest;
                    return chest && chest.GetAbsoluteParentRoom() == user.CurrentRoom && chest.IsLocked && !chest.IsLockBroken && !chest.IsMimic && base.CanBeUsed(user);
                }
            }
            return false;
        }

        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
			IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
			if (nearestInteractable is InteractableLock || nearestInteractable is Chest || nearestInteractable is DungeonDoorController)
			{
				if (nearestInteractable is InteractableLock)
				{
					InteractableLock interactableLock = nearestInteractable as InteractableLock;
					if (interactableLock.lockMode == InteractableLock.InteractableLockMode.NORMAL)
					{
						interactableLock.ForceUnlock();
                        user.healthHaver.ApplyDamage(1, Vector2.zero, "Blood Loss");
                        AkSoundEngine.PostEvent("m_OBJ_lock_pick_01", GameManager.Instance.gameObject);
					}
					return;
				}
				if (nearestInteractable is DungeonDoorController)
				{
					DungeonDoorController dungeonDoorController = nearestInteractable as DungeonDoorController;
					if (dungeonDoorController != null && dungeonDoorController.Mode == DungeonDoorController.DungeonDoorMode.COMPLEX && dungeonDoorController.isLocked)
					{
						dungeonDoorController.Unlock();
                        user.healthHaver.ApplyDamage(1, Vector2.zero, "Blood Loss");
                        AkSoundEngine.PostEvent("m_OBJ_lock_pick_01", GameManager.Instance.gameObject);
					}
				}
				else if (nearestInteractable is Chest)
				{
					Chest chest = nearestInteractable as Chest;
					if (chest.IsLocked)
					{
                        chest.ForceUnlock();
                        user.healthHaver.ApplyDamage(1, Vector2.zero, "Blood Loss");
                        AkSoundEngine.PostEvent("m_OBJ_lock_pick_01", GameManager.Instance.gameObject);
					}
				}
			}
		}
	}
}
