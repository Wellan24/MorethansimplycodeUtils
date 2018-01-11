using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cartif.Extensions;
using Cartif.IO;
using Cartif.Forms;
using System.Diagnostics;
using Cartif.Logs;
using Cartif.Expectation;
using Cartif.Util;
using Cartif.Extensions;

namespace Cartif
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A test. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public partial class Test : Form
    {
        Prueba p = new Prueba("a", "b");
        AbstractExpectation<Prueba> com;

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public Test()
        {
            InitializeComponent();
            com = Expectation<Prueba>.ShouldBe(2)
                .In(null, null, p, null)
                .NotBe().In(null, p, null);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by button1 for click events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///-------------------------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            //AbstractExpectation<Prueba> inm/*= Expectation<Prueba>.InmutableShould().BeEqual(p).BeSameAs(p).BeIn(null, null, p, null)*/;

            //Console.WriteLine(Expectation<Prueba>.FastShould(p).Apply(o => o.Equals(p)).Evaluate());            

            //inm = com.Convert().ToInmutable();
            CartifStopwatch.RestartStopwatch("A");

            for (int i = 0; i < 100000; i++)
            {
                com = Expectation<Prueba>.ShouldBe(2)
                    .In(null, null, p, null)
                    .NotBe().In(null, p, null);
                com.Evaluate(p);
            }

            CartifStopwatch.PrintStopwatchElapsedTime("A", true);
            CartifStopwatch.RestartStopwatch("A");

            for (int i = 0; i < 100000; i++)
            {
                Expectation<Prueba>.FastShouldBe(p).In(null, null, p, null).NotBe().In(null, p, null).Evaluate();
            }
            
            var a = new int[] { 1, 2, 3 };

            CartifStopwatch.PrintStopwatchElapsedTime("A", true);
            //Console.WriteLine(inm.Evaluate(p));

            //com.ShouldNot().BeIn(null, null, p, null);
            //inm.ShouldNot().BeEqual(p);

            //Console.WriteLine(com.Evaluate(p));
            //Console.WriteLine(inm.Evaluate(p));
        }
    }

    ///------------------------------------------------------------------------------------------------------
    /// <summary> A prueba. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class Prueba
    {
        public String Nombre { get; set; }
        public String Apellido { get; set; }

        public Prueba(String n, String a)
        {
            Nombre = n;
            Apellido = a;
        }

        public override Boolean Equals(Object obj)
        {
            Prueba o = obj as Prueba;

            if (o != null)
                return this.Nombre.Equals(o.Nombre) && this.Apellido.Equals(o.Apellido);

            return false;
        }
    }
}
