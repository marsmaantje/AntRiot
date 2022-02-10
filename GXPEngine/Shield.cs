using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Shield : Pivot
{
    List<AnimationSprite> segments;
    string spriteSheetName;
    Scene parentScene;

    public int length
    {
        get => segments.Count;

        set => setShieldSize(value);
    }

    public Shield(string filename)
    {
        segments = new List<AnimationSprite>();
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
        segments.Add(new ShieldSegment(spriteSheetName, 3, 1, 3));
    }

    /// <summary>
    /// removes a segment from the shield
    /// </summary>
    void removeSegment()
    {
        segments.RemoveAt(0);
    }

    /// <summary>
    /// calculates the porition, rotation and animationFrame of each fragment
    /// </summary>
    void recalculateShield()
    {
        int i = 0;
        foreach (ShieldSegment segment in segments)
        {

            i++;
        }
    }

    /// <summary>
    /// adds or remmoves segments untill the desired length has been reached
    /// </summary>
    /// <param name="segments">desired amount of segments</param>
    void setShieldSize(int segments)
    {
        //only do anything if the lenth is actually different
        if(segments != length)
        {
            if(segments > length)
            {
                for (int i = 0; i < segments - length; i++)
                {
                    addSegment();
                }
            }
            else
            {
                for (int i = 0; i < length - segments; i++)
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
}
