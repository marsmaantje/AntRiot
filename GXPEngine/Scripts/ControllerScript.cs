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

        //public variables
        public int shooterStickPosition = 0;
        public bool shootButtonDown = false;
        public bool shootSpecialDown = false;
        public int defenderStickPosition = 0;
        public bool defenderButtonDown = false;
        public bool defenderSpecialDown = false;
        public bool bothSpecialDown = false;

        //local variables
        public bool prevShootButton = false;
        public bool prevShootSpecial = false;
        public bool prevDefenderButton = false;
        public bool prevDefenderSpecial = false;

        int lastSpecialPress = 0;

        int lastMessageTime = 0;
        const int timeout = 2000;

        //Incoming signals
        int shooterPinReading = 0;
        bool shooterButton = false;
        bool shooterSpecial = false;
        int defenderPinReading = 0;
        bool defenderButton = false;
        bool defenderSpecial = false;


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
            if (Globals.controller != null && Globals.controller.IsOpen)
            {
                string input = Globals.controller.ReadExisting();
                string[] lines = input.Split('\n');

                foreach (string line in lines)
                {
                    if (line.Length > 1)
                    {
                        lastMessageTime = Time.time;
                        string[] args = line.Split(',');
                        if (args.Length >= 6)
                        {
                            parseIntOrDefault(args[0], out shooterPinReading, shooterPinReading);
                            parseBoolOrDefault(args[1], out shooterButton, false);
                            parseIntOrDefault(args[2], out defenderPinReading, defenderPinReading);
                            parseBoolOrDefault(args[3], out defenderButton, false);
                            parseBoolOrDefault(args[4], out shooterSpecial, false);
                            parseBoolOrDefault(args[5], out defenderSpecial, false);

                        }
                    }
                }

                if (Time.time > lastMessageTime + timeout)
                {
                    Console.WriteLine("not recieving anything, connecting to different device");
                    Globals.controller.Close();
                }
            }
            else
            {
                findController();
                lastMessageTime = Time.time;
            }

            shootButtonDown = !prevShootButton && shooterButton;
            shootSpecialDown = !prevShootSpecial && shooterSpecial;
            prevShootButton = shooterButton;
            prevShootSpecial = shooterSpecial;
            shooterStickPosition = shooterPinReading;

            defenderButtonDown = !prevDefenderButton && defenderButton;
            defenderSpecialDown = !prevDefenderSpecial && defenderSpecial;
            prevDefenderButton = defenderButton;
            prevDefenderSpecial = defenderSpecial;
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
                    Globals.controller = new SerialPort(ports[ports.Length - 1]);
                    Globals.controller.Open();
                }
                catch (Exception e)
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
