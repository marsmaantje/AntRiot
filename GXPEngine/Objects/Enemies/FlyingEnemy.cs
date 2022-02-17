using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Objects.Enemies
{
    class FlyingEnemy : Enemy
    {
        int health = 1;
        public FlyingEnemy(string filename, int cols, int rows, int startFrame, float angle, float distance = -1, int score = 0, int animationFrames = 1, int health = 1) : base(filename, cols, rows, startFrame, angle, distance, score, animationFrames)
        {
            this.health = health;
        }

        public new void Update()
        {
            base.Update();

            Move(0, 30*Time.deltaTime / 1000f);
        }

        public override void damage()
        {
            health--;
            if (health <= 0)
                base.kill();
        }
    }
}
