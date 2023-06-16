using Microsoft.Xna.Framework;
using Terraria;

namespace Bangarang.Helpers;

public static class CollisionHelpers
{
	/// <summary>Simply wraps Collision.CanHit</summary>
	/// <returns></returns>
	public static bool CanHit(Entity source, Vector2 targetPosition, int targetWidth = 1, int targetHeight = 1) => Collision.CanHit(source.position, source.width, source.height, targetPosition, targetWidth, targetHeight);
}