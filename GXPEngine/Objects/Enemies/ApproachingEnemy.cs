using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Objects.Enemies
{
    class ApproachingEnemy : Enemy
    {
        int health = 1;
        float speed = 30;

        public ApproachingEnemy(string filename, int cols, int rows, int startFrame, float angle, float distance = -1, int score = 0, int animationFrames = 1, int health = 1, float speed = 30) : base(filename, cols, rows, startFrame, angle, distance, score, animationFrames)
        {
            this.health = health;
            this.speed = speed;
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
                    Move(0, -speed * Time.deltaTime / 1000f);
                }
            }
            if(!hitShield)
                Move(0, speed*Time.deltaTime / 1000f);
        }

        public override void damage()
        {
            health--;
            if (health <= 0)
                base.kill();
        }
    }
}
