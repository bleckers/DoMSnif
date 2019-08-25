using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using Windows.Devices.Enumeration;
using Windows.Security.Cryptography;
using System.Threading.Tasks;

/*To use the WinRT APIs, add two references:

    C:\Program Files (x86)\Windows Kits\10\UnionMetadata\Windows.winmd

    C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework.NETCore\v4.5\System.Runtime.WindowsRuntime.dll

Note: #2 Depends on the framework version you are using!*/

namespace KS0108
{
    public partial class FormLCD : Form
    {
        public FormLCD()
        {
            InitializeComponent();
        }

        List<LCDCommand> LCDStreamData = new List<LCDCommand>();

        Int32 currentLCDCommandNum = 0;

        bool debug = false;

        KS0108_LCD LCD = new KS0108_LCD();

        Thread dataProcess;

        Thread csvParseProcess;

        Color LCDColorDark = Color.Black;
        Color LCDColorLight = Color.White;

        private void buttonBrowse_Click(object sender, EventArgs e)
        {

            if (openFileDialogCSV.ShowDialog() == DialogResult.OK)
            {
                LCDStreamData = new List<LCDCommand>();

                //buttonRun.Enabled = false;
                textBoxFilename.Text = "";

                toolStripStatusLabel.Text = "Loading...";

                this.Refresh();

                //Get the path of specified file
                String filePath = openFileDialogCSV.FileName;

                //Load loading thread

                ThreadStart csvParseProcessStart = new ThreadStart(delegate { ParseCSV(filePath); });

                csvParseProcess = new Thread(csvParseProcessStart);

                csvParseProcess.Start();

            }
        }

        int lineCount;
        double currentLine;
        int prevProgressBarValue;
        int progressBarMax = 0;


