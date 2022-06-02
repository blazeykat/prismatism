using ItemAPI;
using UnityEngine;

namespace katmod
{
    class Randy : IounStoneOrbitalItem
    {
        public static void Init()
        {
            string name = "Randy";
            string resourcePath = "katmod/Resources/DecaySet/randy.png";
            GameObject gameObject = new GameObject();
            Randy item = gameObject.AddComponent<Randy>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "https://modworkshop.net/mod/27616";
            string longDesc = "god himself";
            item.SetupItem(shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.EXCLUDED;

            Randy.BuildPrefab();
            item.OrbitalPrefab = Randy.orbitalPrefab;
            item.Identifier = IounStoneIdentifier.GENERIC;

            item.CanBeDropped = true;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 3f);
        }

        public static void BuildPrefab()
        {
            bool flag = Randy.orbitalPrefab != null;
            if (!flag)
            {
                GameObject gameObject = SpriteBuilder.SpriteFromResource("katmod/Resources/DecaySet/randy.png", null);
                gameObject.name = "randorbital";
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(48, 48));
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;
                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.BulletBlocker;
                Randy.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                Randy.orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
                Randy.orbitalPrefab.shouldRotate = false;
                Randy.orbitalPrefab.orbitRadius = 4.5f;
                Randy.orbitalPrefab.SetOrbitalTier(0);
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);
            }
        }

        public static PlayerOrbital orbitalPrefab;
    }
}
