using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalLFP
{
    class HTML_tokens
    {
        public HTML_tokens(List<Token> Listatokens, String nombrearchivo_cs, String ruta)
        {
            try
            {
                string fecha = DateTime.Now.ToString("dd/MM/yyyy");
                string hora = DateTime.Now.ToString("hh:mm:ss");
                string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                StreamWriter creardoc = new StreamWriter(@path + "\\Tokens_final_Proyecto2.html");
                creardoc.WriteLine("<html><head><title> Archivo de tokens </title ></head ><body><br><p><h2> Hora y fecha de creacion: " + fecha + " , " + hora + " <br>Ruta de este archivo: " + path + "\\Tokens_final_Proyecto2.html" + " <br>Nombre del archivo .cs:" + nombrearchivo_cs + ".cs <br>Nombre del arvhivo .py: " + nombrearchivo_cs + ".py <br></p></h2><br><br><br><center><table border = 5 WIDTH = \"65%\" ><tbody>    <tr bgcolor = \"skyblue\">  <th colspan = 5> El encabezado de la tabla </th>    </tr>    <tr>    <th width=\"10%\"> No.</th>    <th> Lexema </th>  <th > Tipo </th>  <th> Fila </th>  <th> Columna </th>   </tr>");
                int contador = 0;
                foreach (Token item in Listatokens)
                {
                    
                    creardoc.WriteLine("<tr>    <th>" + contador + "</th>    <th>" + item.lexema + "</th>  <th>" + item.descripcion + " </th>  <th>" + item.fila + "</th>   <th>" + item.columna + "</th> </tr>");
                    contador++;
                }
                creardoc.WriteLine("</tbody></table></center ></body></html>");
                creardoc.Close();
                Process.Start(path + @"\Tokens_final_Proyecto2.html");
            }
            catch (Exception)
            {

            }
        }
    }
}
