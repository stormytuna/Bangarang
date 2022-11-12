using Bangarang.Common.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bangarang.Content.Projectiles.Weapons {
    public class ShadeChakramProj : Boomerang {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shade Chakram");
            ProjectileID.Sets.TrailCacheLength[Type] = 4;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override bool IsLoadingEnabled(Mod mod) => ServerConfig.Instance.ModdedBoomerangs;

        public override void SetDefaults() {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = 3;

            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;

            Projectile.tileCollide = true;

            ReturnSpeed = 24f;
            HomingOnOwnerStrength = 2f;
            TravelOutFrames = 25;
            DoTurn = true;
        }

        private Asset<Texture2D> _shadeChakram;
        private Asset<Texture2D> ShadeChakramTexture {
            get {
                if (_shadeChakram == null) {
                    _shadeChakram = ModContent.Request<Texture2D>("Bangarang/Content/Projectiles/Weapons/ShadeChakramProj");
                }
                return _shadeChakram;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            for (int i = 0; i < Projectile.oldPos.Length; i++) {
                Main.EntitySpriteDraw(
                    ShadeChakramTexture.Value,
                    Projectile.oldPos[i] - Main.screenPosition + new Vector2(15f, 15f),
                    null,
                    Color.White * ((float)i / (float)Projectile.oldPos.Length),
                    Projectile.rotation,
                    ShadeChakramTexture.Value.Size() / 2f,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                    );
            }

            return true;
        }
    }
}