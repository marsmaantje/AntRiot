using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

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

            //if too far away, delete
            Vector2 currentPos = new Vector2(x, y);
            if(Vector2.Distance(startPoint, currentPos) > (Mathf.Max(game.width, game.height)))
            {
                this.LateDestroy();
                Console.WriteLine("bullet destroyed at X:" + x + " Y:" + y);
            }
        }

        
    }
}
