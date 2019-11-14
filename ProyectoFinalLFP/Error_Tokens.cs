using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalLFP
{
    class Error_Tokens
    {
        public int fila;
        public int columna;
        public string caracter;
        public string descripcion;

        public Error_Tokens(string caracter, string descripcion, int columna, int fila) {
            this.fila = fila;
            this.columna = columna;
            this.caracter = caracter;
            this.descripcion = descripcion;
        }
    }
}
