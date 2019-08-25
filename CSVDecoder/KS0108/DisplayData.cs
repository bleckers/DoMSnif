using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KS0108
{
    class ControllerDisplayData
    {
        private const int DISPLAY_DATA_BYTES = 512;

        public byte[] data = new byte[DISPLAY_DATA_BYTES];

        public ControllerDisplayData() {

        }

        public Bitmap GetBitMap(bool invert)
        {
            byte[] byteArray = new byte[4096];
            int currentIndex = 0;
            int invertByte = 0;
            if (invert) invertByte = 0x01;

            for (int p = 0; p < 8; p++)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (int c = 0; c < 64; c++)
                    {

                        byteArray[currentIndex] = (byte)(((data[(p * 64) + c] >> j) & 0x01 ^ invertByte) * 255);

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

    };



    class DisplayData
    {
        UInt16 currentByte = 0;
        private const int TOTAL_BYTES = 1024;
        private const int BUFFER_ROUNDING = 16; //Round/pad total bytes to bluetooth buffer size 20

        private const int DISPLAY_DATA_BYTES = 512;

        private ControllerDisplayData[] display = new ControllerDisplayData[2];

        public DisplayData()
        {
            display[0] = new ControllerDisplayData();
            display[1] = new ControllerDisplayData();
        }

        public int GetBufferRounding()
        {
            return BUFFER_ROUNDING;
        }

        public int GetMaxBytes()
        {
            return TOTAL_BYTES;
        }

        public Bitmap GetImage(bool invert)
        {
            Bitmap A = display[0].GetBitMap(invert);
            Bitmap B = display[1].GetBitMap(invert);
            Bitmap bitmap = new Bitmap(128, 64);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                try
                {
                    g.DrawImage(A, 0, 0);
                    g.DrawImage(B, A.Width, 0);
                }
                catch (Exception)
                {

                }
            }
            

            return bitmap;
        }

        public bool AddNewByte(byte byteToAdd)
        {
            if (currentByte < TOTAL_BYTES)
            {
                if (currentByte < DISPLAY_DATA_BYTES)
                {
                    display[0].data[currentByte] = byteToAdd;
                }
                else
                {
                    display[1].data[currentByte - DISPLAY_DATA_BYTES] = byteToAdd;
                }
                currentByte++;
                return true;
            }

            return false;
        }

        public uint GetNextByte()
        {
            uint byteToReturn;
            if (currentByte < TOTAL_BYTES)
            {
                if (currentByte < DISPLAY_DATA_BYTES)
                {
                    byteToReturn = display[0].data[currentByte];
                }
                else
                {
                    byteToReturn = display[1].data[currentByte - DISPLAY_DATA_BYTES];
                }
                currentByte++;
            }
            else
            {
                if (currentByte < TOTAL_BYTES + BUFFER_ROUNDING) currentByte++;

                byteToReturn = '#';
            }

            return byteToReturn;
        }

        public UInt16 GetByteCount()
        {
            return currentByte;
        }

        public UInt16 GetTotalBytes()
        {
            return TOTAL_BYTES + BUFFER_ROUNDING;
        }

        public void ResetByteCount()
        {
            currentByte = 0;
        }

    };
}
