using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace ReceiverWinApp
{
    public partial class Main : Form
    {

        HttpClient httpClient = new HttpClient();
        public Main()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            HttpResponseMessage response = await httpClient.GetAsync("http://localhost:56375/tests/ATests");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            textBox1.Text = responseBody;
        }
    }
}