        private void ParseCSV(String filePath)
        {
            string[] prevFields = { "1", "1", "1", "1", "1" };

            lineCount = File.ReadLines(filePath).Count();

            currentLine = 0;
            prevProgressBarValue = 0;

            foreach (String line in File.ReadLines(filePath))
            {

                string[] fields = line.Split(',').ToArray();

                currentLine++;

                LCDCommand newCommand;
                try
                {
                    if (uint.Parse(fields[4]) == 0 && uint.Parse(prevFields[4]) == 1 && int.Parse(fields[3]) == 0) 
                                                                                //If E (H->L) transition and read command
                    {
                        //Update every major tick
                        int progressBarValue = (int)(currentLine / lineCount * progressBarMax);
                        if (lineCount != 0 && prevProgressBarValue < progressBarValue)
                        {
                            prevProgressBarValue = progressBarValue;
                            this.Invoke((MethodInvoker)delegate
                            {
                                this.toolStripProgressBar.Value = progressBarValue;
                            });
                        }

                        newCommand = new LCDCommand();
                        newCommand.time = double.Parse(fields[0]);

                        newCommand.data = uint.Parse(fields[5]) |
                                          uint.Parse(fields[6]) << 1 |
                                          uint.Parse(fields[7]) << 2 |
                                          uint.Parse(fields[8]) << 3 |
                                          uint.Parse(fields[9]) << 4 |
                                          uint.Parse(fields[10]) << 5 |
                                          uint.Parse(fields[11]) << 6 |
                                          uint.Parse(fields[12]) << 7;

                        newCommand.di = int.Parse(fields[2]) == 1;
                        newCommand.rw = int.Parse(fields[3]) == 1;
                        newCommand.e = int.Parse(fields[4]) == 1;
                        newCommand.csa = int.Parse(fields[13]) == 1;
                        newCommand.csb = int.Parse(fields[14]) == 1;
                        newCommand.nreset = int.Parse(fields[15]) == 1;


                        LCDStreamData.Add(newCommand);
                    }
                    prevFields = fields;
                }
                catch (Exception ex)
                { }
            }

            this.Invoke((MethodInvoker)delegate
            {
                if (LCDStreamData.Count > 0)
                {

                    textBoxFilename.Text = Path.GetFileName(filePath);
                    //buttonRun.Enabled = true;
                }
                else
                {
                    textBoxFilename.Text = "";
                    //buttonRun.Enabled = false;
                }

                toolStripStatusLabel.Text = "";
                currentLine = 0;
                toolStripProgressBar.Value = 0;
            });
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            checkBoxSlow.Enabled = true;
            toolStripStatusLabel.Text = "";

            debug = checkBoxDebug.Checked;

            if (textBoxFilename.Text == "") {
                if (buttonRun.Text == "Run")
                {
                    //Use bluetooth
                    dataProcess = new Thread(new ThreadStart(processBluetoothCommands));
                    dataProcess.Start();
                    toolStripProgressBar.Enabled = false;
                    buttonRun.Text = "Stop";
                    toolStripStatusLabel.Text = "Connecting to Bluetooth...";
                    checkBoxSlow.Enabled = false;
                  
                }
                else
                {
                    //timerLCD.Stop();

                    try
                    {
                        dataProcess.Abort();
                    }
                    catch (Exception ex) { }
                    buttonRun.Text = "Run";
                    try
                    {
                        BluetoothUARTService.Dispose();
                    }
                    catch (Exception ex) { }
                }
            }
            else
            {
                if (LCDStreamData.Count > 0)
                {
                    if (buttonRun.Text == "Run")
                    {
                        //timerLCD.Start();
                        dataProcess = new Thread(new ThreadStart(processCommands));
                        dataProcess.Start();
                        toolStripProgressBar.Enabled = false;
                        buttonRun.Text = "Stop";
                    }
                    else
                    {
                        //timerLCD.Stop();
                        try
                        {
                            dataProcess.Abort();
                        }
                        catch (Exception ex) { }
                        buttonRun.Text = "Run";
                    }
                }
                else
                {
                    toolStripStatusLabel.Text = "No file loaded!";
                }
            }
        }


        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                /*graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;*/

using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private static void Sleep(double durationSeconds)
        {
            var durationTicks = Math.Round(durationSeconds * Stopwatch.Frequency);
            var sw = Stopwatch.StartNew();

            while (sw.ElapsedTicks < durationTicks)
            {

            }
        }


        //https://stackoverflow.com/questions/46504859/unable-to-read-ble-characteristic-with-windows-bluetooth-apis

        GattCharacteristic gattCharacteristic = null;
        GattDeviceService BluetoothUARTService = null;
        int LCDTimeout = 0;

        bool dataSyncLock = false;

        DisplayData display = new DisplayData();

