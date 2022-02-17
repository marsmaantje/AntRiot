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
        float speed = 30;

        public FlyingEnemy(string filename, int cols, int rows, int startFrame, float angle, float distance = -1, int score = 0, int animationFrames = 1, int health = 1, bool killOnImpact = true, float speed = 30) : base(filename, cols, rows, startFrame, angle, distance, score, animationFrames, killOnImpact)
        {
            this.health = health;
            this.speed = speed;
        }

        public new void Update()
        {
            base.Update();

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
