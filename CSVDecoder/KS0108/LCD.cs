using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace KS0108
{
    class KS0108_LCD
    {
        Controller[] controllers = new Controller[2];

        public KS0108_LCD()
        {
            controllers[0] = new Controller();
            controllers[1] = new Controller();
        }

        void SetPage(uint page, bool cs1, bool cs2)
        {
            if (cs1) controllers[0].SetPage(page);
            if (cs2) controllers[1].SetPage(page);
        }

        void SetAddress(uint column, bool cs1, bool cs2)
        {
            if (cs1) controllers[0].SetAddress(column);
            if (cs2) controllers[1].SetAddress(column);
        }

        void IncrementAddress(bool cs1, bool cs2)
        {
            if (cs1) controllers[0].IncrementAddress();
            if (cs2) controllers[1].IncrementAddress();
        }

        void WriteColumnData(byte data, bool cs1, bool cs2)
        {
            if (cs1) controllers[0].WriteColumnData(data);
            if (cs2) controllers[1].WriteColumnData(data);
        }

        public void Clear(bool cs1, bool cs2)
        {
            if (cs1) controllers[0].Clear();
            if (cs2) controllers[1].Clear();

        }

        bool displayOn = false;

        public Bitmap GetImage(bool invert)
        {
            Bitmap A = controllers[0].GetBitMap(invert);
            Bitmap B = controllers[1].GetBitMap(invert);
            Bitmap bitmap = new Bitmap(128, 64);
            if (displayOn)
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    try
                    {
                        g.DrawImage(A, 0, 0);
                        g.DrawImage(B, A.Width, 0);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return bitmap;
        }

        public void DisplayOn()
        {
            displayOn = true;
        }

        private int currentCommands;
        private int commandsPerSecond;

        long previousTime;

        public int GetCommandsPerSecond()
        {
            return commandsPerSecond;
        }


        //Return display write
        public bool ProcessCommand(LCDCommand command, bool debug)
        {
            currentCommands++;
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            if (milliseconds - previousTime > 1000)
            {
                previousTime = milliseconds;
                commandsPerSecond = currentCommands;
                currentCommands = 0;
            }

            /*Reset
            Reset can be initialized system by setting RSTB terminal at low level when turning power on, receiving instruction from MPU.
            When RSTB becomes low, following procedure is occured.
            1.Display off
            2.Display start line register become set by 0.(Z - address 0)
            While RSTB is low, any instruction except status read can be accepted. Reset status appers at DB4.After DB4 is low, any
            instruction can be accepted.*/

            if (!command.nreset)
            {
                this.Clear(command.csa, command.csb);
                if (debug) System.Console.WriteLine("Reset         : " + command.ToString());
            }
            
            //Read command
            if (command.di && command.rw)
            {
                //this.IncrementAddress(command.csa, command.csb);
                if (debug) System.Console.WriteLine("Read          : " + command.ToString());
                return false;
            }

            //Display on/off
            if (!command.di && !command.rw && !command.GetDataBit(7) && !command.GetDataBit(6)
                && command.GetDataBit(5) && command.GetDataBit(4) && command.GetDataBit(3)
                && command.GetDataBit(2) && command.GetDataBit(1))
            {
                displayOn = command.GetDataBit(0);
                if (debug) System.Console.WriteLine("Display On/Off: " + command.ToString());
                
                return true;
            }

            //Clear
            if (!command.di && !command.rw && command.data == 0x01)
            {
                this.Clear(command.csa, command.csb);
                if (debug) System.Console.WriteLine("Clear         : " + command.ToString());
                return true;
            }

            //Display Start
            if (!command.di && !command.rw && command.GetDataBit(7) && command.GetDataBit(6))
            {
                if (debug) System.Console.WriteLine("Display Start:  " + command.ToString());

                return false;
            }

            //Set address/column
            if (!command.di && !command.rw && !command.GetDataBit(7) && command.GetDataBit(6))
            {
                this.SetAddress((byte)(command.data & 0x3f), command.csa, command.csb);
                if (debug) System.Console.WriteLine("Set Addr      : " + command.ToString());
                return false;
            }

            //Set Page
            if (!command.di && !command.rw && command.GetDataBit(7) && !command.GetDataBit(6))
            {
                this.SetPage((byte)(command.data & 0x07), command.csa, command.csb);
                if (debug) System.Console.WriteLine("Set Page      : " + command.ToString());
                return false;
            }

            //Write Display Data
            if (command.di && !command.rw)
            {
                this.WriteColumnData((byte)(command.data & 0xff), command.csa, command.csb);
                if (debug) System.Console.WriteLine("Write         : " + command.ToString());
                return true;
            }

            //Unknown command
            if (debug) System.Console.WriteLine("Unknown Cmd   : " + command.ToString());
            
            return false;

        }
    }

    class Controller
    {
        uint currentPage = 0;
        uint currentColumn = 0;

        Page[] pages = new Page[8];

        public Controller()
        {
            for (int i = 0; i < 8; i++)
            {
                pages[i] = new Page();
            }
        }

        public void Clear()
        {
            for (int i = 0; i < 8; i++)
            {
                pages[i] = new Page();
            }
        }

        public void SetPage(uint page)
        {
            if (page < 8) currentPage = page;
        }

        public void SetAddress(uint column)
        {
            if (column < 64) currentColumn = column;
        }

        //Column is address
        public uint WriteColumnData(byte data)
        {
            pages[currentPage].SetColumn(currentColumn, data);

            //Auto increment
            currentColumn++;
            if (currentColumn >= 64)
            {
                currentColumn = 0;
            }

            return currentColumn;
        }

        public uint IncrementAddress()
        {
            //Auto increment
            currentColumn++;
            if (currentColumn >= 64)
            {
                currentColumn = 0;
            }

            return currentColumn;
        }

        public Bitmap GetBitMap(bool invert)
        {
            byte[] byteArray = new byte[4096];
            int currentIndex = 0;
            byte column = 0;
            int invertByte = 0;
            if (invert) invertByte = 0x01;


            foreach (Page p in pages)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (uint i = 0; i < 64; i++)
                    {
                        column = p.GetColumn(i);
                        byteArray[currentIndex] = (byte)(((column >> j) & 0x01 ^ invertByte) * 255);

                        currentIndex++;
                    }
                }                
            }
             
            int width = 64;
            int height = 64;

            var b = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            ColorPalette ncp = b.Palette;
            for (int i = 0; i < 256; i++)
                ncp.Entries[i] = Color.FromArgb(255, i, i, i);
            b.Palette = ncp;

            var BoundsRect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = b.LockBits(BoundsRect,
                                            ImageLockMode.WriteOnly,
                                            b.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytes = bmpData.Stride * b.Height;

            // fill in rgbValues, e.g. with a for loop over an input array

            Marshal.Copy(byteArray, 0, ptr, bytes);
            b.UnlockBits(bmpData);
            return b;

        }


    }

    class Page
    {
        //Page is byte array of 64x8bit columns
        byte[] page = new byte[64];

        public void SetColumn(uint column, byte data) { if (column < 64) page[column] = data; }

        public byte GetColumn(uint column) { return page[column]; }
    }
}
