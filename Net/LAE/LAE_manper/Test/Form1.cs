using Scripts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = GenerarDictionaryModelo
                .GenerateDictionaryMejorado("medicion","");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = GenerarDictionaryModelo.GenerateDictionaryMejorado(textBox2.Text, textBox3.Text.Trim());
        }
    }
}