        private void Charac_ValueChangedAsync(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            //LCDTimeout = 0;
            CryptographicBuffer.CopyToByteArray(args.CharacteristicValue, out byte[] data);

            uint hashFound = 0;

            if (dataSyncLock == false)
            {
                
                foreach (byte b in data) {
                    if (b.Equals((byte)'#'))
                    {
                        hashFound++;
                    }
                }

                if (hashFound >= display.GetBufferRounding())
                {
                    dataSyncLock = true;
                    display.ResetByteCount();
                }
            }
            else
            {
                foreach (byte b in data)
                {
                    if (!display.AddNewByte(b))
                    {
                        if (b.Equals((byte)'#'))
                            hashFound++;
                    }
                }

                if (hashFound >= display.GetBufferRounding())
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        pictureBoxLCD.Image = ResizeImage(ChangeColor(display.GetImage(!checkBoxInvert.Checked), LCDColorDark, LCDColorLight), pictureBoxLCD.Width, pictureBoxLCD.Height);
                        display.ResetByteCount();
                    });
                }
                else if (display.GetByteCount() >= display.GetMaxBytes())
                {
                    
                    dataSyncLock = false;
                    display.ResetByteCount();
                }
            }
        }

        

        private async void GetBytesAsync()
        {

            Guid guid = new Guid("6E400001-B5A3-F393-E0A9-E50E24DCCA9E"); // Base UUID for UART service
            Guid charachID = new Guid("6E400003-B5A3-F393-E0A9-E50E24DCCA9E"); // RX characteristic UUID

            var Services = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(guid), null);
            BluetoothUARTService = await GattDeviceService.FromIdAsync(Services[0].Id);
            while (BluetoothUARTService == null) { }
            gattCharacteristic = BluetoothUARTService.GetCharacteristics(charachID)[0];

            try
            {
                GattReadResult result = await gattCharacteristic.ReadValueAsync();

                GattCharacteristicProperties properties = gattCharacteristic.CharacteristicProperties;
                if (properties.HasFlag(GattCharacteristicProperties.Read))
                {
                    //Console.WriteLine("This characteristic supports reading from it.");
                }
                if (properties.HasFlag(GattCharacteristicProperties.Write))
                {
                    //Console.WriteLine("This characteristic supports writing.");
                }
                if (properties.HasFlag(GattCharacteristicProperties.Notify))
                {
                    //Console.WriteLine("This characteristic supports subscribing to notifications.");
                }
                try
                {
                    //Write the CCCD in order for server to send notifications.               
                    var notifyResult = await gattCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                                                              GattClientCharacteristicConfigurationDescriptorValue.Notify);
                    if (notifyResult == GattCommunicationStatus.Success)
                    {

                        //Console.WriteLine("Successfully registered for notifications");
                        this.Invoke((MethodInvoker)delegate
                        {
                           
                            toolStripStatusLabel.Text = "Connected";

                            //Turn on, since it's off by default and we could be mid transaction at this point.
                            LCD.DisplayOn();
                            
                        });
                    }
                    else
                    {
                        Console.WriteLine($"Error registering for notifications: {notifyResult}");
                        this.Invoke((MethodInvoker)delegate
                        {
                            try
                            {
                                dataProcess.Abort();
                            }
                            catch (Exception ex) { }

                            checkBoxSlow.Enabled = true;

                            buttonRun.Text = "Run";
                            toolStripStatusLabel.Text = "Connection Failed!";
                            try
                            {
                                BluetoothUARTService.Dispose();
                            }
                            catch (Exception ex) { }
                        });
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine(ex.Message);
                    this.Invoke((MethodInvoker)delegate
                    {
                        try
                        {
                            dataProcess.Abort();
                        }
                        catch (Exception ex2) { }
                        buttonRun.Text = "Run";
                        checkBoxSlow.Enabled = true;
                        toolStripStatusLabel.Text = "Connection Failed!";
                        try
                        {
                            BluetoothUARTService.Dispose();
                        }
                        catch (Exception ex2) { }
                    });
                }

                gattCharacteristic.ValueChanged += Charac_ValueChangedAsync;

            }
            catch (Exception blue) { }
            // }
        }

        public void processBluetoothCommands()
        {
            GetBytesAsync();
        }

        public void processCommands()
        {
            
            int timeoutValue = 1; //Display blocks of 8
            double slower = 0.0;

            while (currentLCDCommandNum < LCDStreamData.Count)
            {
                //While not a display change pixel command
                while (!LCD.ProcessCommand((LCDCommand)LCDStreamData[currentLCDCommandNum], debug)
                    && currentLCDCommandNum < LCDStreamData.Count)
                {
                    currentLCDCommandNum++;

                    if (currentLCDCommandNum < LCDStreamData.Count)
                    {
                        Sleep(((LCDCommand)LCDStreamData[currentLCDCommandNum]).time
                        - ((LCDCommand)LCDStreamData[currentLCDCommandNum - 1]).time + slower);
                    }
                }

                bool longSleep = false;

                //Check if there is a long sleep between commands to be able to redraw correctly on a "last" 
                //command on a page if there is still LCD timeout to go
                if (currentLCDCommandNum + 1 < LCDStreamData.Count)
                {
                    if ((((LCDCommand)LCDStreamData[currentLCDCommandNum + 1]).time
                    - ((LCDCommand)LCDStreamData[currentLCDCommandNum]).time) > 0.01)
                    {
                        longSleep = true;
                    }
                }

                LCDTimeout++;
                if (LCDTimeout > timeoutValue || longSleep
                    || currentLCDCommandNum >= LCDStreamData.Count) //If sleep > 10ms or if last command, then redraw
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate
                    {

                        pictureBoxLCD.Image = ResizeImage(ChangeColor(LCD.GetImage(!checkBoxInvert.Checked), LCDColorDark, LCDColorLight), pictureBoxLCD.Width, pictureBoxLCD.Height);

                        debug = checkBoxDebug.Checked;

                        toolStripStatusLabelCPS.Text = LCD.GetCommandsPerSecond().ToString();

                        if (checkBoxSlow.Checked)
                        {
                            timeoutValue = 1; //Show each column drawn
                            slower = 0.001;
                        }
                        else
                        {
                            timeoutValue = 8; //Display blocks of 8
                            slower = 0.0;
                        }

                    });
                        LCDTimeout = 0;
                    }
                    catch (Exception ex) { }
                }

                //The loop above only increments non draw commands, the last one of the loop won't increment or delay.
                currentLCDCommandNum++;

                if (currentLCDCommandNum < LCDStreamData.Count)
                {
                    Sleep(((LCDCommand)LCDStreamData[currentLCDCommandNum]).time
                    - ((LCDCommand)LCDStreamData[currentLCDCommandNum - 1]).time + slower);
                }
            }

            this.Invoke((MethodInvoker)delegate
            {
                buttonRun.Text = "Run";
                currentLCDCommandNum = 0;
            });
        }

        private void FormLCD_Load(object sender, EventArgs e)
        {
            //buttonRun.Enabled = false;
            progressBarMax = toolStripProgressBar.Maximum;
            blackWhiteToolStripMenuItem.Checked = true;
        }

        private void FormLCD_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { dataProcess.Abort(); } catch (Exception ex) { }

            try { csvParseProcess.Abort(); } catch (Exception ex) { }

            try
            {
                BluetoothUARTService.Dispose();
            }
            catch (Exception ex) { }
        }



        public static Bitmap ChangeColor(Bitmap image, Color toColorDark, Color toColorLight)
        {
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetRemapTable(new ColorMap[]
            {
                new ColorMap()
                {
                    OldColor = Color.Black,
                    NewColor = toColorDark,
                },
                new ColorMap()
                {
                    OldColor = Color.White,
                    NewColor = toColorLight,
                }
            }, ColorAdjustType.Bitmap);

            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawImage(
                    image,
                    new Rectangle(Point.Empty, image.Size),
                    0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel,
                    attributes);
            }

            return image;
        }

        void CheckClicked(object sender)
        {
            foreach (ToolStripMenuItem item in colourToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
            }
        ((ToolStripMenuItem)sender).Checked = true;
        }

        private void blackWhiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LCDColorDark = Color.Black;
            LCDColorLight = Color.White;
            CheckClicked(sender);
        }

        private void blueWhiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LCDColorDark = Color.Blue;
            LCDColorLight = Color.White;
            CheckClicked(sender);
        }

        private void darkGreenLightGreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LCDColorDark = Color.DarkGreen;
            LCDColorLight = Color.LightGreen;
            CheckClicked(sender);
        }

        private void blackBlueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LCDColorDark = Color.Black;
            LCDColorLight = Color.LightBlue;
            CheckClicked(sender);
        }

        private void blackGreyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LCDColorDark = Color.Black;
            LCDColorLight = Color.LightGray;
            CheckClicked(sender);
        }
    }


}
