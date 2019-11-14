using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalLFP
{
    class TabladeSimbolos 
    {
        public String Nombre_variable;
        public String Tipo_variable;
        public String Valor_variable;

        public TabladeSimbolos(String Nombre_variable, String Tipo_variable, String Valor_variable)
        {
            this.Nombre_variable = Nombre_variable;
            this.Tipo_variable = Tipo_variable;
            this.Valor_variable = Valor_variable;                
        }

        public void tabla(List<Token> Listatokens_tabla) { 
        
        
        }
    }
}
