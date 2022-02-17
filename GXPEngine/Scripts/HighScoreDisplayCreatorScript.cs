using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using TiledMapParser;
using GXPEngine;
using Objects.Enemies;

namespace Scripts
{
    class HighScoreDisplayCreatorScript : Script
    {
        public HighScoreDisplayCreatorScript(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
        {

        }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            UIElements.HighScoreDisplay display = new UIElements.HighScoreDisplay(200, 55);
            parentScene.AddChild(display);
            display.TextAlign(CenterMode.Center, CenterMode.Min);
            display.TextSize(12);
            display.SetXY(game.width/2 - 60 , game.height - 70);
        }
    }
}
