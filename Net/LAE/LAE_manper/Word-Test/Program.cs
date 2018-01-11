using LAE.Comun.Cartif.Util;
using LAE.Comun.Modelo;
using LAE.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Word_Test
{
    static class Program
    {
        static void Main()
        {
            DynamicMapper<Tecnico> mapper = DynamicMapper<Tecnico>.Mapper;

            Tecnico tec = new Tecnico() { Dni = "asdagsd" };
            dynamic din = mapper.ToDynamic(tec);

            Console.WriteLine(mapper.FromDynamic(din).Dni);
        }
    }
}
