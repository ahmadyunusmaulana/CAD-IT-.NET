using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace Sensor_Agregation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file_repo = new OpenFileDialog();
            file_repo.Filter = "(*.json)|*.json";
            if (file_repo.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = file_repo.FileName;
            }
        }

        public static double findMedian(int[] a, int n)
        {
            Array.Sort(a);
            if (n % 2 != 0) 
            {
                return (double)a[n / 2];
            }
            return (double)(a[(n - 1) / 2] + a[n / 2]) / 2.0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string file_location = textBox1.Text;
            string json_local = File.ReadAllText(file_location);
            dynamic DynamicDataLocal = JsonConvert.DeserializeObject(json_local);

            //string message = Convert.ToString(DynamicDataLocal.array);
            //string title = "Title";
            //MessageBox.Show(message, title);

            float[] temp_array = {0, 0, 0};
            float[] humd_array = { 0, 0, 0 };

            foreach (var data in DynamicDataLocal.array) 
            {
                string data_timestamp_str = Convert.ToString(data.timestamp);
                long data_timestamp = long.Parse(data_timestamp_str);
                var datetime_convert = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(data_timestamp / 1000d)).ToLocalTime();
                int counter_key = Convert.ToInt32(data.id);
                int modulo_count = counter_key % 3;
                
                int num = dataGridView1.Rows.Add();
                dataGridView1.Rows[num].Cells[0].Value = Convert.ToString(data.id);
                dataGridView1.Rows[num].Cells[1].Value = Convert.ToString(data.temperature);
                dataGridView1.Rows[num].Cells[2].Value = Convert.ToString(data.humidity);
                dataGridView1.Rows[num].Cells[3].Value = Convert.ToString(data.roomArea);
                dataGridView1.Rows[num].Cells[4].Value = Convert.ToString(data.timestamp);
                dataGridView1.Rows[num].Cells[5].Value = Convert.ToString(datetime_convert);

                string data_temp_convert = Convert.ToString(data.temperature);
                string data_humd_convert = Convert.ToString(data.humidity);

                if (modulo_count == 0)
                {

                    temp_array[2] = float.Parse(data_temp_convert);
                    humd_array[2] = float.Parse(data_humd_convert);

                    string numberOfMinTemp = Convert.ToString(temp_array.Min());
                    string numberOfMaxTemp = Convert.ToString(temp_array.Max());

                    string numberOfMinHumd = Convert.ToString(humd_array.Min());
                    string numberOfMaxHumd = Convert.ToString(humd_array.Max());


                    int[] convertTempArrayValue = temp_array.Select(x => Convert.ToInt32(x)).ToArray();
                    int[] convertHumdArrayValue = humd_array.Select(x => Convert.ToInt32(x)).ToArray();

                    int temp_array_length = convertTempArrayValue.Length;
                    int humd_array_length = convertHumdArrayValue.Length;

                    double average_temp = Queryable.Average(convertTempArrayValue.AsQueryable());
                    double average_humd = Queryable.Average(convertHumdArrayValue.AsQueryable());


                    dataGridView1.Rows[num].Cells[6].Value = "Temperature: " + numberOfMinTemp + ", " + "Humidity: " + numberOfMinHumd;
                    dataGridView1.Rows[num].Cells[7].Value = "Temperature: " + numberOfMaxTemp + ", " + "Humidity: " + numberOfMaxHumd;
                    dataGridView1.Rows[num].Cells[8].Value = "Temperature: " + findMedian(convertTempArrayValue, temp_array_length) + "Humidity: " + findMedian(convertHumdArrayValue, humd_array_length);
                    dataGridView1.Rows[num].Cells[9].Value = "Temperature: " + average_temp + ", " + "Humidity: " + average_humd;
                }
                else if (modulo_count == 1) 
                {
                    temp_array[0] = float.Parse(data_temp_convert);
                    humd_array[0] = float.Parse(data_humd_convert);
                }

                else if (modulo_count == 2)
                {
                    temp_array[1] = float.Parse(data_temp_convert);
                    humd_array[1] = float.Parse(data_humd_convert);
                }


            }
        }
    }
}
