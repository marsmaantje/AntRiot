using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace UIElements
{
    /// <summary>
    /// simple class for displaying the current score
    /// </summary>
    class HighScoreDisplay : EasyDraw
    {

        public HighScoreDisplay(int width, int height) : base(width, height, addCollider:false)
        {
        }

        public void Update()
        {
            string newText = "High score: " + Globals.highScore;
            Clear(0, 0, 0, 0);
            float width = 0;
            if(HorizontalTextAlign == CenterMode.Center)
                width = TextWidth(newText);
            Text(newText, width/2, 0);
        }
    }
}
