using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalLFP
{
    class HTML_tablasimbolos
    {
        public HTML_tablasimbolos(List<TabladeSimbolos> tsimbolos) 
        {
            try
            {
                string fecha = DateTime.Now.ToString("dd/MM/yyyy");
                string hora = DateTime.Now.ToString("hh:mm:ss");
                string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                StreamWriter sr = new StreamWriter(@path + "\\Tabla_simbolos.html");
                int contador = 0;
                sr.WriteLine("<html> <head><title> Lista de errores </title></head><body><br><p><h2> Hora y fecha de creacion: " + fecha + " , " + hora + " <br>Ruta de este archivo: " + path + "\\Tabla_simbolos.html <br><br></p></h2><br><br><br><center><table border = 5 WIDTH = \"65%\" ><tbody>    <tr bgcolor = \"red\">  <th colspan = 4><h1>**Tabla de simbolos**</h1></th>    </tr>    <tr>    <th width=\"10%\"> No.</th>    <th>Tipo varible</th>  <th> Nombre de la variable </th> <th> Valor de la variable </th>  </tr><br><br><br><br><br><br><br><br>");
                foreach (TabladeSimbolos lista in tsimbolos)
                {                    
                    sr.WriteLine("<tr>    <th>" + contador + "</th>    <th>" + lista.Tipo_variable + "</th>  <th>" + lista.Nombre_variable + " </th>  <th>" + lista.Valor_variable + "</th>  </tr>");
                }
                sr.WriteLine("</tbody></table></center ></body></html>");
                sr.Close();
                Process.Start(path + @"\Tabla_simbolos.html");
                //MessageBox.Show(tsimbolos[i].Tipo_variable + "\n" + tsimbolos[i].Nombre_variable + "\n" + tsimbolos[i].Valor_variable);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
