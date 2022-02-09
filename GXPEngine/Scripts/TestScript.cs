using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using TiledMapParser;

namespace Scripts
{
    class TestScript : Script
    {
        SerialPort controller;

        public TestScript(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
        {

        }

        public override void initialize(Scene parentScene)
        {
            base.initialize(parentScene);
            Console.WriteLine("testScript initialized");

            // Get a list of serial port names.
            string[] ports = SerialPort.GetPortNames();

            Console.WriteLine("The following serial ports were found:");

            // Display each port name to the console.
            foreach (string port in ports)
            {
                Console.WriteLine(port);
            }

            if (ports.Length > 0)
            {
                controller = new SerialPort(ports[0]);
                controller.Open();
            }
        }

        public void Update()
        {
            //if there is a controller, read the serial data
            if(controller != null)
            {
                string input = controller.ReadExisting();
                Console.WriteLine(input);
            }
        }
    }
}
