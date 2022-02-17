using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Static class used for global variables.
/// recommended to USE THIS AS SPARINGLY AS POSSIBLE
/// 
/// current use is for bringing information between scenes when loading a new tiled level
/// </summary>
static class Globals
{
    public static int score = 0; //global because the endScreen needs to know the score you got from the game screen
    public static int highScore = 0;
    public static SerialPort controller; //global to prevent the controller from needing to reïnitialize between scenes
    public static float animationFramerate = 18; //global so allsprites animate at the same speed

    public static float map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = value - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }
}