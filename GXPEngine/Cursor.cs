using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Cursor : EasyDraw
{
    public Cursor() : base (5,5,false)
    {
        this.SetOrigin(width / 2, height / 2);
        ShapeAlign(CenterMode.Center, CenterMode.Center);
        Ellipse(2.5f, 2.5f, 2, 2);
    }

    public void Update()
    {
        this.x = Input.mouseX;
        this.y = Input.mouseY;
    }
}
