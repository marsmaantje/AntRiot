using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;

namespace Objects.Enemies
{
    class Enemy : CustomObject
    {
        float angle = 0;
        protected Pivot pivot;
        const float degToRad = Mathf.PI / 180f;

        public Enemy(string filename, int cols, int rows, int startFrame, float angle) : base(null, filename, cols, rows)
        {
            currentFrame = startFrame;
            this.angle = angle;
        }

        /// <summary>
        /// positions and rotates the enemy correctly using the given angle
        /// </summary>
        /// <param name="parentScene"></param>
        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            Vector2 playerPos = new Vector2(parentScene.player.x, parentScene.player.y);
            SetOrigin(width / 2, height / 2);
            SetXY(playerPos.x, playerPos.y);
            createPivotAtPoint(x, y);
            pivot.rotation = angle;

            Vector2 globalPosition = TransformPoint(0, 0);
            float globalScale = TransformDirection(1, 0).length();

            //calculate
            float distance = 0;
            /*
            switch(Mathf.Floor(angle/90f))
            {
                case 0:
                    Console.WriteLine(0);
                    distance = angle == 0 ? globalPosition.y: Mathf.Min(globalPosition.y / Mathf.Cos(angle * degToRad), globalPosition.x / Mathf.Cos((90 - angle) * degToRad));
                    break;

                case 1:
                    Console.WriteLine(1);
                    break;

                case 2:
                    Console.WriteLine(2);
                    break;

                case 3:
                    Console.WriteLine(3);
                    break;
            }
            Console.WriteLine(distance);
            */
            distance = 200;

            this.SetXY(0, -distance);
        }
        
        public void Update()
        {
            GameObject[] hits = GetCollisions();
            if(y >= -this.height)
            {
                parentScene.player.takeDamage();
                kill();
            }
        }

        /// <summary>
        /// Creates a pivot at the given point and parents itself to it
        /// </summary>
        /// <returns>Pivot created</returns>
        public Pivot createPivotAtPoint(float x, float y)
        {
            pivot = new Pivot();
            parentScene.AddChild(pivot);
            pivot.SetXY(x, y);
            pivot.AddChild(this);
            return pivot;
        }

        /// <summary>
        /// deletes the enemy and its pivot
        /// </summary>
        public void kill()
        {
            this.LateDestroy();
            pivot.LateDestroy();
        }

        public Pivot getPivot() => pivot;

        /// <summary>
        /// methos to get the value closest to zero
        /// </summary>
        /// <param name="inputs">float array of input values</param>
        /// <returns>the number in the array that is closest to zero, max float value if the  array was empty</returns>
        public float closestToZero(float[] inputs)
        {
            float returnVal = float.MaxValue;
            foreach (float value in inputs)
            {
                if(Mathf.Abs(value) < Mathf.Abs(returnVal))
                {
                    returnVal = value;
                }
            }
            return returnVal;
        }
    }
}