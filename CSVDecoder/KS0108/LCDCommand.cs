using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS0108
{
    class LCDCommand
    {
        public double time;
        public uint data;
        public bool e;
        public bool rw;
        public bool di; //RS
        public bool csa;
        public bool csb;
        public bool nreset;

        public bool GetDataBit(int bitNum)
        {
            return ((data >> bitNum) & 0x01) == 1;
        }


        public override String ToString()
        {
            return time.ToString("0.000000") + " "
                + (di ? 1 : 0).ToString() + " "
                + (rw ? 1 : 0).ToString() + " "
                + (this.GetDataBit(7) ? 1 : 0).ToString() + " "
                + (this.GetDataBit(6) ? 1 : 0).ToString() + " "
                + (this.GetDataBit(5) ? 1 : 0).ToString() + " "
                + (this.GetDataBit(4) ? 1 : 0).ToString() + " "
                + (this.GetDataBit(3) ? 1 : 0).ToString() + " "
                + (this.GetDataBit(2) ? 1 : 0).ToString() + " "
                + (this.GetDataBit(1) ? 1 : 0).ToString() + " "
                + (this.GetDataBit(0) ? 1 : 0).ToString() + " "
                + (csa ? 1 : 0).ToString() + " "
                + (csb ? 1 : 0).ToString() + " "
                + (nreset ? 1 : 0).ToString() + " ";
        }
    }
}
