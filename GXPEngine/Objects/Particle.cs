using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Objects
{
    class Particle : AnimationSprite
    {
        float animationSpeed;
        public Particle(string filename, int cols, int rows, int frameCount, float animationSpeed) : base(filename, cols, rows, frameCount, addCollider:false)
        {
            this.animationSpeed = animationSpeed;
        }

        public void Update()
        {
            Animate(animationSpeed * Time.deltaTime / 1000f);
            if(currentFrame == frameCount -1)
                LateDestroy();
        }
    }
}
