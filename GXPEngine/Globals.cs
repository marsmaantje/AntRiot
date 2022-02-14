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
    public static SerialPort controller; //global to prevent the controller from needing to reïnitialize between scenes
}