using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports; // IO Port to communicate with Arduino

namespace temp_app
{
    public partial class TempDisplay : Form
    {
        // define datatypes and  variables
        private SerialPort Comport;

        private DateTime datetime;
        private String in_data;

        public TempDisplay()
        {
            InitializeComponent();
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            // Start connection with Serial port

            Comport = new SerialPort();
            Comport.BaudRate = 9600;
            Comport.PortName = portname_cb.Text;
            Comport.Parity = Parity.None;
            Comport.DataBits = 8;
            Comport.StopBits = StopBits.One;
            Comport.DataReceived += Comport_DataReceived;

            // open COM Port and start empty text box to disaplay

            try
            {
                Comport.Open();
                data_tb.Text = "";
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message, "COM PORT Error");
            }
        }

        private void Comport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Read incoming serial data and read line by line

            in_data = Comport.ReadLine();
            this.Invoke(new EventHandler(displaydata_event)); // pass that data to displaydata_event
        }

        private void displaydata_event(object sender, EventArgs e)
        {
            // Display Data in textbox

            datetime = DateTime.Now;
            string time = datetime.Hour + "HH " + ":" + datetime.Minute + "MM " + ":" + datetime.Second + "SS ";
            data_tb.AppendText(time + "\t \t" + in_data + "\n");
        }

        private void stop_button_Click(object sender, EventArgs e)
        {
            // Stop button event

            try
            {
                Comport.Close();
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message, "Error");
            }
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            // Save all Data event

            try
            {
                string pathfile = @"C:\Users\bhanuka kamantha\Desktop\DATA";
                string filename = "light_data.txt";
                System.IO.File.WriteAllText(pathfile + filename, data_tb.Text);
                MessageBox.Show("Data has benn saved" + pathfile, "save File");
            }
            catch (Exception ex3)
            {
                MessageBox.Show(ex3.Message, "Error Saving DATA");
            }
        }
    }
}