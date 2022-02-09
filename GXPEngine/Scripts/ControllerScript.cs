using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using TiledMapParser;

namespace Scripts
{
    class ControllerScript : Script
    {
        SerialPort controller;

        //public variables
        public int stickPosition = 0;
        public bool shootButtonDown = false;

        //local variables
        bool prevShootButton = false;
        int offset = 512;

        //Incoming signals
        int pinReading = 0;
        bool shootButton = false;


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
                        string[] args = line.Split(',');
                        parseIntOrDefault(args[0], out pinReading, pinReading);
                        parseBoolOrDefault(args[1], out shootButton, false);
                    }
                }
            }
            else
            {
                findController();
            }

            shootButtonDown = !prevShootButton && shootButton;
            prevShootButton = shootButton;
            stickPosition = pinReading - offset;

        }

        void findController()
        {
            // Get a list of serial port names.
            string[] ports = SerialPort.GetPortNames();

            if (ports.Length > 0)
            {
                try
                {
                    controller = new SerialPort(ports[0]);
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

        public void callibrate()
        {
            offset = pinReading;
        }
    }
}
