using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;
using Objects.Enemies;

namespace Objects
{
    class Bullet : CustomObject
    {

        float speed;

        //gets set first time update is called, we can safely assume it has been positioned then
        Vector2 startPoint;

        public Bullet(string filename, int cols, int rows, int frame, float speed) : base(null, filename, cols, rows)
        {
            currentFrame = frame;
            this.speed = speed;
            collider.isTrigger = true;
        }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            startPoint = new Vector2(x, y);
        }

        public void Update()
        {
            //move the bullet
            Move(0, speed * Time.deltaTime / 1000f);

            //do collision check with enemy
            checkCollisions();

            //if too far away, delete
            Vector2 globalPosition = TransformPoint(0, 0);
            float extraDistance = new Vector2(width, height).length();
            bool outsideX = globalPosition.x < -extraDistance || globalPosition.x > game.width + extraDistance;
            bool outsideY = globalPosition.y < -extraDistance || globalPosition.y > game.height + extraDistance;
            if(outsideX || outsideY)
            {
                this.LateDestroy();
            }
        }

        void checkCollisions()
        {
            GameObject[] objects = GetCollisions();
            foreach (GameObject other in objects)
            {
                if(other is Enemy)
                {
                    ((Enemy)other).kill();
                    LateDestroy();
                }
            }
        }

        
    }
}
