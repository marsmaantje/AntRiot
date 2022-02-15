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
    class ScoreDisplay : EasyDraw
    {

        public ScoreDisplay(int width, int height) : base(width, height, addCollider:false)
        {
            setText("");
        }

        public void setText(string newText)
        {
            Clear(0, 0, 0, 0);
            float width = 0;
            if(HorizontalTextAlign == CenterMode.Center)
                width = TextWidth(newText);
            Text(newText, width/2, 0);
        }
    }
}
