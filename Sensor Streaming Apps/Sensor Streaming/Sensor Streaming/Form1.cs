using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using Newtonsoft.Json;
using System.IO;

namespace Sensor_Streaming
{
    public partial class Form1 : Form
    {
        public int count_id = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToString("T");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();

        }

        class Sensor_properties
        {
            public int id { get; set; }
            public string time { get; set; }
            public float temperature { get; set; }
            public float humidity { get; set; }
            public string room_area { get; set; }
        }


        private void timer2_Tick(object sender, EventArgs e)
        {
            double temperature = 0.00;
            String instanceName = "";

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
            foreach (ManagementObject obj in searcher.Get())
            {
                temperature = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                // Convert the value to celsius degrees
                temperature = (temperature - 2732.00) / 10.00;
                instanceName = obj["InstanceName"].ToString();
            }

            textBox1.Text = string.Format("{0:0.00}", temperature);
            textBox3.Text = string.Format("{0:0.00}", temperature);
            textBox5.Text = string.Format("{0:0.00}", temperature);
            textBox7.Text = string.Format("{0:0.00}", temperature);
            textBox9.Text = string.Format("{0:0.00}", temperature);

            float humidity1 = pHumd1.NextValue();
            float humidity2 = pHumd2.NextValue();
            float humidity3 = pHumd3.NextValue();
            float humidity4 = pHumd4.NextValue();
            float humidity5 = pHumd5.NextValue();

            //Set value to cpu and ram

            //metroProgressBarCPU.Value = (int)fcpu;
            //metroProgressBarRAM.Value = (int)fram;
            //Update value to cpu and ram label
            textBox2.Text = string.Format("{0:0.00}", humidity1);
            textBox4.Text = string.Format("{0:0.00}", humidity2);
            textBox6.Text = string.Format("{0:0.00}", humidity3);
            textBox8.Text = string.Format("{0:0.00}", humidity4);
            textBox10.Text = string.Format("{0:0.00}", humidity5);

            timer2.Interval = 120 * 1000; // 5000ms -> 5s

            string folder_location = textBox11.Text;
            if (checkBox4.Checked)
            {
                if(folder_location == "")
                {
                    MessageBox.Show("Please select folder location");
                    checkBox4.Checked = false;
                }
                else
                {
                    float data_humidity = 0;
                    float data_temperature = 0;
                    string data_room = "";

                    for (int count_key = 1; count_key <= 5; count_key++)
                    {
                        count_id += 1;

                        if (count_key == 1)
                        {
                            data_temperature = float.Parse(textBox1.Text);
                            data_humidity = float.Parse(textBox2.Text);
                            data_room = "Room 1";
                        }
                        if (count_key == 2)
                        {
                            data_temperature = float.Parse(textBox3.Text);
                            data_humidity = float.Parse(textBox4.Text);
                            data_room = "Room 2";
                        }
                        if (count_key == 3)
                        {
                            data_temperature = float.Parse(textBox5.Text);
                            data_humidity = float.Parse(textBox6.Text);
                            data_room = "Room 3";
                        }
                        if (count_key == 4)
                        {
                            data_temperature = float.Parse(textBox7.Text);
                            data_humidity = float.Parse(textBox8.Text);
                            data_room = "Room 4";
                        }
                        if (count_key == 5)
                        {
                            data_temperature = float.Parse(textBox9.Text);
                            data_humidity = float.Parse(textBox10.Text);
                            data_room = "Room 5";
                        }

                        Sensor_properties array_sensor = new Sensor_properties()
                        {
                            id = count_id,
                            time = label2.Text,
                            temperature = data_temperature,
                            humidity = data_humidity,
                            room_area = data_room
                        };
                        string Filepath = textBox11.Text + "/file.json";
                        var jsonData = JsonConvert.SerializeObject(array_sensor, Formatting.Indented);
                        System.IO.File.AppendAllText(Filepath, jsonData + "," + "\n");

                    }
                    

                }
                    
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            string folder_name = folderBrowserDialog1.SelectedPath;
            textBox11.Text = folder_name;
        }
    }
}
