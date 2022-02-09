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
        Pivot pivot;

        public Enemy(string filename, int cols, int rows, int startFrame, float angle) : base(null, filename, cols, rows)
        {
            currentFrame = startFrame;
            this.angle = angle;
        }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            Vector2 playerPos = new Vector2(parentScene.player.x, parentScene.player.y);
            SetXY(playerPos.x, playerPos.y);
            createPivotAtPoint(x, y);
            pivot.rotation = angle;
            this.SetXY(0, 100);
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

        public void kill()
        {
            this.LateDestroy();
            pivot.LateDestroy();
        }

        public Pivot getPivot() => pivot;
    }
}