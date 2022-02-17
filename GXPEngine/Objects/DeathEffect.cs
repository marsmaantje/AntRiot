using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

namespace Objects
{
    class DeathEffect : CustomObject
    {
        
        public DeathEffect(string filename, int cols, int rows, int frames = -1) : base (null, filename, cols, rows, frames)
        {

        }

        public void Update()
        {
            Animate(Globals.animationFramerate * Time.deltaTime / 1000f);
            if (currentFrame >= frameCount - 1)
                LateDestroy();
        }
    }
}
