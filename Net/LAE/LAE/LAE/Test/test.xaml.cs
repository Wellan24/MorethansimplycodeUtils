
using Cartif.XamlClasses;
using LAE.Modelo;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media;
using Cartif.Extensions;
using Cartif.Util;
using GenericForms;
using GenericForms.Abstract;
using GenericForms.Settings;
using GenericForms.Implemented;
using MahApps.Metro.Controls;

namespace LAE.Test
{
    /// <summary>
    /// Lógica de interacción para test.xaml
    /// </summary>
    public partial class test : MetroWindow
    {
        Cliente c;
        Contacto con;

        public test()
        {
            InitializeComponent();

            Peticion p = PersistenceManager<Peticion>.SelectByID(1);
            //date1.Value = p.Fecha;
            //date2.Value = p.Fecha;

            Contacto pruebaCliente = PersistenceManager<Contacto>.SelectByID(2, "Id", "Nombre");
            pruebaCliente.Load("Email");

            Expectation<Contacto> expect = Expectation<Contacto>.Should().BeEqualTo(pruebaCliente);

            Boolean p1 = expect.Check(pruebaCliente);
            Boolean p2 = expect.Check(new Contacto { });
            Boolean p3 = expect.FollowingShouldNot().BeTheSameAs(pruebaCliente).Check(pruebaCliente);
            Boolean p4 = expect.AddTest(c => "".Equals(c?.Apellidos)).BeNull().Check(pruebaCliente);

            //expect.BeEqualTo();

            //expect.IsWhatExpected();

            con = PersistenceManager<Contacto>.SelectByID(2);

            Cliente[] clientes = PersistenceManager<Cliente>.SelectAll().ToArray();

            Contacto[] contactos = PersistenceManager<Contacto>.SelectAll().ToArray();


            IPropertyControlSettings setting3 = new PropertyControlSettings()
            {
                Type = typeof(PropertyControlComboBox),
                InnerValues = clientes,
                Label = "El cliente",
                ControlToolTipText = "Combo elige",
                PathValue = "Id"
            };



            /* 1º - clientes */

            //myTypePanel.Build(new Dictionary<string, IPropertyControlSettings>
            //{
            //    ["Nombre"] = PropertyControlSettingsEnum.TextBoxDefault
            //        .SetOnValid((p) => p.ShowMessage("Correcto", Brushes.Green))
            //        .SetOnInvalid((p) => p.ShowMessage("Incorrecto", Brushes.Red))
            //        .SetValidate((o) => o.Equals("Juan"))
            //        .SetLabel("Otro nombre"),
            //    ["Direccion"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty
            //}, clientes.FirstOrDefault());


            /* 2º - contactos */

            con.IdCliente = 300;
            myTypePanel.Build(con, new TypePanelSettings<Contacto>()
            {
                DefaultSettings = PropertyControlSettingsEnum.TextBoxDefault,
                Fields = new FieldSettings
                {
                    ["Id"] = PropertyControlSettingsEnum.TextBoxDefault,
                    ["Nombre"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty,
                    ["IdCliente"] = new PropertyControlSettings()
                    {
                        Type = typeof(PropertyControlComboBox),
                        InnerValues = clientes,
                        Label = "El cliente",
                        ControlToolTipText = "Combo elige",
                        PathValue = "Id"
                    }.SetValidate(ValidateEnum.noEmpty)
                },
                PanelValidation = Expectation<Contacto>.Should().AddTest(c => "Antonio".Equals(c.Nombre))
            });

            //myTypePanel.Build(con);

            /* 1º - clientes */

            //IDataGridSettings sett = new DataGridSettings()
            //{
            //    Label = "Nombress",
            //    Width = 0.3
            //};

            //myTypeGrid.Build(new Dictionary<string, IDataGridSettings>
            //{
            //    ["Nombre"] = sett,
            //    ["Direccion"] = new DataGridSettings()
            //        .SetLabel("Mi apellido")
            //        .SetWidth(0.7)
            //}, clientes);

            /* 2º - contactos */


            myTypeGrid.Build(contactos, new TypeGridSettings()
            {
                Columns = new ColumnGridSettings
                {
                    ["Id"] = new TypeGridColumnSettings()
                    {
                        Width = 2
                    },
                    ["Nombre"] = new TypeGridColumnSettings()
                    {
                        Label = "Adios",
                        Width = 1
                    }
                },
                CanResizeColumns = false,
                CanReorderColumns = false
            });

            myTypeGrid.Build(contactos);
            DataGrid d = new DataGrid();


            /*ejemplo completo*/

            //Tecnico[] tecnicos = PersistenceManager<Tecnico>.SelectAll().ToArray();
            //Oferta[] ofertas = PersistenceManager<Oferta>.SelectAll().ToArray();
            //myTypePanel.Build<Revisiones_oferta>(r,
            //    new TypePanelSettings<Revisiones_oferta>
            //    {
            //        ColumnWidths = new int[] { 2, 3, 4 },
            //        DefaultSettings = new PropertyControlSettings
            //        {
            //            Validate = ValidateEnum.noEmpty,
            //            OnInvalid = ((p) => p.ShowMessage("Error validación", Brushes.Red)),
            //            OnValid = ((p) => p.ShowMessage("", Brushes.Black))
            //        },
            //        Fields = new FieldSettings
            //        {
            //            ["Id"] = PropertyControlSettingsEnum.TextBoxDefault,
            //            ["Num"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty,
            //            ["Condiciones"] = PropertyControlSettingsEnum.TextBoxDefault
            //                                .SetLabel("Mis Condiciones"),
            //            ["Objeto"] = new PropertyControlSettings
            //            {
            //                Validate = ValidateEnum.noEmpty,
            //                Type = typeof(PropertyControlTextBox)
            //            },

            //            ["IdTecnico"] = new PropertyControlSettings
            //            {
            //                Label = "Técnico",
            //                InnerValues = tecnicos,
            //                Type = typeof(PropertyControlComboBox)

            //            },
            //            ["IdOferta"] = PropertyControlSettingsEnum.ComboBoxDefault
            //                                .SetInnerValues(ofertas)
            //        },
            //        PanelValidation = Expectation<Revisiones_oferta>.Should().AddTest(c => "Revisión".Equals(c.Objeto))
            //    });


            
        }

        private void button_Click(Object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.myTypePanel.GetValidatedInnerValue<Contacto>()?.ToString());
            //MessageBox.Show(this.myTypePanel.Expectation.Check());

        }
    }
}
