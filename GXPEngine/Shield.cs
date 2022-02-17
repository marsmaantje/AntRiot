using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using Objects.Enemies;

class Shield : Pivot
{
    List<ShieldSegment> segments;
    string spriteSheetName;
    Scene parentScene;
    float distance = 80; //distance from center in pixels
    const float radToDeg = 180 / Mathf.PI;
    public bool isShooting = false;
    bool isReturning = false;
    int shieldLength = 0;
    float shootSpeed = 0;
    int shootStartTime = 0;
    int nextShrink = 0;
    AnimationSprite bullet = new AnimationSprite("sprites/shieldroll.png", 2, 1, 2, addCollider: true);

    /// <summary>
    /// set and get the length of the shield
    /// </summary>
    public int length
    {
        get => segments.Count;
        set => setShieldSize(Math.Max(value, 0));
    }

    public Shield(string filename)
    {
        segments = new List<ShieldSegment>();
        spriteSheetName = filename;
    }

    public void Initialize(Scene parentScene)
    {
        this.parentScene = parentScene;
        bullet = new AnimationSprite("sprites/shieldroll.png", 2, 1, 2, addCollider: true);
        bullet.visible = false;
        AddChild(bullet);
        bullet.SetOrigin(bullet.width / 2, bullet.height / 2);
        bullet.SetXY(0, distance);
        SetXY(0, 0);
    }

    public void Update()
    {
        bullet.visible = false;
        //if we are currently doing the shooting animation
        if (isShooting)
        {
            //filthy statemachine, but it works...
            if (length > 0 && nextShrink < Time.time && !isReturning) //should remove two segemnts?
            {
                nextShrink = Time.time + 200;
                length -= (length % 2 == 0) ? 2 : 1;
            }
            else if (length <= 0) //are we finished shrinking?
            {
                bullet.visible = true;
                if (isReturning)
                {
                    bullet.Move(0, -shootSpeed * Time.deltaTime / 1000f);
                    if (bullet.y <= distance)
                    {
                        length += 2;
                        bullet.visible = false;
                    }
                }
                else
                {
                    bullet.Move(0, shootSpeed * Time.deltaTime / 1000f);
                    if (bullet.y > 350)
                        isReturning = true;
                }
            }
            else if (length > 0 && nextShrink < Time.time && isReturning) //are we expanding
            {
                bullet.visible = false;
                nextShrink = Time.time + 200;
                length += Math.Min(2, shieldLength - length);
                if(length == shieldLength)
                {
                    isShooting = false;
                }
            }
            else
            {
                bullet.SetXY(0, distance);
            }

            bullet.Animate(Globals.animationFramerate * Time.deltaTime / 1000f);

            //kill all enemies we are colliding with
            GameObject[] collisions = bullet.GetCollisions();
            foreach (GameObject other in collisions)
            {
                if(other is Enemy)
                {
                    ((Enemy)other).kill();
                }
            }
        }
    }

    /// <summary>
    /// Tells the shield to fire, it will shrink and then roll out.
    /// It will return once it has left the boundaries of the screen + 50 pixels
    /// </summary>
    /// <param name="fireSpeed">speed at which it should roll</param>
    public void fire(float shootSpeed)
    {
        this.shootSpeed = shootSpeed;
        isShooting = true;
        isReturning = false;
        shieldLength = length;
        shootStartTime = Time.time;
    }

    /// <summary>
    /// Adds a segment to the shield
    /// </summary>
    void addSegment()
    {
        ShieldSegment newSegment = new ShieldSegment(spriteSheetName, 2, 3, 6);
        segments.Add(newSegment);
        AddChild(newSegment);

    }

    /// <summary>
    /// removes a segment from the shield
    /// </summary>
    void removeSegment()
    {
        segments[0].LateDestroy();
        segments.Remove(segments[0]);
    }

    /// <summary>
    /// calculates the porition, rotation and animationFrame of each fragment
    /// </summary>
    void recalculateShield()
    {
        float arcLength = length * 16 + 68; //(magic values yay) total length the shield will have
        float anglePerSegment = 90 - (Mathf.Atan((2 * distance) / 18) * radToDeg);
        float specialAngle = 90 - (Mathf.Atan((2 * distance) / 20) * radToDeg);
        for (int i = 0; i < length; i++)
        {
            ShieldSegment segment = segments[i];
            segment.SetXY(0, 0);
            segment.SetOrigin(segment.width / 2, segment.height / 2);

            if (i == 0) //head
            {
                segment.rotation = (i - length / 2) * anglePerSegment - specialAngle;
                segment.Move(0, distance);
                segment.rotation += 90;
                segment.currentFrame = 0;
                segment.SetCycle(0, 2);
            }
            else if (i == length - 1) //tail
            {
                segment.rotation = (i - length / 2) * anglePerSegment + specialAngle;
                segment.Move(0, distance);
                segment.rotation += 90;
                segment.currentFrame = 4;
                segment.SetCycle(4, 2);
            }
            else //body
            {
                segment.rotation = (i - length / 2) * anglePerSegment;
                segment.Move(0, distance);
                segment.rotation += 90;
                segment.currentFrame = 2;
                segment.SetCycle(2, 2);
            }
        }
    }

    /// <summary>
    /// adds or remmoves segments untill the desired length has been reached
    /// </summary>
    /// <param name="segments">desired amount of segments</param>
    void setShieldSize(int segments)
    {
        //only do anything if the lenth is actually different
        if (segments != length)
        {
            int startLength = length;
            if (segments > startLength)
            {
                for (int i = 0; i < segments - startLength; i++)
                {
                    addSegment();
                }
            }
            else
            {
                for (int i = 0; i < startLength - segments; i++)
                {
                    removeSegment();
                }
            }
            recalculateShield();
        }
    }
}

/// <summary>
/// class made just for typeChecking on enemy colission
/// </summary>
class ShieldSegment : AnimationSprite
{
    public ShieldSegment(string filename, int cols, int rows, int frames = -1) : base(filename, cols, rows, frames) { }

    public void Update()
    {
        this.Animate(Globals.animationFramerate / 6 * Time.deltaTime / 1000f);
    }
}
