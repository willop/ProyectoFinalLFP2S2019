using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalLFP
{
    class HTML_error
    {
        public HTML_error(List<Error_Tokens> Listaerrores,String nombrearchivo_cs,String ruta) 
        {

            try
            {
                string fecha = DateTime.Now.ToString("dd/MM/yyyy");
                string hora = DateTime.Now.ToString("hh:mm:ss");
                string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                StreamWriter sr = new StreamWriter(@path + "\\Errores_Proyecto2.html");
                int contador = 0;
                sr.WriteLine("<html> <head><title> Lista de errores </title></head><body><br><p><h2> Hora y fecha de creacion: "+fecha+" , "+hora+" <br>Ruta de este archivo: "+ path + "\\Errores_Proyecto2.html" + " <br>Nombre del archivo .cs:"+nombrearchivo_cs+".cs <br>Nombre del arvhivo .py: "+nombrearchivo_cs+".py <br></p></h2><br><br><br><center><table border = 5 WIDTH = \"65%\" ><tbody>    <tr bgcolor = \"red\">  <th colspan = 5><h1>**Lista de Errores**</h1></th>    </tr>    <tr>    <th width=\"10%\"> No.</th>    <th>Error </th>  <th> Descripcion </th>  <th width=\"10%\"> fila </th>  <th width=\"10%\"> Columna </th>  </tr><br><br><br><br><br><br><br><br>");
                foreach (Error_Tokens lista in Listaerrores)
                {
                    
                    sr.WriteLine("<tr>    <th>" + contador + "</th>    <th>" + lista.caracter + "</th>  <th>" + lista.descripcion + " </th>  <th>" + lista.fila + "</th>  <th>" + lista.columna + "</th>   </tr>");
                    contador++;
                }               
                sr.WriteLine("</tbody></table></center ></body></html>");
                sr.Close();
                Process.Start(path + @"\Errores_Proyecto2.html");
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
