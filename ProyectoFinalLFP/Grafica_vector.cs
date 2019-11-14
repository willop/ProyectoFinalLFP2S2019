using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalLFP
{
    class Grafica_vector
    {
        public Grafica_vector(){
        

        }
        public void graficar(String vector, String nombre, List<TabladeSimbolos> Tablade_simbolos) {
            String rutaescritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


            try
            {

                int index = Tablade_simbolos.FindIndex(X => X.Nombre_variable == vector);
                String[] valor_id = Tablade_simbolos[index].Valor_variable.Split(',');
                int tamañodevector = valor_id.Length-1;
                int r = 0;

                //                  node1 [fillcolor=yellow style="filled" label="Hola mjnd"]
                //                  node2[fillcolor = red]
                //                  node3[fillcolor = green]
                //                  node1->node2->node3
                string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                StreamWriter creardoc = new StreamWriter(@path + "\\grafo.txt");
                creardoc.WriteLine("digraph "+nombre+ " \n{\nrankdir = LR\nnode[Style = rounded, shape = box]\n in [fillcolor=red style =\"filled\" label=\"inicio\"]\n fin [fillcolor=red style =\"filled\" label=\"fin\"]\nin-> node0");
                 
                for (int i = 0; i < valor_id.Length; i++)
                {
                    creardoc.WriteLine("node"+i+" [fillcolor=blue style=\"filled\" label=\""+valor_id[i]+"\"]");
                }

                creardoc.WriteLine("\n");
                while (r<=tamañodevector) {
                    if (r != 0)
                    {
                        creardoc.Write(" -> "+"node" + r);
                        r++;
                    }
                    else
                    {
                        creardoc.Write("node" + r);
                        r++;
                    }
                }

                creardoc.WriteLine("\nnode"+tamañodevector +"-> fin\n}");
                creardoc.Close();


                //comando para crear la imagen del vector 
                String comando = "dot -Tpng " + rutaescritorio + "\\grafo.txt -o " + rutaescritorio + "\\grafico.png";


                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();

                cmd.StandardInput.WriteLine(comando);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                Process.Start(path + @"\grafico.png");
            }
            catch { 
            
            }
        }

    }
}
