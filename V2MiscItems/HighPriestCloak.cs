using ItemAPI;
using UnityEngine;


namespace katmod
{

    class HighPriestCloak : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Low Priest's Cloak";
            string resourceName = "katmod/Resources/V2MiscItems/lowpriestcape";
            GameObject obj = new GameObject();
            HighPriestCloak item = obj.AddComponent<HighPriestCloak>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Omniscent";
            string longDesc = "A relic of the low priest, before they became more than human.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = ItemQuality.A;
            item.PlaceItemInAmmonomiconAfterItemById(293);
        }


        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            woop.spawnShadows = true;
            player.gameObject.AddComponent(woop);
            player.OnKilledEnemy += SacrificialDaggers;
            player.OnReloadedGun += Reloader;
        }

        private void Reloader(PlayerController player, Gun gun)
        {
            Kills = 0;
            if (shield != null)
                shield.ThrowShield();
        }

        private void SacrificialDaggers(PlayerController player)
        {
            Kills += 1;
            if (shield != null && shield.gameObject)
            {
                shield.ThrowShield();
            }
            shield = CreateEffect(player, Kills);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnKilledEnemy -= SacrificialDaggers;
            player.OnReloadedGun -= Reloader;
            player.gameObject.GetComponent<ImprovedAfterImage>().spawnShadows = false;
            return base.Drop(player);
        }

        readonly ImprovedAfterImage woop = new ImprovedAfterImage()
        {
            dashColor = Color.red,
            spawnShadows = true,
        };

        private KnifeShieldEffect CreateEffect(PlayerController user, int numKnives, float radiusMultiplier = 2f)
        {
            KnifeShieldItem shield = (KnifeShieldItem)PickupObjectDatabase.GetById(65);
            KnifeShieldEffect knifeShieldEffect = new GameObject("knife shield effect")
            {
                transform =
            {
                position = user.LockedApproximateSpriteCenter,
                parent = user.transform
            }
            }.AddComponent<KnifeShieldEffect>();
            knifeShieldEffect.numKnives = numKnives;
            knifeShieldEffect.remainingHealth = shield.knifeHealth;
            knifeShieldEffect.knifeDamage = shield.knifeDamage;
            knifeShieldEffect.circleRadius = radiusMultiplier;
            knifeShieldEffect.rotationDegreesPerSecond = shield.rotationDegreesPerSecond;
            knifeShieldEffect.throwSpeed = shield.throwSpeed;
            knifeShieldEffect.throwRange = shield.throwRange;
            knifeShieldEffect.throwRadius = shield.throwRadius;
            knifeShieldEffect.radiusChangeDistance = shield.radiusChangeDistance;
            knifeShieldEffect.deathVFX = shield.knifeDeathVFX;
            this.shieldPrefab = shield.knifePrefab;
            knifeShieldEffect.Initialize(user, shieldPrefab);
            return knifeShieldEffect;
        }

        GameObject shieldPrefab;

        public int Kills;

        public KnifeShieldEffect shield;
    }

}
