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
        int spawnInterval = 1500;
        Random rand = new Random();
        int waveInterval = 30000;
        int startTime = 0;
        int waves = 4;

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

                        Console.WriteLine("wave 1");
                        spawnBeetle();
                        break;

                    case 1: //wave 1
                        Console.WriteLine("wave 2");
                        switch (rand.Next(0,2))
                        {
                            case 0:
                                spawnBeetle();
                                break;
                            case 1:
                                spawnYellowBug();
                                break;
                        }
                        spawnInterval = 1200;
                        break;

                    case 2: //wave 2
                        Console.WriteLine("wave 3");
                        switch (rand.Next(0, 3))
                        {
                            case 0:
                                spawnBeetle();
                                break;
                            case 1:
                                spawnLadyBug();
                                break;
                            case 2:
                                spawnYellowBug();
                                break;
                        }
                        spawnInterval = 1000;
                        break;

                    case 3:
                        Console.WriteLine("wave 4");
                        switch (rand.Next(0, 4))
                        {
                            case 0:
                                spawnBeetle();
                                break;
                            case 1:
                                spawnLadyBug();
                                break;
                            case 2:
                                spawnYellowBug();
                                break;
                            case 3:
                                spawnGreenBug();
                                break;
                        }
                        spawnInterval = 900;
                        break;

                    case 4:
                        Console.WriteLine("wave 5");
                        switch (rand.Next(0, 5))
                        {
                            case 0:
                                spawnBeetle();
                                break;
                            case 1:
                                spawnLadyBug();
                                break;
                            case 2:
                                spawnYellowBug();
                                break;
                            case 3:
                                spawnGreenBug();
                                break;
                            case 4:
                                spawnWasp();
                                break;
                        }
                        spawnInterval = 500;
                        break;
                }

            }

            if ((Time.time / 300) % 2 == 0
                && startTime + (currentWave * waveInterval) + 3000 > Time.time)
            {
                parentScene.ui.showText("Wave " + (currentWave + 1), 1);
            }

            if (currentWave == waves && startTime + (currentWave * waveInterval) + 3000 > Time.time)
                parentScene.ui.showText("Final Wave!", 2);
        }

        void spawnBeetle()
        {
            float angle = (float)(rand.Next(0, 20) * 18);
            Enemy enemy = new ApproachingEnemy("sprites/enemy.png", 4, 1, 0, angle, -1, 25, 4, 1);
            parentScene.AddChild(enemy);
            enemy.initialize(parentScene);
        }

        void spawnLadyBug()
        {
            float angle = (float)(rand.Next(0, 20) * 18);
            Enemy enemy = new ApproachingEnemy("sprites/enemies.png", 4, 2, 0, angle, -1, 25, 4, 2);
            parentScene.AddChild(enemy);
            enemy.initialize(parentScene);
        }

        void spawnYellowBug()
        {
            float angle = (float)(rand.Next(0, 20) * 18);
            Enemy enemy = new ShootingEnemy("sprites/enemies2.png", 4, 2, 0, angle, -1, 25, 4);
            parentScene.AddChild(enemy);
            enemy.initialize(parentScene);
        }

        void spawnGreenBug()
        {
            float angle = (float)(rand.Next(0, 20) * 18);
            Enemy enemy = new FlyingEnemy("sprites/enemies.png", 4, 2, 6, angle, -1, 25, 2);
            parentScene.AddChild(enemy);
            enemy.initialize(parentScene);
        }

        void spawnWasp()
        {
            float angle = (float)(rand.Next(0, 20) * 18);
            Enemy enemy = new FlyingEnemy("sprites/enemies.png", 4, 2, 4, angle, -1, 25, 2, killOnImpact:false, speed:60);
            parentScene.AddChild(enemy);
            enemy.initialize(parentScene);
        }
    }
}
