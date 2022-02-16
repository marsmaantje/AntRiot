using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Objects.Enemies
{
    class ApproachingEnemy : Enemy
    {

        public ApproachingEnemy(string filename, int cols, int rows, int startFrame, float angle, float distance = -1, int score = 0, int animationFrames = 1) : base(filename, cols, rows, startFrame, angle, distance, score, animationFrames)
        {

        }

        public new void Update()
        {
            base.Update();

            bool hitShield = false;
            GameObject[] collisions = GetCollisions();
            foreach (GameObject other in collisions)
            {
                if(other is ShieldSegment)
                {
                    hitShield = true;
                }
            }
            if(!hitShield)
                Move(0, 30*Time.deltaTime / 1000f);
        }
    }
}
