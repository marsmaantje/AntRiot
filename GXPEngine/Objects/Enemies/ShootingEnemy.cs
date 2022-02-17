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
        const int shootInterval = 5000; //minimum interval between shots
        const int shootIntervalRandomness = 800; //randomness of shotInterval
        int circleDistance = 200; //distance at wich the buf starts circling the player and shooting
        bool finalApproach = false;
        int ammo = 500000;
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
            else //if too far or on final approach, move closer
            {
                Move(0, 15*Time.deltaTime / 1000f);
            }
        }

        /// <summary>
        /// Fire a bullet
        /// </summary>
        void shoot()
        {
            Shot bullet = new Shot("sprites/shot.png", 4, 1, 0, pivot.rotation, -y, 0, animationFrames:4, speed:60);
            parentScene.AddChild(bullet);
            bullet.initialize(parentScene);
            ammo--;
            if (ammo == 0)
                finalApproach = true;
        }
    }
}