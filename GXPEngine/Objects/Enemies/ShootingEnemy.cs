using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Objects.Enemies
{
    class ShootingEnemy : Enemy
    {
        int lastShot = 0;
        bool canShoot = false;
        int spawnTime;
        int circleDistance = 130; //distance at wich the buf starts circling the player and shooting

        public ShootingEnemy(string filename, int cols, int rows, int startFrame, float angle) : base(filename, cols, rows, startFrame, angle)
        {
            spawnTime = Time.time;
        }

        public new void Update()
        {
            base.Update();

            if(y >= -circleDistance) //if distance reached, circle and occasionally shoot
            {
                pivot.rotation += 30 * Time.deltaTime / 1000f;
            }
            else //if too far, move closer
            {
                Move(0, 30*Time.deltaTime / 1000f);
            }
        }
    }
}
