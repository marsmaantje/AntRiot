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
        public int stickPosition = 0;
        int pinReading = 0;
        int offset = 512;


        public ControllerScript(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj) { }

        public ControllerScript() : base() { }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            Console.WriteLine("ControllerScript initialized");


        }

        public void Update()
        {
            //if there is a controller, read the serial data
            if (controller != null && controller.IsOpen)
            {
                string input = controller.ReadExisting();
                string[] args = input.Split('\n');

                foreach (string arg in args)
                {
                    if (arg.Length > 0)
                    {
                        parseIntOrDefault(arg, out pinReading, pinReading);
                    }
                }
            }
            else
            {
                findController();
            }

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

        public void callibrate()
        {
            offset = pinReading;
        }
    }
}
