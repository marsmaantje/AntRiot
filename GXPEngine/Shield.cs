using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Shield : Pivot
{
    List<ShieldSegment> segments;
    string spriteSheetName;
    Scene parentScene;
    float distance = 80; //distance from center in pixels
    const float radToDeg = 180 / Mathf.PI;

    public int length
    {
        get => segments.Count;
        set => setShieldSize((int)Mathf.Max(value, 2));
    }

    public Shield(string filename)
    {
        segments = new List<ShieldSegment>();
        spriteSheetName = filename;
    }

    public void Initialize(Scene parentScene)
    {
        this.parentScene = parentScene;
        SetXY(0, 0);
    }

    /// <summary>
    /// Adds a segment to the shield
    /// </summary>
    void addSegment()
    {
        ShieldSegment newSegment = new ShieldSegment(spriteSheetName, 1, 3, 3);
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
        float anglePerSegment = 90 - (Mathf.Atan((2*distance)/18) * radToDeg);
        float specialAngle = 90 - (Mathf.Atan((2*distance)/20) * radToDeg);
        for (int i = 0; i < length; i++)
        {
            ShieldSegment segment = segments[i];
            segment.SetXY(0,0);
            segment.SetOrigin(segment.width / 2, segment.height / 2);

            if (i == 0) //head
            {
                segment.rotation = (i - length / 2) * anglePerSegment - specialAngle;
                segment.Move(0, distance);
                segment.rotation += 90;
                segment.currentFrame = 0;

            }
            else if (i == length - 1) //tail
            {
                segment.rotation = (i - length / 2) * anglePerSegment + specialAngle;
                segment.Move(0, distance);
                segment.rotation += 90;
                segment.currentFrame = 2;

            }
            else //body
            {
                segment.rotation = (i - length/2)*anglePerSegment;
                segment.Move(0, distance);
                segment.rotation += 90;
                segment.currentFrame = 1;
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
        //this.rotation = Time.time / 100;
    }
}
