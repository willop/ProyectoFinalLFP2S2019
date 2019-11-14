using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFinalLFP
{
    class Traduccion
    {
        public Traduccion(List<Token> Listatokens, RichTextBox cuadrotexto) {
            string cadenaparafor = "";
            bool banderaprimercase = true;
            string contenidoswitch = "";
            int canttabulaciones = 0;
            List<string> Listatabulaciones = new List<string>();
            //llenado de tabulaciones 

            Listatabulaciones.Add("");
            Listatabulaciones.Add("");
            Listatabulaciones.Add("");
            Listatabulaciones.Add("\t");
            Listatabulaciones.Add("\t\t");
            Listatabulaciones.Add("\t\t\t");
            Listatabulaciones.Add("\t\t\t\t");
            Listatabulaciones.Add("\t\t\t\t\t");




            for (int i = 0; i < Listatokens.Count; i++)
            {

                if (Listatokens[i].id_token.Equals("TK_{"))
                {
                    canttabulaciones++;
                }
                if (Listatokens[i].id_token.Equals("TK_}"))
                {
                    canttabulaciones--;
                }
                if (Listatokens[i].id_token.Equals("TK_[")||Listatokens[i].id_token.Equals("TK_]"))
                {
                    //nada
                }

                //-----------------------------------traduccion para comentarios
                if (Listatokens[i].id_token.Equals("TK_comentario"))
                {
                    cuadrotexto.Text += Listatokens[i].lexema.Replace("//", "#") + "\n";
                }
                //-------------------------------------------para comentario multilinea
                if (Listatokens[i].id_token.Equals("TK_comentario_multilinea"))
                {
                    string multilinea = Listatokens[i].lexema.Replace("/*", "'''");
                    multilinea = multilinea.Replace("*/", "'''");
                    cuadrotexto.Text += multilinea + "\n";

                }
                //------------------------------------------------para el punto y coma se convierte en un salto de linea

                //if (Listatokens[i].id_token.Equals("TK_pcoma"))
                //{
                //    cuadrotexto.Text += "\n";
                //}

                //----------------------------------------------- para los int o id ------------------------
                if (Listatokens[i].id_token.Equals("TK_id") && Listatokens[i + 1].id_token.Equals("TK_igual"))
                {
                    
                    string variablestraduccion = Listatabulaciones[canttabulaciones] +Listatokens[i].lexema + " = ";
                    i++;//para igual
                    i++;//comienza la asignacion
                    if (Listatokens[i].id_token.Equals("TK_new"))
                    {
                        cuadrotexto.Text += variablestraduccion+ "[]\n";
                        variablestraduccion = "";
                        continue;

                    }
                    //--------------------------------------------------------------------para traduccion de arreglos
                    if (Listatokens[i].id_token.Equals("TK_{"))
                    {
                        canttabulaciones++;
                        string aux = cuadrotexto.Text;
                        cuadrotexto.Text = "from numpy import *\n" + aux;
                        variablestraduccion += "[";
                        i++;
                        for (int j = i; j < Listatokens.Count; j++)
                        {
                            if (Listatokens[j].id_token.Equals("TK_pcoma"))
                            {
                                cuadrotexto.Text += variablestraduccion + "\n";
                                break;

                            }
                            if (Listatokens[j].id_token.Equals("TK_}"))
                            {
                                canttabulaciones--;
                                variablestraduccion += "]";
                            }
                            else
                            {
                                variablestraduccion += Listatokens[j].lexema;
                            }
                            i = j;
                        }

                    }
                    else
                    //-----------------------------------------------------------------------para variables normales
                    {
                        for (int j = i; j < Listatokens.Count; j++)
                        {
                            if (Listatokens[j].id_token.Equals("TK_pcoma") || Listatokens[j].id_token.Equals("TK_coma"))
                            {
                                cuadrotexto.Text += variablestraduccion + "\n";
                                break;
                            }
                            else
                            {
                                variablestraduccion += Listatokens[j].lexema;
                            }
                            i = j;
                        }
                    }
                }

                //---------------------------------------------------------para imprimir algo en consola -------------------------------------
                if (Listatokens[i].id_token.Equals("TK_Console"))
                {
                    string mensajeaimprimir = "";
                    i++;
                    cuadrotexto.Text += Listatabulaciones[canttabulaciones]+ "print";
                    if (Listatokens[i].id_token.Equals("TK_punto"))
                    {
                        i++;
                        if (Listatokens[i].id_token.Equals("TK_WriteLine"))
                        {
                            i++;
                            if (Listatokens[i].id_token.Equals("TK_("))
                            {
                                i++;
                                cuadrotexto.Text += "(";
                                for (int j = i; j < Listatokens.Count; j++)
                                {
                                    if (Listatokens[j].id_token.Equals("TK_pcoma"))
                                    {
                                        cuadrotexto.Text += mensajeaimprimir + "\n";
                                        break;
                                    }
                                    else
                                    {
                                        mensajeaimprimir += Listatokens[j].lexema;
                                    }
                                    i = j;
                                }


                            }
                        }
                    }
                }

                //-------------------------------------------------------traduccion para el if ----------------------------------------------
                if (Listatokens[i].id_token.Equals("TK_if"))
                {
                    cuadrotexto.Text += Listatabulaciones[canttabulaciones]+ "if ";
                    string condicion = "";

                    i++;
                    if (Listatokens[i].id_token.Equals("TK_("))
                    {
                        i++;
                        for (int j = i; j < Listatokens.Count; j++)
                        {
                            if (Listatokens[j].id_token.Equals("TK_)"))
                            {
                                cuadrotexto.Text += condicion + ":\n";
                                break;
                            }
                            else
                            {
                                condicion += Listatokens[j].lexema;
                            }
                            i = j;
                        }
                    }
                }
                if (Listatokens[i].id_token.Equals("TK_else"))
                {
                    cuadrotexto.Text += Listatabulaciones[canttabulaciones]+Listatokens[i].lexema + "\n";
                }

                //---------------------------------------------------  traduccion para switch - case ------------------------------

                if (Listatokens[i].id_token.Equals("TK_break"))
                {
                    canttabulaciones--;
                }
                if (Listatokens[i].id_token.Equals("TK_switch"))
                {
                    cuadrotexto.Text += "\n";
                    i++;
                    i++;
                    contenidoswitch = Listatokens[i].lexema;
                }
                if (Listatokens[i].id_token.Equals("TK_case"))
                {
                    canttabulaciones++;
                    i++;
                    if (banderaprimercase == true)
                    {
                        cuadrotexto.Text += Listatabulaciones[canttabulaciones]+"if " + contenidoswitch + " == " + Listatokens[i].lexema + ":\n";
                        banderaprimercase = false;
                    }
                    else
                    {
                        cuadrotexto.Text += Listatabulaciones[canttabulaciones] + "elif " + contenidoswitch + " == " + Listatokens[i].lexema + ":\n\t";
                    }
                }
                if (Listatokens[i].id_token.Equals("TK_default"))
                {
                    cuadrotexto.Text += Listatabulaciones[canttabulaciones] + "else:\n";
                }

                //---------------------------------------------------  traduccion para el for --------------------------
                if (Listatokens[i].id_token.Equals("TK_for"))
                {
                    cadenaparafor += Listatabulaciones[canttabulaciones] + Listatokens[i].lexema;
                    i++;//(
                    i++;//int
                    i++;
                    cadenaparafor += " " + Listatokens[i].lexema;
                    i++;//igual
                    i++;//valor
                    cadenaparafor += " in range(" + Listatokens[i].lexema;
                    i += 4;//segundo valor o limite
                    cadenaparafor += "," + Listatokens[i].lexema;
                    i += 3;
                    if (Listatokens[i].id_token.Equals("TK_incremento"))
                    {
                        cadenaparafor += ")\n";
                        cuadrotexto.Text += cadenaparafor;
                        cadenaparafor = "";
                    }
                    else
                    {
                        cadenaparafor += ",-1)\n";
                        cuadrotexto.Text += cadenaparafor;
                        cadenaparafor = "";
                    }
                }


                //------------------------------------------------------ traduccion para el while -----------------------------------------

                if (Listatokens[i].id_token.Equals("TK_while"))
                {
                    string condicionwhile = "";
                    cuadrotexto.Text += Listatabulaciones[canttabulaciones] + Listatokens[i].lexema+" ";
                    i++;//parentesis 
                    i++;
                    for (int j = i; j < Listatokens.Count; j++)
                    {
                        if (Listatokens[j].id_token.Equals("TK_)"))
                        {
                            cuadrotexto.Text += condicionwhile + ":\n";
                            break;
                        }
                        else
                        {
                            condicionwhile += Listatokens[j].lexema;
                        }
                        i = j;
                    }
                }





            }
        }
    }
}
