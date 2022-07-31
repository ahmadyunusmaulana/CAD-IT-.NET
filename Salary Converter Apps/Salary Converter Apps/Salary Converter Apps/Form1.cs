using System;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace Salary_Converter_Apps
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog file_repo = new OpenFileDialog();
            file_repo.Filter = "(*.json)|*.json";
            if (file_repo.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = file_repo.FileName;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString(textBox1.Text);
                dynamic DynamicData = JsonConvert.DeserializeObject(json);

                string file_location = textBox2.Text;
                string json_local = File.ReadAllText(file_location);

                dynamic DynamicDataLocal = JsonConvert.DeserializeObject(json_local);

                int count_number = -1;

                DateTime today = DateTime.Today;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://currencies.apps.grandtrunk.net/getrate/"+ today.ToString("yyyy-MM-dd") + "/usd/zar");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                string json_result = Convert.ToString(reader.ReadToEnd());
                float currency = float.Parse(json_result) * 1000;


                foreach (var data in DynamicData)
                {

                    var dataSalary = DynamicDataLocal.array;

                    count_number += 1;
                    string data_salary = Convert.ToString(dataSalary[count_number].salaryInIDR);
                    float salary = float.Parse(data_salary);
                    float salary_USD = salary / currency;

                    int num = dataGridView1.Rows.Add();
                    dataGridView1.Rows[num].Cells[0].Value = Convert.ToString(data.id);
                    dataGridView1.Rows[num].Cells[1].Value = Convert.ToString(data.name);
                    dataGridView1.Rows[num].Cells[2].Value = Convert.ToString(data.username);
                    dataGridView1.Rows[num].Cells[3].Value = Convert.ToString(data.email);
                    dataGridView1.Rows[num].Cells[4].Value = Convert.ToString(data.address.street + ", " + data.address.suite + ", " + data.address.city + ", " + data.address.zipcode);
                    dataGridView1.Rows[num].Cells[5].Value = Convert.ToString(data.phone);
                    dataGridView1.Rows[num].Cells[6].Value = Convert.ToString(data_salary);
                    dataGridView1.Rows[num].Cells[7].Value = Convert.ToString(salary_USD);
                }
            }
        }
    }
}
