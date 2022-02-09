using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using TiledMapParser;
using GXPEngine;
using Objects.Enemies;

namespace Scripts
{
    class TestScript : Script
    {
        int lastSpawnTime = 0;
        const int spawnInterval = 2000;
        Random rand = new Random();

        public TestScript(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
        {

        }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            Console.WriteLine("testScript started");
        }

        public void Update()
        {
            if(Time.time > lastSpawnTime + spawnInterval)
            {
                lastSpawnTime = Time.time;
                float angle = (float)(rand.NextDouble() * 360);
                Console.WriteLine("spawning bug at " + angle);
                ApproachingEnemy enemy = new ApproachingEnemy("sprites/bug3.png", 1, 1, 1, angle);
                parentScene.AddChild(enemy);
                enemy.initialize(parentScene);
            }
        }
    }
}
