using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace Objects.Enemies
{
    class ShootingEnemy : Enemy
    {
        int nextShotTime = 0; //time at wich the next shot will be fired
        const int shootInterval = 1500; //minimum interval between shots
        const int shootIntervalRandomness = 800; //randomness of shotInterval
        int circleDistance = 130; //distance at wich the buf starts circling the player and shooting
        bool finalApproach = false;
        int ammo = 5;
        Random ran = new Random();

        public ShootingEnemy(string filename, int cols, int rows, int startFrame, float angle, float distance = -1, int score = 0, int animationFrames = 1) : base(filename, cols, rows, startFrame, angle, distance, score, animationFrames)
        {}

        public new void Update()
        {
            base.Update();

            if(y >= -circleDistance && !finalApproach) //if distance reached, circle and occasionally shoot
            {
                pivot.rotation += 30 * Time.deltaTime / 1000f;

                if(Time.time > nextShotTime)
                {
                    shoot();
                    nextShotTime = Time.time + shootInterval + ran.Next(0, shootIntervalRandomness);
                }
            }
            else //if too far, move closer
            {
                Move(0, 30*Time.deltaTime / 1000f);
            }
        }

        void shoot()
        {
            Shot bullet = new Shot("sprites/enemies.png", 4, 2, 4, pivot.rotation, -y, 25, 2);
            parentScene.AddChild(bullet);
            bullet.initialize(parentScene);
            ammo--;
            if (ammo == 0)
                finalApproach = true;
        }
    }
}