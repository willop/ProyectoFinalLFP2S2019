using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFinalLFP
{
    public partial class Form1 : Form
    {
        string rutaarchivo="";
        string nombredelarchivo = "";
        int cantidaderrores = 0;
        List<Token> Lista_tokens = new List<Token>();
        List<Token> Lista_tokens2 = new List<Token>();
        List<Error_Tokens> Lista_errores = new List<Error_Tokens>();
        Analizador_sintactico parser;
        List<Error_Tokens> erroressintacticos = new List<Error_Tokens>();
        string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void AbrirToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //--------------------------------abrir el explorador----------------------------------------
            string contenido;
                OpenFileDialog cuadrodialogo = new OpenFileDialog();
                cuadrodialogo.InitialDirectory = @"C:\Users\WillOP\Desktop\";
                if (cuadrodialogo.ShowDialog() == DialogResult.OK)
                {

                    StreamReader sr = new StreamReader(cuadrodialogo.FileName, System.Text.Encoding.UTF8, false);
                    rutaarchivo = cuadrodialogo.FileName.ToString();
                nombredelarchivo = System.IO.Path.GetFileNameWithoutExtension(rutaarchivo);

                    //MessageBox.Show(rutaarchivo);
                contenido = sr.ReadToEnd();
                    richTextBox1.Text = contenido;
                    sr.Close();
                }
            
            else
            {

                //--------------------------------abrir el explorador----------------------------------------
                cuadrodialogo.InitialDirectory = @"C:\Users\WillOP\Desktop\";
                if (cuadrodialogo.ShowDialog() == DialogResult.OK)
                {
                    StreamReader sr = new StreamReader(cuadrodialogo.FileName, System.Text.Encoding.UTF8, false);
                    rutaarchivo = cuadrodialogo.FileName.ToString();
                    nombredelarchivo = cuadrodialogo.SafeFileName.ToString();
                    //MessageBox.Show(rutaarchivo);
                    contenido = sr.ReadToEnd();
                    richTextBox1.Text = contenido;
                    sr.Close();
                }
            }
        }

        private void GuardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {


                if (rutaarchivo == "")
                {
                    SaveFileDialog cuadroguardar = new SaveFileDialog();
                    cuadroguardar.InitialDirectory = @"C:\Users\WillOP\Desktop\";
                    cuadroguardar.ShowDialog();

                    if (cuadroguardar.FileName != "")
                    {
                        //MessageBox.Show("entro por que se selecciono y la ruta es: "+cuadroguardar.FileName+"\ny escribira:\n"+contenedortexto.Text);
                        StreamWriter textoguardar = new StreamWriter(cuadroguardar.FileName);
                        rutaarchivo = cuadroguardar.FileName.ToString();
                        textoguardar.Write(richTextBox1.Text);
                        textoguardar.Close();
                    }

                }
                else
                {
                    StreamWriter textoguardar = new StreamWriter(rutaarchivo);
                    textoguardar.Write(richTextBox1.Text);
                    textoguardar.Close();
                }

            }
            catch (Exception)
            {


            }
        }

        private void AcercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Wilfred Stewart Perez Solorzano\n201408419\n\nLenguajes Formales y programables\nSeccion: B-\nAuxiliar: Luis Yela\n\n\n\nContacto:\nwilfredp159@gmail.com", "");
        }

        private void SalirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GenerarTraduccionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            richTextBox3.Text = "";
            //-----------------------------------------------------------------------inicia el analizador lexico-------------------------------------------

            Lista_tokens.Clear();
            Lista_tokens2.Clear();
            Lista_errores.Clear();
            String palabra = "";
            int longitudcodigo, fila, columna;
            int estado = 0;
            fila = 0;
            columna = 0;
            String codigo;
            codigo = richTextBox1.Text;
            longitudcodigo = codigo.Length;
            int cantidndad_errores = 0;


            for (int i = 0; i < codigo.Length; i++)
            {
                switch (estado)
                {
                    case 0:
                        {
                            //------salto de linea------
                            if (codigo[i].Equals((char)10))
                            {
                                estado = 0;
                                fila++;
                                columna = 0;
                                break;
                            }
                            //-------espacion en blanco-----
                            if (codigo[i].Equals((char)32))
                            {
                                estado = 0;
                                columna++;
                                break;
                            }
                            //----------tabulacion---------
                            if (codigo[i].Equals((char)09))
                            {
                                estado = 0;
                                columna += 9;
                                break;
                            }

                            //---------- { --------------
                            if (codigo[i].Equals((char)123))
                            {
                                estado = 0;
                                columna++;
                                Lista_tokens.Add(new Token("TK_{",codigo[i].ToString(),"Llave de apertura", fila,columna));
                                Lista_tokens2.Add(new Token("TK_{", codigo[i].ToString(), "Llave de apertura", fila, columna));
                                break;
                            }
                            //---------- } --------------
                            if (codigo[i].Equals((char)125))
                            {
                                estado = 0;
                                columna++;
                                Lista_tokens.Add(new Token("TK_}", codigo[i].ToString(), "Llave de cierre", fila, columna));
                                Lista_tokens2.Add(new Token("TK_}", codigo[i].ToString(), "Llave de apertura", fila, columna));
                                break;
                            }
                            //----------[ ----------------
                            if (codigo[i].Equals((char)91))
                            {
                                estado = 0;
                                columna++;
                                Lista_tokens.Add(new Token("TK_[", codigo[i].ToString(), "Corchete de apertura", fila, columna));
                                Lista_tokens2.Add(new Token("TK_[", codigo[i].ToString(), "Corchete de apertura", fila, columna));
                                break;
                            }
                            //----------] ----------------
                            if (codigo[i].Equals((char)93))
                            {
                                estado = 0;
                                columna++;
                                Lista_tokens.Add(new Token("TK_]", codigo[i].ToString(), "Corchete de cierre", fila, columna));
                                Lista_tokens2.Add(new Token("TK_]", codigo[i].ToString(), "Corchete de cierre", fila, columna));
                                break;
                            }
                            //----------( ----------------
                            if (codigo[i].Equals((char)40))
                            {
                                estado = 0;
                                columna++;
                                Lista_tokens.Add(new Token("TK_(", codigo[i].ToString(), "Parentesis de apertura", fila, columna));
                                Lista_tokens2.Add(new Token("TK_(", codigo[i].ToString(), "Parentesis de apertura", fila, columna));
                                break;
                            }
                            //----------) ----------------
                            if (codigo[i].Equals((char)41))
                            {
                                estado = 0;
                                columna++;
                                Lista_tokens.Add(new Token("TK_)", codigo[i].ToString(), "Parentecis de cierre", fila, columna));
                                Lista_tokens2.Add(new Token("TK_)", codigo[i].ToString(), "Parentecis de cierre", fila, columna));
                                break;
                            }
                            //----------; ----------------
                            if (codigo[i].Equals((char)59))
                            {
                                estado = 0;
                                columna++;
                                Lista_tokens.Add(new Token("TK_pcoma", codigo[i].ToString(), "Punto y coma", fila, columna));
                                Lista_tokens2.Add(new Token("TK_pcoma", codigo[i].ToString(), "Punto y coma", fila, columna));
                                break;
                            }
                            //----------, ----------------
                            if (codigo[i].Equals((char)44))
                            {
                                estado = 0;
                                columna++;
                                Lista_tokens.Add(new Token("TK_coma", codigo[i].ToString(), "Coma", fila, columna));
                                Lista_tokens2.Add(new Token("TK_coma", codigo[i].ToString(), "Coma", fila, columna));
                                break;
                            }
                            //----------  . ----------------
                            if (codigo[i].Equals((char)46))
                            {
                                estado = 0;
                                columna++;
                                Lista_tokens.Add(new Token("TK_punto", codigo[i].ToString(), "Punto", fila, columna));
                                Lista_tokens2.Add(new Token("TK_punto", codigo[i].ToString(), "Punto", fila, columna));
                                break;
                            }
                            //----------* ----------------
                            if (codigo[i].Equals((char)42))
                            {
                                estado = 0;
                                columna++;
                                Lista_tokens.Add(new Token("TK_por", codigo[i].ToString(), "Signo de multiplicación", fila, columna));
                                Lista_tokens2.Add(new Token("TK_por", codigo[i].ToString(), "Punto", fila, columna));
                                break;
                            }
                            //----------: ----------------
                            if (codigo[i].Equals((char)58))
                            {
                                estado = 0;
                                columna++;
                                Lista_tokens.Add(new Token("TK_dospuntos", codigo[i].ToString(), "Dos puntos", fila, columna));
                                Lista_tokens2.Add(new Token("TK_dospuntos", codigo[i].ToString(), "Dos puntos", fila, columna));
                                break;
                            }
                            //----------si es letra ----------------
                            if (char.IsLetter(codigo[i]))
                            {
                                palabra = "";
                                i--;
                                estado = 1;
                                break;
                            }
                            //-----------si es un digito-----------
                            if (char.IsDigit(codigo[i]))
                            {
                                palabra = codigo[i].ToString();
                                estado = 2;
                                columna++;
                                break;
                            }
                            //-------------" -------------------
                            if (codigo[i].Equals((char)34))
                            {
                                palabra = codigo[i].ToString();
                                estado = 4;
                                columna++;
                                break;
                            }
                            //----------- = -------------------
                            if (codigo[i].Equals((char)61))
                            {
                                estado = 19;
                                columna++;
                                palabra = codigo[i].ToString(); ;
                                break;
                            }
                            //------------------/----------------
                            if (codigo[i].Equals((char)47))
                            {
                                estado = 6;
                                palabra = codigo[i].ToString();
                                columna++;
                                break;
                            }
                            //-------------------+ -------------
                            if (codigo[i].Equals((char)43))
                            {
                                estado = 11;
                                palabra = codigo[i].ToString();
                                columna++;
                                break;

                            }
                            //-----------------  -  ------------
                            if (codigo[i].Equals((char)45))
                            {
                                estado = 13;
                                palabra = codigo[i].ToString();
                                columna++;
                                break;
                            }
                            //----------------- > --------------
                            if (codigo[i].Equals((char)62))
                            {
                                estado = 15;
                                palabra = codigo[i].ToString();
                                columna++;
                                break;
                            }
                            //------------------ < -------------
                            if (codigo[i].Equals((char)60))
                            {
                                estado = 17;
                                palabra = codigo[i].ToString();
                                columna++;
                                break;
                            }
                            //--------------------- ! ----------
                            if (codigo[i].Equals((char)33))
                            {
                                estado = 18;
                                palabra = codigo[i].ToString();
                                columna++;
                                break;
                            }
                            //--------------- ' ---------------
                            if (codigo[i].Equals((char)39))
                            {
                                estado = 22;
                                palabra = codigo[i].ToString();
                                columna++;
                                break;
                            }
                            else
                            {
                                Lista_errores.Add(new Error_Tokens(codigo[i].ToString(), "Caracter no reconocido en el lenguaje", columna, fila));
                                //--------- ERROR -------- 
                            }
                            break;
                        }// fin del estado 0

                    case 1:
                        {
                            if (char.IsLetter(codigo[i])||char.IsDigit(codigo[i])||codigo[i].Equals((char)95))
                            {
                                palabra += codigo[i].ToString();
                                estado = 1;
                                columna++;
                                break;

                            }
                            else
                            {
                                if (palabra.Equals("class"))
                                {
                                    Lista_tokens.Add(new Token("TK_class", palabra, "Palabra reservada class", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_class", palabra, "Palabra reservada class", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("static"))
                                {
                                    Lista_tokens.Add(new Token("TK_static", palabra, "Palabra reservada static", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_static", palabra, "Palabra reservada static", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("void"))
                                {
                                    Lista_tokens.Add(new Token("TK_void", palabra, "Palabra reservada void", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_void", palabra, "Palabra reservada void", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("new"))
                                {
                                    Lista_tokens.Add(new Token("TK_new", palabra, "Palabra reservada new", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_new", palabra, "Palabra reservada new", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("graficarVector"))
                                {
                                    Lista_tokens.Add(new Token("TK_metodo_graficar_vector", palabra, "Metodo para graficar un vector", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_metodo_graficar_vector", palabra, "Metodo para graficar un vector", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("int"))
                                {
                                    Lista_tokens.Add(new Token("TK_var_int", palabra, "Palabra reservada variable int", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_var_int", palabra, "Palabra reservada variable int", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("float"))
                                {
                                    Lista_tokens.Add(new Token("TK_var_float", palabra, "Palabra reservada variable float", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_var_float", palabra, "Palabra reservada variable float", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("char"))
                                {
                                    Lista_tokens.Add(new Token("TK_var_char", palabra, "Palabra reservada variable char", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_var_char", palabra, "Palabra reservada variable char", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("String"))
                                {
                                    Lista_tokens.Add(new Token("TK_var_String", palabra, "Palabra reservada variable string", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_var_String", palabra, "Palabra reservada variable string", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("bool"))
                                {
                                    Lista_tokens.Add(new Token("TK_var_bool", palabra, "Palabra reservada variable bool", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_var_bool", palabra, "Palabra reservada variable bool", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("false"))
                                {
                                    Lista_tokens.Add(new Token("TK_false", palabra, "Palabra reservada booleana false", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_false", palabra, "Palabra reservada booleana false", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("true"))
                                {
                                    Lista_tokens.Add(new Token("TK_true", palabra, "Palabra reservada booleana true", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_true", palabra, "Palabra reservada booleana true", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("Console"))
                                {
                                    Lista_tokens.Add(new Token("TK_Console", palabra, "Palabra reservada Console", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_Console", palabra, "Palabra reservada Console", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("WriteLine"))
                                {
                                    Lista_tokens.Add(new Token("TK_WriteLine", palabra, "Palabra reservada WriteLine", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_WriteLine", palabra, "Palabra reservada WriteLine", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("args"))
                                {
                                    Lista_tokens.Add(new Token("TK_args", palabra, "Palabra reservada args", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_args", palabra, "Palabra reservada args", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("if"))
                                {
                                    Lista_tokens.Add(new Token("TK_if", palabra, "Palabra reservada sentencia if", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_if", palabra, "Palabra reservada sentencia if", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("else"))
                                {
                                    Lista_tokens.Add(new Token("TK_else", palabra, "Palabra reservada sentencia else", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_else", palabra, "Palabra reservada sentencia else", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("switch"))
                                {
                                    Lista_tokens.Add(new Token("TK_switch", palabra, "Palabra reservada switch", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_switch", palabra, "Palabra reservada switch", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("case"))
                                {
                                    Lista_tokens.Add(new Token("TK_case", palabra, "Palabra reservada case", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_case", palabra, "Palabra reservada case", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("break"))
                                {
                                    Lista_tokens.Add(new Token("TK_break", palabra, "Palabra reservada break", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_break", palabra, "Palabra reservada break", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("default"))
                                {
                                    Lista_tokens.Add(new Token("TK_default", palabra, "Palabra reservada default", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_default", palabra, "Palabra reservada default", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("for"))
                                {
                                    Lista_tokens.Add(new Token("TK_for", palabra, "Palabra reservada for", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_for", palabra, "Palabra reservada for", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("while"))
                                {
                                    Lista_tokens.Add(new Token("TK_while", palabra, "Palabra reservada while", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_while", palabra, "Palabra reservada while", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                if (palabra.Equals("Main"))
                                {
                                    Lista_tokens.Add(new Token("TK_Main", palabra, "Palabra reservada Main", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_Main", palabra, "Palabra reservada Main", fila, columna));
                                    palabra = "";
                                    estado = 0;
                                    i--;
                                    break;
                                }
                                else
                                {

                                    Lista_tokens.Add(new Token("TK_id", palabra, "Identificador", fila, columna));
                                    Lista_tokens2.Add(new Token("TK_id", palabra, "Identificador", fila, columna));
                                    palabra = "";
                                    columna++;
                                    estado = 0;
                                    i--;
                                    break;
                                 
                                }
                            }
                        }
                         
                    case 2:
                        {
                            if (char.IsDigit(codigo[i]))
                            {
                                palabra += codigo[i].ToString();
                                estado = 2;
                                columna++;
                                break;
                            }
                            if (codigo[i].Equals((char)46))
                            {
                                palabra += codigo[i].ToString();
                                estado = 3;
                                columna++;
                                break;
                            }
                            else
                            {
                                Lista_tokens.Add(new Token("TK_num_entero",palabra, "Numero entero", fila, columna));
                                Lista_tokens2.Add(new Token("TK_num_entero", palabra, "Numero entero", fila, columna));
                                i--;
                                estado = 0;
                                palabra = "";
                                break;
                            }

                        }

                    case 3:
                        {
                            if (char.IsDigit(codigo[i]))
                            {
                                palabra += codigo[i].ToString();
                                columna++;
                                estado = 20;
                                break;
                            }
                            else
                            {
                                Lista_errores.Add(new Error_Tokens(palabra, "Falta numero decimal", columna, fila));
                                i--;
                                estado = 0;
                                palabra = "";
                                break;
                            }

                        }
                    case 4:
                        {
                            // si son comillas pasa a 5 de lo contrario acepta
                            if (codigo[i].Equals((char)34))
                            {
                                columna++;
                                palabra += codigo[i].ToString();
                                Lista_tokens.Add(new Token("TK_cadena", palabra, "Cadena", fila, columna));
                                Lista_tokens2.Add(new Token("TK_cadena", palabra, "Cadena", fila, columna));
                                estado = 0;
                                palabra = "";
                                break;
                            }
                            if (codigo[i].Equals((char)10))
                            {
                                Lista_errores.Add(new Error_Tokens(palabra,"No cerro comillas", columna,fila));
                                estado = 0;
                                i--;
                                palabra = "";
                                break;
                            }
                            else
                            {
                                palabra += codigo[i].ToString();
                                estado = 4;
                                columna++;
                                break;
                            }

                        }
                    case 6:
                        {
                            if (codigo[i].Equals((char)47))
                            {
                                estado = 7;
                                columna++;
                                palabra += codigo[i].ToString();
                                break;
                            }
                            if (codigo[i].Equals((char)42))
                            {
                                estado = 8;
                                columna++;
                                palabra += codigo[i].ToString();
                                break;
                            }
                            else
                            {
                                Lista_tokens.Add(new Token("TK_div", palabra, "Operador de divición", fila, columna));
                                Lista_tokens2.Add(new Token("TK_div", palabra, "Operador de divición", fila, columna));
                                estado = 0;
                                palabra = "";
                                i--;
                                break;
                            }

                        }
                    case 7:
                        {
                            if (codigo[i].Equals((char)10))
                            {
                                Lista_tokens2.Add(new Token("TK_comentario", palabra, "Comentario", fila, columna));
                                fila++;
                                columna = 0;
                                estado = 0;
                                palabra = "";
                                break;
                            }
                            else
                            {
                                palabra += codigo[i].ToString();
                                columna++;
                                estado = 7;
                                break;
                            }

                        }

                    case 8:
                        {

                            if (codigo[i].Equals((char)42))
                            {
                                columna++;
                                estado = 9;
                                palabra += codigo[i].ToString();
                                break;
                            }
                            else
                            {
                                palabra += codigo[i].ToString();
                                estado = 8;
                                columna++;
                                break;
                            }
                        }
                    case 9:
                        {
                            // si es diagonal
                            if (codigo[i].Equals((char)47))
                            {
                                columna++;
                                palabra += codigo[i].ToString();
                                Lista_tokens2.Add(new Token("TK_comentario_multilinea", palabra, "Comentario multilinea", fila, columna));
                                columna = 0;
                                estado = 0;
                                palabra = "";
                                break;

                            }
                            if (codigo[i].Equals((char)42))
                            {
                                columna++;
                                estado = 9;
                                palabra += codigo[i].ToString();
                                break;
                            }
                            else
                            {                               
                                palabra += codigo[i].ToString();
                                estado = 8;
                                columna++;
                                break;
                            }
                        }
                    case 11:
                        {
                            if (codigo[i].Equals((char)43))
                            {
                                columna++;
                                estado = 0;
                                palabra += codigo[i].ToString();
                                Lista_tokens.Add(new Token("TK_incremento", palabra, "Incrementador ++", fila, columna));
                                Lista_tokens2.Add(new Token("TK_incremento", palabra, "Incrementador ++", fila, columna));
                                palabra = "";
                                break;
                            }
                            else
                            {
                                Lista_tokens.Add(new Token("TK_mas", palabra, "Operador de suma", fila, columna));
                                Lista_tokens2.Add(new Token("TK_mas", palabra, "Operador de suma", fila, columna));
                                i--;
                                palabra = "";
                                estado = 0;
                                break;

                            }
                        }
                    case 13:
                        {
                            if (codigo[i].Equals((char)45))
                            {
                                columna++;
                                estado = 0;
                                palabra += codigo[i].ToString();
                                Lista_tokens.Add(new Token("TK_decremento", palabra, "Decremeto --", fila, columna));
                                Lista_tokens2.Add(new Token("TK_decremento", palabra, "Decremeto --", fila, columna));
                                palabra = "";
                                break;
                            }
                            else
                            {
                                Lista_tokens.Add(new Token("TK_menos", palabra, "Operador de resta", fila, columna));
                                Lista_tokens2.Add(new Token("TK_menos", palabra, "Operador de resta", fila, columna));
                                i--; 
                                palabra = "";
                                estado = 0;
                                break;

                            }
                        }
                    case 15:
                        {
                            if (codigo[i].Equals((char)61))
                            {
                                columna++;
                                estado = 0;
                                palabra += codigo[i].ToString();
                                Lista_tokens.Add(new Token("TK_mayor_igual", palabra, "Mayor o igual ", fila, columna));
                                Lista_tokens2.Add(new Token("TK_mayor_igual", palabra, "Mayor o igual ", fila, columna));
                                palabra = "";
                                break;
                            }
                            else
                            {
                                Lista_tokens.Add(new Token("TK_mayor", palabra, "Mayor", fila, columna));
                                Lista_tokens2.Add(new Token("TK_mayor", palabra, "Mayor", fila, columna));
                                i--;
                                palabra = "";
                                estado = 0;
                                break;

                            }
                        }
                    case 17:
                        {
                            if (codigo[i].Equals((char)61))
                            {
                                columna++;
                                estado = 0;
                                palabra += codigo[i].ToString();
                                Lista_tokens.Add(new Token("TK_menor_igual", palabra, "Menor o igual ", fila, columna));
                                Lista_tokens2.Add(new Token("TK_menor_igual", palabra, "Menor o igual ", fila, columna));
                                palabra = "";
                                break;
                            }
                            else
                            {
                                Lista_tokens.Add(new Token("TK_menor", palabra, "Menor ", fila, columna));
                                Lista_tokens2.Add(new Token("TK_menor", palabra, "Menor ", fila, columna));
                                i--;
                                palabra = "";
                                estado = 0;
                                break;

                            }
                        }
                    case 18:
                        {
                            if (codigo[i].Equals((char)61))
                            {
                                columna++;
                                estado = 0;
                                palabra += codigo[i].ToString();
                                Lista_tokens.Add(new Token("TK_diferencia", palabra, "Distinto a ", fila, columna));
                                Lista_tokens2.Add(new Token("TK_diferencia", palabra, "Distinto a ", fila, columna));
                                palabra = "";
                                break;
                            }
                            else
                            {
                                Lista_errores.Add(new Error_Tokens(palabra, "Caracter no se reconoce en el lenguaje", columna, fila));
                                cantidndad_errores++;
                                MessageBox.Show(codigo[i].ToString());
                                i--;
                                palabra = "";
                                estado = 0;
                                break;

                            }
                        }
                    case 19:
                        {
                            if (codigo[i].Equals((char)61))
                            {
                                columna++;
                                estado = 0;
                                palabra += codigo[i].ToString();
                                Lista_tokens.Add(new Token("TK_igualacion", palabra, "Comparacion igual == ", fila, columna));
                                Lista_tokens2.Add(new Token("TK_igualacion", palabra, "Comparacion igual == ", fila, columna));
                                palabra = "";
                                break;
                            }
                            else
                            {
                                Lista_tokens.Add(new Token("TK_igual", palabra, "Igual ", fila, columna));
                                Lista_tokens2.Add(new Token("TK_igual", palabra, "Igual ", fila, columna));
                                i--;
                                palabra = "";
                                estado = 0;
                                break;

                            }
                        }

                    case 20:
                        {
                            if (char.IsDigit(codigo[i]))
                            {
                                palabra += codigo[i].ToString();
                                columna++;
                                estado = 20;
                                break;
                            }
                            if (codigo[i].ToString().ToLower().Equals("f"))
                            {
                                palabra += codigo[i].ToString();
                                columna++;
                                Lista_tokens.Add(new Token("TK_num_decimal_f", palabra, "Numero decimal de asignación", fila, columna));
                                Lista_tokens2.Add(new Token("TK_num_decimal_f", palabra, "Numero decimal de asignación", fila, columna));
                                estado = 0;
                                palabra = "";
                                break;
                            }
                            else
                            {
                                Lista_tokens.Add(new Token("TK_num_decimal", palabra, "Numero decimal", fila, columna));
                                Lista_tokens2.Add(new Token("TK_num_decimal", palabra, "Numero decimal", fila, columna));
                                palabra = "";
                                estado = 0;
                                i--;
                                break;
                            }
                        }

                    case 22:
                        {
                            if (codigo[i].Equals((char)10))
                            {
                                Lista_errores.Add(new Error_Tokens("\\n", "Salto de linea", columna, fila));
                                i--;
                                estado = 0;
                                palabra = "";
                            }
                            if (codigo[i].Equals((char)39))
                            {
                                columna++;
                                estado = 0;
                                palabra += codigo[i].ToString();
                                Lista_tokens.Add(new Token("TK_caracter", palabra, "Caracter", fila, columna));
                                Lista_tokens2.Add(new Token("TK_caracter", palabra, "Caracter", fila, columna));
                                palabra = "";
                                break;
                            }
                            else
                            {
                                palabra += codigo[i].ToString();
                                estado = 22;
                                columna++;
                                break;

                            }
                        }

                    case 23:
                        {
                            if (codigo[i].Equals((char)39))
                            {
                                columna++;
                                estado = 0;
                                palabra += codigo[i].ToString();
                                //*-*-*-*-*-*-*-*-*-*-*-*-**-*-**-*-*--*-**--**--*-*-*-* agregar a lista de tokens *-*-*-*--*--*-*-*-*
                                palabra = "";
                                break;
                            }
                            break;
                        }


                }//fin del switch
            }//-----------------------------------------------------------------------fin del analizador---------------------------------------------------

            Lista_tokens.Add(new Token("fin","","",0,0));
            parser = new Analizador_sintactico();
            cantidaderrores = parser.Parser(Lista_tokens);
            richTextBox3.Text = parser.ejecucion();
            erroressintacticos = parser.errorsintactico();

            //para ver la tabla de simbolos

            //para ver todas las variables dentro del lenguaje (tabla de simbolos)
            //for (int i = 0; i < tsimbolos.Count; i++)
            //{
            //    MessageBox.Show(tsimbolos[i].Tipo_variable + "\n" + tsimbolos[i].Nombre_variable + "\n" + tsimbolos[i].Valor_variable);
            //}

            //HTML_tokens generarhtmltokens2 = new HTML_tokens(Lista_tokens);
            if (Lista_errores.Count > 0 || cantidaderrores > 0)
            {
                if (Lista_errores.Count > 0)
                {
                    MessageBox.Show("Se encontraron errores lexicos en el archivo\n\nRevise la tabla de errores creada en el escritorio");
                    richTextBox2.Text = "ERROR";
                    richTextBox3.Text = "ERROR";
                    //HTML_error generarhtmlerrores = new HTML_error(Lista_errores,nombredelarchivo,rutaarchivo);
                }
                if (cantidaderrores > 0)
                {
                    MessageBox.Show("Se encontraron errores sintacticos en el archivo\n\nRevise la tabla de errores creada en el escritorio");
                    richTextBox2.Text = "ERROR";
                    richTextBox3.Text = "ERROR";
                    //HTML_error generarhtmlerrores = new HTML_error(Lista_errores,nombredelarchivo,rutaarchivo);
                }
            }
            else
            {

                MessageBox.Show("No hay errores lexicos ni errores sintacticos");
                
                //cantidad de tokens  MessageBox.Show(Lista_tokens.Count.ToString());
                ////recorrido de la lista for (int i = 0; i < Lista_tokens.Count; i++)
                //{
                //    MessageBox.Show(Lista_tokens[i].lexema.ToString());
                //}

                //for (int i = 0; i < tsimbolos.Count; i++)
                //{
                //    MessageBox.Show(tsimbolos[i].Tipo_variable + "\n" + tsimbolos[i].Nombre_variable + "\n" + tsimbolos[i].Valor_variable);
                //}
                //HTML_tokens generarhtmltokens = new HTML_tokens(Lista_tokens,nombredelarchivo,rutaarchivo);
                Traduccion tracuccion = new Traduccion(Lista_tokens2, richTextBox2);
                StreamWriter sr = new StreamWriter(@path + "\\"+nombredelarchivo+".py");
                sr.WriteLine(richTextBox2.Text);
                sr.Close();
            }
        }



        private void LimpiarDocumentosToolStripMenuItem_Click(object sender, EventArgs e)
        {

            richTextBox1.Text = "";
            richTextBox2.Text = "";
            richTextBox3.Text = "";
        }

        private void tablaDeTokensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            parser = new Analizador_sintactico();
            cantidaderrores = parser.Parser(Lista_tokens);
            erroressintacticos = parser.errorsintactico();
            //generar y abrir tabla de tokens 

            if (Lista_errores.Count > 0 || cantidaderrores > 0)
            {
                if (Lista_errores.Count > 0)
                {
                    MessageBox.Show("No se puede crear la tabla de tokens ya que hay erores lexicos en el archivo de entreda\nRevise el arvho de errores lexicos y sintacticos.");
                    //HTML_error generarhtmlerrores = new HTML_error(Lista_errores, nombredelarchivo, rutaarchivo);
                }
                if (cantidaderrores > 0)
                {
                    MessageBox.Show("No se puede crear la tabla de tokens ya que hay erores sintacticos en el archivo de entreda\nRevise el arvho de errores lexicos y sintacticos.");
                    //HTML_error generarhtmlerrores = new HTML_error(Lista_errores, nombredelarchivo, rutaarchivo);
                }
            }
            else
            {
                //hasta este puntos se va a generar la tabla de token
                //MessageBox.Show("No hay errores lexicos ni errores sintacticos");
                HTML_tokens generarhtmltokens = new HTML_tokens(Lista_tokens, nombredelarchivo, rutaarchivo);

            }


        }

        private void tablaDeSimbolosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //crear la tabla de simbolos si no hay errores

            parser = new Analizador_sintactico();
            cantidaderrores = parser.Parser(Lista_tokens);
            erroressintacticos = parser.errorsintactico();
            List<TabladeSimbolos> tsimbolos = parser.tabla();

            if (Lista_errores.Count > 0 || cantidaderrores > 0)
            {
                if (Lista_errores.Count > 0)
                {
                    MessageBox.Show("No se puede crear la tabla de tokens ya que hay erores lexicos en el archivo de entreda\nRevise el arvho de errores lexicos y sintacticos.");
                    //HTML_error generarhtmlerrores = new HTML_error(Lista_errores, nombredelarchivo, rutaarchivo);
                }
                if (cantidaderrores > 0)
                {
                    MessageBox.Show("No se puede crear la tabla de tokens ya que hay erores sintacticos en el archivo de entreda\nRevise el arvho de errores lexicos y sintacticos.");
                    //HTML_error generarhtmlerrores = new HTML_error(Lista_errores, nombredelarchivo, rutaarchivo);
                }
            }
            else
            {
                //hasta este puntos se va a generar la tabla de token
                //MessageBox.Show("No hay errores lexicos ni errores sintacticos");
                
                //para ver la tabla de simbolos

                //imprimir html de la tabla de simbolos
                HTML_tablasimbolos ht = new HTML_tablasimbolos(tsimbolos);

            }

        }

        private void tablaDeErroresLexicosYSintacticosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // aca se mostara la tabla de errores 
            parser = new Analizador_sintactico();
            cantidaderrores = parser.Parser(Lista_tokens);
            erroressintacticos = parser.errorsintactico();


            if (Lista_errores.Count > 0 || cantidaderrores > 0)
            {
                if (Lista_errores.Count > 0)
                {                    
                    HTML_error generarhtmlerrores = new HTML_error(Lista_errores, nombredelarchivo, rutaarchivo);
                }
                if (cantidaderrores > 0)
                {

                    HTML_error_sintactico generarhtmlerrores2 = new HTML_error_sintactico(erroressintacticos, nombredelarchivo, rutaarchivo);
                }
            }
            else
            {
                MessageBox.Show("No tiene errores para mostrar");
            }
        }
    }
}
