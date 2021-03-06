using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

namespace UIElements
{
    class ExitButton : Button
    {
        /// <summary>
        /// Whether the button should take you back to the main menu or close the game
        /// </summary>
        bool backToMain = false;

        public ExitButton(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj) { }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            backToMain = obj.GetBoolProperty("BackToMain", backToMain);
        }

        protected override void OnClicked()
        {
            //either go back to the main menu or close the game
            if(backToMain)
            {
                ((MyGame)game).loadNewLevel("Main menu.tmx");
            }
            else
            {
                game.Destroy();
            }
        }
    }
}
