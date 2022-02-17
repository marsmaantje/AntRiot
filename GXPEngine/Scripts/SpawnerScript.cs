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
    class SpawnerScript : Script
    {
        int lastSpawnTime = 0;
        const int spawnInterval = 1500;
        Random rand = new Random();
        int waveInterval = 5000;
        int startTime = 0;
        int waves = 2;

        public SpawnerScript(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
        {

        }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            Console.WriteLine("spawnerScript started");
            startTime = Time.time;
        }

        public void Update()
        {
            int currentWave = Math.Min(waves, (Time.time - startTime) / waveInterval);
            if (Time.time > lastSpawnTime + spawnInterval)
            {
                lastSpawnTime = Time.time;
                Console.WriteLine(currentWave);
                switch (currentWave)
                {
                    case 0: //wave 0
                        Console.WriteLine("wave 0");
                        break;

                    case 1: //wave 1
                        Console.WriteLine("wave 1");
                        break;

                    case 2: //wave 2
                        Console.WriteLine("wave 2");
                        break;
                }

            }

            if ((Time.time / 300) % 2 == 0
                && startTime + (currentWave * waveInterval) + 3000 > Time.time)
            {
                parentScene.ui.showText("Wave " + currentWave, 1);
            }
        }

        void spawnBeetle()
        {
            float angle = (float)(rand.Next(0, 20) * 18);
            Enemy enemy = new ApproachingEnemy("sprites/enemies.png", 4, 2, 6, angle, -1, 25, 2, 1);
            parentScene.AddChild(enemy);
            enemy.initialize(parentScene);
        }

        void spawnLadyBug()
        {
            float angle = (float)(rand.Next(0, 20) * 18);
            Enemy enemy = new ApproachingEnemy("sprites/enemies.png", 4, 2, 6, angle, -1, 25, 2, 2);
            parentScene.AddChild(enemy);
            enemy.initialize(parentScene);
        }

        void spawnYellowBug()
        {
            float angle = (float)(rand.Next(0, 20) * 18);
            Enemy enemy = new ShootingEnemy("sprites/enemies.png", 4, 2, 0, angle, -1, 25, 4);
            parentScene.AddChild(enemy);
            enemy.initialize(parentScene);
        }

        void spawnGreenBug()
        {
            float angle = (float)(rand.Next(0, 20) * 18);
            Enemy enemy = new FlyingEnemy("sprites/enemies.png", 4, 2, 0, angle, -1, 25, 4);
            parentScene.AddChild(enemy);
            enemy.initialize(parentScene);
        }
    }
}
