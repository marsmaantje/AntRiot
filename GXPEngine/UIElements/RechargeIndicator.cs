using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using Objects;
using TiledMapParser;

namespace UIElements
{
    class RechargeIndicator : CustomObject
    {
        public float progress = 0;
        EasyDraw progressBar;

        public RechargeIndicator(string filename, int cols, int rows, TiledObject obj) : base(obj, filename, cols, rows, addCollider:false)
        {

        }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            progressBar = new EasyDraw(width*2 - 1, height*2 - 1, false);
            AddChild(progressBar);
            Console.WriteLine("my width: " + width);
            SetOrigin(0, 0);
            Move((int)(-width / 2), (int)(-height / 2));
            progressBar.SetXY(0,0);
            progressBar.SetOrigin(0, 0);
            progressBar.SetScaleXY(1, 1);
        }

        public void Update()
        {
            progressBar.Clear(0, 0, 0, 0);
            progressBar.NoStroke();
            progressBar.Fill(0,204);
            progressBar.Rect(0, 0, progressBar.width, Globals.map(1 - progress, 0, 1, 0, progressBar.height));
        }

        

    }
}
