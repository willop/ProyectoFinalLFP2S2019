using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalLFP
{
    class Token
    {
        public string id_token;
        public string lexema;
        public int fila;
        public int columna;
        public string descripcion;

        public Token(string id_token, string lexema, string descripcion, int fila, int columna) {
            this.id_token = id_token;
            this.lexema = lexema;
            this.fila = fila;
            this.columna = columna;
            this.descripcion = descripcion;
            
        }
    }
}
