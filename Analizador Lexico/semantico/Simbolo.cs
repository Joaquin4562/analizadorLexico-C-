using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizador_Lexico.semantico
{
    public class Simbolo
    {
        public string Tipo;
        public string Id;
        public string Operador;
        public string Dato;

        public Simbolo(string tipo, string identificador)
        {
            this.Tipo = tipo;
            this.Id = identificador;
        }

        public Simbolo(string tipo, string identificador, string operador, string dato)
        {
            this.Tipo = tipo;
            this.Id = identificador;
            this.Operador = operador;
            this.Dato = dato;
        }

        public override string ToString()
        {
            return "Simbolo{'" + Tipo + "', '" + Id + "', '" + Dato + "'}";
        }
    }
}
