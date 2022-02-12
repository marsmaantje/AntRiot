using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using GXPEngine;
using TiledMapParser;

namespace Scripts
{
    class ControllerScript : Script
    {
        SerialPort controller;

        //public variables
        public int shooterStickPosition = 0;
        public bool shootButtonDown = false;
        public int defenderStickPosition = 0;
        public bool defenderButtonDown = false;

        //local variables
        public bool prevShootButton = false;
        public bool prevDefenderButton = false;

        int lastMessageTime = 0;
        const int timeout = 2000;

        //Incoming signals
        int shooterPinReading = 0;
        bool shooterButton = false;
        int defenderPinReading = 0;
        bool defenderButton = false;


        public ControllerScript(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj) { }

        public ControllerScript() : base() { }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            Console.WriteLine("ControllerScript initialized");
            this.visible = false;


        }

        public void Update()
        {
            //if there is a controller, read the serial data
            if (controller != null && controller.IsOpen)
            {
                string input = controller.ReadExisting();
                string[] lines = input.Split('\n');

                foreach (string line in lines)
                {
                    if (line.Length > 1)
                    {
                        lastMessageTime = Time.time;
                        string[] args = line.Split(',');
                        if (args.Length >= 4)
                        {
                            parseIntOrDefault(args[0], out shooterPinReading, shooterPinReading);
                            parseBoolOrDefault(args[1], out shooterButton, false);
                            parseIntOrDefault(args[2], out defenderPinReading, defenderPinReading);
                            parseBoolOrDefault(args[3], out defenderButton, false);
                        }
                    }
                }

                if (Time.time > lastMessageTime + timeout)
                {
                    Console.WriteLine("not recieving anything, connecting to different device");
                    controller.Close();
                }
            }
            else
            {
                findController();
            }

            shootButtonDown = !prevShootButton && shooterButton;
            prevShootButton = shooterButton;
            shooterStickPosition = shooterPinReading;
            defenderButtonDown = !prevDefenderButton && defenderButton;
            prevDefenderButton = defenderButton;
            defenderStickPosition = defenderPinReading;

        }

        void findController()
        {
            // Get a list of serial port names.
            string[] ports = SerialPort.GetPortNames();

            if (ports.Length > 0)
            {
                try
                {
                    controller = new SerialPort(ports[ports.Length - 1]);
                    controller.Open();
                } catch (Exception e)
                { }
            }
        }

        void parseIntOrDefault(string arg, out int output, int fallback)
        {
            int result = 0;

            output = Int32.TryParse(arg, out result) ? result : fallback;
        }

        void parseBoolOrDefault(string arg, out bool output, bool fallback)
        {
            int result = 0;
            Int32.TryParse(arg, out result);
            output = result > 0;
        }
    }
}
