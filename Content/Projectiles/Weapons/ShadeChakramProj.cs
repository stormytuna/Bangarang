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
            ProjectileID.Sets.TrailCacheLength[Type] = 11;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults() {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.penetrate = -1;

            Projectile.tileCollide = true;

            ReturnSpeed = 24f;
            HomingOnOwnerStrength = 8f;
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
            for (int i = 1; i < Projectile.oldPos.Length; i += 2) {
                Main.EntitySpriteDraw(
                    ShadeChakramTexture.Value,
                    Projectile.oldPos[i] - Main.screenPosition,
                    null,
                    Color.White * (i / Projectile.oldPos.Length),
                    Projectile.rotation,
                    Vector2.Zero,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                    );
            }

            return true;
        }
    }
}