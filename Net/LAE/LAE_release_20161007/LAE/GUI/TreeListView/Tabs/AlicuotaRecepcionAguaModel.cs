using GUI.TreeListView.Tree;
using LAE.Modelo;
using Microsoft.Win32;
using Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.TreeListView.Tabs
{
    class AlicuotaRecepcionAguaModel : ITreeModel
    {

        public AlicuotaItem Root { get; private set; }

        public AlicuotaRecepcionAguaModel()
        {
            Root = new AlicuotaItem();
        }

        public static AlicuotaRecepcionAguaModel CreateAlicuotaModel(List<AlicuotaRecepcionAgua> alicuotas, List<LineaAliRecepcionAgua> parametros)
        {
            AlicuotaRecepcionAguaModel model = new AlicuotaRecepcionAguaModel();

            foreach (AlicuotaRecepcionAgua item in alicuotas)
            {
                AlicuotaItem ali = new AlicuotaItem(item);
                model.Root.Items.Add(ali);

                for (int i = parametros.Count - 1; i >= 0; i--)
                {
                    if (parametros[i].IdAlicuota == item.Id && parametros[i].IdAlicuota != 0)
                    {
                        ParametroItem param = new ParametroItem(PersistenceManager.SelectByID<Parametro>(parametros[i].IdParametro));
                        ali.Items.Add(param);
                        parametros.RemoveAt(i);
                    }
                }
            }

            foreach (LineaAliRecepcionAgua item in parametros)
            {
                ParametroItem param = new ParametroItem(PersistenceManager.SelectByID<Parametro>(item.IdParametro));
                model.Root.Items.Add(param);
            }
            
            return model;
        }


        public IEnumerable GetChildren(object parent)
        {
            if (parent == null)
                parent = Root;
            return (parent as Item).Items;
        }

        public bool HasChildren(object parent)
        {
            return (parent as Item).Items?.Count > 0;
        }
    }

    public interface Item
    {
        List<Item> Items { get; }
        int Id { get; }
        String Nombre { get; }
        String Conservacion { get; }
    }

    public class AlicuotaItem : Item
    {
        private AlicuotaRecepcionAgua alic;
        public AlicuotaRecepcionAgua Alicuota { get { return alic; } }

        private List<Item> items;
        public List<Item> Items { get { return items; } }

        public int Id { get { return Alicuota.Id; } }
        public String Nombre { get { return alic.NumeroAlicuotas + " botella/s de " + (alic.RecipienteVidrio ? "vidrio" : "PE") + " con " + alic.Cantidad + PersistenceManager.SelectByID<Unidad>(alic.IdUdsCantidad ?? 0)?.Abreviatura; } }
        public String Conservacion { get; }

        public AlicuotaItem()
        {
            items = new List<Item>();
        }

        public AlicuotaItem(AlicuotaRecepcionAgua ali)
        {
            alic = ali;
            items = new List<Item>();
        }

        public override bool Equals(object obj)
        {
            AlicuotaItem compare = obj as AlicuotaItem;
            if (compare != null)
                return Id == compare.Id;

            return false;
        }
    }

    public class ParametroItem : Item
    {
        private Parametro param;
        public Parametro Parametro { get { return param; } }

        public List<Item> Items { get { return null; } }

        public int Id { get { return param.Id; } }
        public String Nombre { get { return param.NombreParametro; } }
        public String Conservacion { get { return param.Conservacion; } }

        public ParametroItem(Parametro para)
        {
            this.param = para;
        }

        public override bool Equals(object obj)
        {
            ParametroItem compare = obj as ParametroItem;
            if (compare != null)
                return Id == compare.Id;

            return false;
        }
    }
}
