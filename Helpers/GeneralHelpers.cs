using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace Bangarang.Helpers;
public class GeneralHelpers
{
    /*
     * Copyright RecursiveCollapse https://github.com/Zeodexic/tsorcRevamp/blob/main/tsorcRevampUtils.cs#L774 
     */

    /// <summary>
    ///     Returns a vector pointing from a source, to a target, with a speed.
    ///     Simplifies basic projectile, enemy dash, etc aiming calculations to a single call.
    ///     If "ballistic" is true it adjusts for gravity. Default is 0.1f, may be stronger or weaker for some projectiles though.
    /// </summary>
    /// <param name="source">The start point of the vector</param>
    /// <param name="target">The end point it is aiming towards</param>
    /// <param name="speed">The length of the resulting vector</param>
    public static Vector2 GenerateTargetingVector(Vector2 source, Vector2 target, float speed) {
        Vector2 distance = target - source;
        distance.Normalize();
        return distance * speed;
    }

    /// <summary>
    ///     Accelerates an entity toward a target in a smooth way
    ///     Returns a Vector2 with length 'acceleration' that points in the optimal direction to accelerate the NPC toward the target
    ///     If the target is moving, then it accounts for that
    ///     (No, unfortunately the optimal direction is not actually a straight line most of the time)
    ///     Accelerates until the NPC is moving fast enough that the acceleration can *just* slow it down in time, then does so
    ///     Do not ask me how long this took 💀
    /// </summary>
    /// <param name="actor">The entity moving</param>
    /// <param name="target">The target point it is aiming for</param>
    /// <param name="acceleration">The rate at which it can accelerate</param>
    /// <param name="topSpeed">The max speed of the entity</param>
    /// <param name="targetVelocity">The velocity of its target, defaults to 0</param>
    /// <param name="bufferZone">Should it smoothly slow down on approach?</param>
    public static void SmoothHoming(Entity actor, Vector2 target, float acceleration, float topSpeed, Vector2? targetVelocity = null, bool bufferZone = true, float bufferStrength = 0.1f) {
        //If the target has a velocity then factor it in
        Vector2 velTarget = Vector2.Zero;
        if (targetVelocity != null) {
            velTarget = targetVelocity.Value;
        }

        //Get the difference between the center of both entities
        Vector2 posDifference = target - actor.Center;

        //Get the distance between them
        float distance = posDifference.Length();

        //Get the difference of velocities
        //This shifts the reference frame of the calculations, from here on out we are looking at the problem as if Entity 1 was still and Entity 2 had the velocity of both entities combined
        //The formulas below calculate where it will be in the future and then the entity is accelerated toward that point on an intercept trajectory
        Vector2 vTarget = velTarget - actor.velocity;

        //Normalize posDifference to get the direction of it, ignoring the length
        posDifference.Normalize();

        //Use a dot product to get the length of the velocity vector in the direction of the target.
        //This tells us how fast the actor is moving toward the target already
        float velocity = Vector2.Dot(-vTarget, posDifference);

        //Use the current velocity plus acceleration to calculate how long it will take to arrive using the formula for acceleration
        float eta = (-velocity / acceleration) + (float)Math.Sqrt((velocity * velocity / (acceleration * acceleration)) + (2 * distance / acceleration));

        //Use the velocity plus arrival time plus current target location to calculate the location the target will be at in the future
        Vector2 impactPos = target + (vTarget * eta);

        //Generate a vector with length 'acceleration' pointing at that future location
        Vector2 fixedAcceleration = GenerateTargetingVector(actor.Center, impactPos, acceleration);

        //If distance or acceleration is 0 it will have nans, this deals with that
        if (fixedAcceleration.HasNaNs()) {
            fixedAcceleration = Vector2.Zero;
        }

        //Update its acceleration
        actor.velocity += fixedAcceleration;

        //Slow it down to the speed limit if it is above it
        if (actor.velocity.Length() > topSpeed) {
            actor.velocity.Normalize();
            actor.velocity *= topSpeed;
        }

        //If it needs to slow down when arriving then do so
        //A distance of 300 and the formula here are super fudged. Could use improvement.
        if (bufferZone && distance < 300) {
            actor.velocity *= (float)Math.Pow(distance / 300, bufferStrength);
        }
    }
}
