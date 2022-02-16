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
        float startDistance;
        int score;
        int animationFrames = 1;

        public Enemy(string filename, int cols, int rows, int startFrame, float angle, float distance = -1, int score = 0, int animationFrames = 1) : base(null, filename, cols, rows)
        {
            currentFrame = startFrame;
            this.angle = angle;
            startDistance = distance;
            this.score = score;
            this.animationFrames = animationFrames;
            SetCycle(startFrame, animationFrames);
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
            if (startDistance < 0)
            {
                startDistance = 350;
            }
            this.SetXY(0, -startDistance);
        }

        public void Update()
        {
            Animate(Globals.animationFramerate * Time.deltaTime / 1000f);
            GameObject[] hits = GetCollisions();
            if (y >= -this.height)
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
        public virtual void kill()
        {
            Globals.score += score;
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
                if (Mathf.Abs(value) < Mathf.Abs(returnVal))
                {
                    returnVal = value;
                }
            }
            return returnVal;
        }
    }
}