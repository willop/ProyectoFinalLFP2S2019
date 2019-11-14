using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalLFP
{
    class Analizador_sintactico
    {
        //---------------------------------------------------------- variable para la consola -------------------------------------------------
        string contenidodeejecucion = "";


        bool primeravez = true;
        int cantidaddeerroressintacticos = 0;
        int fila = 0;
        int columna = 0;
        int contador;
        Token tokenactual;
        List<String> Lista_traduccion = new List<string>();
        List<Token> Listatokensparser;
        List<Error_Tokens> Listaerroressintacticos = new List<Error_Tokens>();
        List<TabladeSimbolos> Tablade_simbolos = new List<TabladeSimbolos>();
        int globalresultado =0;

        //-------------------------------------------------varables para la tabla de simbolos--------------------------
        string T_tipo_variable="";
        string T_nombre_variable="";
        string T_valor_variable="";
        bool esid = false;


    //variable para concatenar los arreglos
        string valor_arreglo="";
        string nombre_id_arreglo = "";
        string nombre_grafica_arreglo = "";


        //----------------------------------------------------------------variables para la condicion if------------------------------------------
        int variable1 = 0;
        int variable2 = 0;
        string operadordelif = "";







        //Iniciamos el analizador sintactico con la primera produccion
        public int Parser(List<Token> Listatokens) {
            Listaerroressintacticos.Clear();
            this.Listatokensparser = Listatokens;
            contador = 0;
            tokenactual = Listatokensparser[contador];
            inicio();
            return cantidaddeerroressintacticos;
        }

        public void inicio() {
            if (tokenactual.id_token.Equals("TK_class"))
            {
                match("TK_class");
                match("TK_id");
                match("TK_{");
                match("TK_static");
                match("TK_void");
                match("TK_Main");
                match("TK_(");
                match("TK_var_String");
                match("TK_[");
                match("TK_]");
                match("TK_args");
                match("TK_)");
                match("TK_{");
                //contenido dentro de la clase
                contenido();
                //termina el contenido de la clase
                match("TK_}");
                match("TK_}");
            }
            else
            {
                // para errores de no venir class de primero
            }

        }



        //-----------------------------------------------------------------------contenido----------------------------------------------------------------------------------------------------
        public void contenido() {
            if (tokenactual.id_token.Equals("TK_if"))
            {
                IF();
                contenido();

            }
            if (tokenactual.id_token.Equals("TK_for"))
            {
                FOR();
                contenido();
            }
            if (tokenactual.id_token.Equals("TK_while"))
            {
                While();
                contenido();
            }
            if (tokenactual.id_token.Equals("TK_Console"))
            {
                Imprimir();
                contenido();
            }
            if (tokenactual.id_token.Equals("TK_switch"))
            {
                SWITCH();
                contenido();
            }

            if (tokenactual.id_token.Equals("TK_metodo_graficar_vector"))
            {
                graficar_vector();
                contenido();
            }

            //if para ver si es una declaracion o id
            if (tokenactual.id_token.Equals("TK_var_int") || tokenactual.id_token.Equals("TK_var_String") || tokenactual.id_token.Equals("TK_var_char") || tokenactual.id_token.Equals("TK_var_bool") || tokenactual.id_token.Equals("TK_var_float") || tokenactual.id_token.Equals("TK_id"))
            {
                variable();
                contenido();
            }

            else
            {
                //epsilon para contenido

            }
        }

        //--------------------------------------------------------------------------------------------para declarar variables---------------------------------------------------------------------
        public void variable() {
            tipo_variable();
            if (tokenactual.id_token.Equals("TK_id"))
            {
                T_nombre_variable = tokenactual.lexema;
                match("TK_id");
                asignacion();
                otra_variable();
                match("TK_pcoma");

                if (esid==true)
                {                   
                    int index = Tablade_simbolos.FindIndex(X => X.Nombre_variable == T_nombre_variable);
                    if (index>=0)
                    {
                        Tablade_simbolos[index].Valor_variable = globalresultado.ToString();
                        globalresultado = 0;
                        T_nombre_variable = "";
                        T_tipo_variable = "";
                        T_valor_variable = "";
                        esid = false;
                    }
                    else
                    {
                        Tablade_simbolos.Add(new TabladeSimbolos("no de declaro", T_nombre_variable, "no de declaro"));
                        T_nombre_variable = "";
                        T_tipo_variable = "";
                        T_valor_variable = "";
                        esid = false;
                    }

                }
                else
                {
                    Tablade_simbolos.Add(new TabladeSimbolos(T_nombre_variable, T_tipo_variable, T_valor_variable));
                    globalresultado =0;
                    T_nombre_variable = "";
                    T_tipo_variable = "";
                    T_valor_variable = "";
                    esid = false;
                }
                
                variable();
            }
            else {

                //epsilon
            }



        }

        public void asignacion() {
            if (tokenactual.id_token.Equals("TK_igual"))
            {
                match("TK_igual");
                asignar_valor();
            }
            else
            {
                //epsilon
            }

        }
        public void asignar_valor() {
            t();
            suma_resta();
            

        }

        public void suma_resta() {
            int temporal = globalresultado;
            if (tokenactual.id_token.Equals("TK_mas"))
            {
                //resultadoglobal = temporal + resultadoglobal;
                match("TK_mas");
                t();
                globalresultado = temporal + globalresultado;
                suma_resta();
            }
            if (tokenactual.id_token.Equals("TK_menos"))
            {
                match("TK_menos");
                t();
                globalresultado = temporal - globalresultado;
                suma_resta();
            }
            else {

                //epsilon
            }
        }

        public void t() {

            valores();
            multi_div();
        }


        public void multi_div() {
            int temporal = globalresultado;
            if (tokenactual.id_token.Equals("TK_por"))
            {
                match("TK_por");
                valores();
                globalresultado = temporal * globalresultado;
                multi_div();
            }
            if (tokenactual.id_token.Equals("TK_div"))
            {
                match("TK_div");
                valores();
                globalresultado = temporal / globalresultado;
                multi_div();
            }
            else
            {
                //otro epsilon
            }
        }

        public void valores() {

            if (tokenactual.id_token.Equals("TK_num_entero"))
            {
                //T_valor_variable = tokenactual.lexema;
                globalresultado = Convert.ToInt32(tokenactual.lexema);
                T_valor_variable = globalresultado.ToString();
                //temporal = Convert.ToInt32(tokenactual.lexema);
                match("TK_num_entero");
            }
            if (tokenactual.id_token.Equals("TK_("))
            {
                match("TK_(");
                globalresultado = Convert.ToInt32(tokenactual.lexema);
                asignar_valor();             
                match("TK_)");

            }
            if (tokenactual.id_token.Equals("TK_num_decimal_f"))
            {
                T_valor_variable = tokenactual.lexema;
                //decimal a = Convert.ToDecimal( tokenactual.lexema.Replace("f"," "));
                //globalresultado = Convert.ToDecimal(a);
                match("TK_num_decimal_f");
                
            }
            if (tokenactual.id_token.Equals("TK_num_decimal"))
            {
                T_valor_variable = tokenactual.lexema;
                //temporal = Convert.ToDecimal(tokenactual.lexema);
                match("TK_num_decimal");
            }
            if (tokenactual.id_token.Equals("TK_true"))
            {
                T_valor_variable = tokenactual.lexema;
                //temporal = true;
                match("TK_true");
            }
            if (tokenactual.id_token.Equals("TK_false"))
            {
                T_valor_variable = tokenactual.lexema;
                //temporal = false;
                match("TK_false");
            }
            if (tokenactual.id_token.Equals("TK_caracter"))
            {
                T_valor_variable = tokenactual.lexema;
                //temporal = tokenactual.lexema;
                match("TK_caracter");
            }
            if (tokenactual.id_token.Equals("TK_cadena"))
            {
                T_valor_variable = tokenactual.lexema;
                //temporal = tokenactual.lexema;
                match("TK_cadena");
            }
            if (tokenactual.id_token.Equals("TK_new"))
            {

                match("TK_new");
                Tablade_simbolos.Add(new TabladeSimbolos(T_nombre_variable, T_tipo_variable, T_valor_variable));
                T_nombre_variable = "";
                T_tipo_variable = "";
                T_valor_variable = "";
                tipo_variable();

            }
            if (tokenactual.id_token.Equals("TK_{"))
            {
                match("TK_{");
                tipo_valores();
                otros_tipos();
                match("TK_}");
                T_valor_variable = valor_arreglo;
                valor_arreglo = "";

            }
            if (tokenactual.id_token.Equals("TK_id"))
            {
                //T_valor_variable = tokenactual.lexema;
                int index = Tablade_simbolos.FindIndex(X => X.Nombre_variable == tokenactual.lexema);
                if (index >= 0)
                {
                    globalresultado = Convert.ToInt32(Tablade_simbolos[index].Valor_variable);
                }
                else
                {
                    globalresultado = 100000000;
                }
                match("TK_id");
                arreglo();
            }

        }

        public void otra_variable() {
            if (tokenactual.id_token.Equals("TK_coma"))
            {
                match("TK_coma");
                esid = false;
                Tablade_simbolos.Add(new TabladeSimbolos(T_nombre_variable, T_tipo_variable, T_valor_variable));
                T_valor_variable = "";
                T_nombre_variable = tokenactual.lexema;
                match("TK_id");             
                asignacion();
                otra_variable();
            }
            else
            {
                //epsilon
            }

        }



        public void tipo_variable()
        {
            T_tipo_variable = "";
            T_valor_variable = "";
            T_nombre_variable = "";
            if (tokenactual.id_token.Equals("TK_var_int"))
            {
                esid = false;
                T_tipo_variable = tokenactual.lexema;
                match("TK_var_int");
                arreglo();
            }
            if (tokenactual.id_token.Equals("TK_var_String"))
            {
                esid = false;
                T_tipo_variable = tokenactual.lexema;
                match("TK_var_String");
                arreglo();
            }
            if (tokenactual.id_token.Equals("TK_var_char"))
            {
                esid = false;
                T_tipo_variable = tokenactual.lexema;
                match("TK_var_char");
                arreglo();
            }
            if (tokenactual.id_token.Equals("TK_var_bool"))
            {
                esid = false;
                T_tipo_variable = tokenactual.lexema;
                match("TK_var_bool");
                arreglo();
            }
            if (tokenactual.id_token.Equals("TK_var_float"))
            {
                esid = false;
                T_tipo_variable = tokenactual.lexema;
                match("TK_var_float");
                arreglo();
            }
            else
            {
                if (T_tipo_variable.Equals(""))
                {
                    esid = true;
                }
                else
                {
                    esid = false;
                }
                
                //epsilon
            }


        }
        public void arreglo()
        {
            if (tokenactual.id_token.Equals("TK_["))
            {
                match("TK_[");
                cont_dentro_arreglo();
                match("TK_]");
                T_tipo_variable = "Arreglo de " + T_tipo_variable;
            }
            else
            {
                //epsilon
            }



        }

        public void tipo_valores() {
            if (tokenactual.id_token.Equals("TK_num_entero"))
            {
                valor_arreglo += tokenactual.lexema;
                match("TK_num_entero");
                Mas_contenido_Console();
            }
            if (tokenactual.id_token.Equals("TK_num_decimal"))
            {
                valor_arreglo += tokenactual.lexema;
                match("TK_num_decimal");
                Mas_contenido_Console();
            }
            if (tokenactual.id_token.Equals("TK_caracter"))
            {
                valor_arreglo += tokenactual.lexema;
                match("TK_caracter");
                Mas_contenido_Console();
            }
            if (tokenactual.id_token.Equals("TK_cadena"))
            {
                valor_arreglo += tokenactual.lexema;
                match("TK_cadena");
                Mas_contenido_Console();
            }
            if (tokenactual.id_token.Equals("TK_id"))
            {
                valor_arreglo += tokenactual.lexema;
                match("TK_id");
                Mas_contenido_Console();
            }


        }

        public void otros_tipos() {
            if (tokenactual.id_token.Equals("TK_coma"))
            {
                valor_arreglo += tokenactual.lexema;
                match("TK_coma");
                
                tipo_valores();
                otros_tipos();
            }
            else
            {
                //epsilon
            }
        }

        public void cont_dentro_arreglo() {
            if (tokenactual.id_token.Equals("TK_id"))
            {
                match("TK_id");
            }
            if (tokenactual.id_token.Equals("TK_num_entero"))
            {
                match("TK_num_entero");
            }
            else
            {
                //epsilon
            }
        }

        //---------------------------------------------gramatica para Console.WriteLine()------------------------------------------------------------------------------------------------------
        public void Imprimir() {
            if (tokenactual.id_token.Equals("TK_Console"))
            {
                match("TK_Console");
                match("TK_punto");
                match("TK_WriteLine");
                match("TK_(");
                contenido_Console();
                match("TK_)");
                match("TK_pcoma");
                contenidodeejecucion += "\n";
                Imprimir();
            }
            else
            {
                //epsilon
            }

        }

        public void contenido_Console() {
            if (tokenactual.id_token.Equals("TK_id"))
            {
                int index = Tablade_simbolos.FindIndex(X => X.Nombre_variable == tokenactual.lexema);
                if (index >= 0)
                {
                    contenidodeejecucion += Tablade_simbolos[index].Valor_variable.ToString();
                    match("TK_id");
                }
                else
                {
                    contenidodeejecucion+="busca un id que no existe";
                }
                //asignar_valor();
                Mas_contenido_Console();
            }
            if (tokenactual.id_token.Equals("TK_caracter"))
            {
                contenidodeejecucion += tokenactual.lexema;
                match("TK_carcter");
                //asignar_valor();
                Mas_contenido_Console();
            }
            if (tokenactual.id_token.Equals("TK_num_entero"))
            {
                contenidodeejecucion += tokenactual.lexema;
                //asignar_valor();
                match("TK_num_entero");
                Mas_contenido_Console();
            }
            if (tokenactual.id_token.Equals("TK_num_decimal"))
            {
                contenidodeejecucion += tokenactual.lexema;
                match("TK_num_decimal");
                //asignar_valor();
                Mas_contenido_Console();
            }
            if (tokenactual.id_token.Equals("TK_cadena"))
            {
                contenidodeejecucion += tokenactual.lexema;
                match("TK_cadena");
                //asignar_valor();
                Mas_contenido_Console();
            }
            if (tokenactual.id_token.Equals("TK_(") ||tokenactual.id_token.Equals("TK_num_decimal_f"))
            {
                //asignar_valor();
            }

        }

        public void Mas_contenido_Console() {

            if (tokenactual.id_token.Equals("TK_mas"))
            {
                contenidodeejecucion += " ";
                match("TK_mas");
                contenido_Console();
                //asignar_valor();
                //contenidodeejecucion += globalresultado.ToString();
                Mas_contenido_Console();
            }
            else
            {
                //epsilon
            }

        }

        //--------------------------------------------fin de la gramatica del Console.WriteLine

        //---------------------------------------------gramatica para graficar el vector------------------------

        public void graficar_vector() {
            if (tokenactual.id_token.Equals("TK_metodo_graficar_vector"))
            {
                match("TK_metodo_graficar_vector");
                match("TK_(");
                nombre_id_arreglo = tokenactual.lexema;
                match("TK_id");
                match("TK_coma");
                nombre_grafica_arreglo = tokenactual.lexema;
                match("TK_cadena");
                match("TK_)");
                match("TK_pcoma");
                Grafica_vector grafica = new Grafica_vector();
                grafica.graficar(nombre_id_arreglo,nombre_grafica_arreglo,Tablade_simbolos);

            }
            else
            {
                //epsilon ya que el graficar no es indispensable que venga  
            }

        }
        //--------------------------------------------------fin de la gramatica -------------------------------------------
        //-----------------------------------------------------------gramatica para el if-----------------------

        public void IF() {
            if (tokenactual.id_token.Equals("TK_if"))
            {
                match("TK_if");
                match("TK_(");
                condicion_if();
                match("TK_)");
                match("TK_{");
                cuerpo_if();
                match("TK_}");
                Otro_if();
                Else();
            }
            else
            {
                //epsilon
            }


        }

        public void Otro_if() {
            IF();

        }

        public void Else() {
            if (tokenactual.id_token.Equals("TK_else"))
            {
                match("TK_else");
                match("TK_{");
                cuerpo_if();
                match("TK_}");
            }
            else
            {
                //epsilon
            }


        }

        public void cuerpo_if() {
            contenido();


        }

        public void condicion_if() {
            parametro_if();
            operador_if();
            parametro_if();
            

            //-------------------aca se verificara la condicion del if



        }

        public void parametro_if() {
                if (tokenactual.id_token.Equals("TK_num_entero"))
                {
                    match("TK_num_entero");

                }
                if (tokenactual.id_token.Equals("TK_num_decimal"))
                {
                    match("TK_num_decimal");
                }
                if (tokenactual.id_token.Equals("TK_true"))
                {
                    match("TK_true");
                }
                if (tokenactual.id_token.Equals("TK_false"))
                {
                    match("TK_false");
                }
                if (tokenactual.id_token.Equals("TK_id"))
                {
                match("TK_id");
                    //int index = Tablade_simbolos.FindIndex(X => X.Nombre_variable == tokenactual.lexema);
                    //if (index >= 0)
                    //{
                    //    variable1= Convert.ToInt32(Tablade_simbolos[index].Valor_variable);
                    //    match("TK_id");
                    //}
                    //else
                    //{
                    //    contenidodeejecucion += "busca un id que no existe";
                    //}
                }

        }

        public void operador_if() {
            if (tokenactual.id_token.Equals("TK_igualacion"))
            {
                operadordelif = tokenactual.id_token;
                match("TK_igualacion");
            }
            if (tokenactual.id_token.Equals("TK_mayor"))
            {
                operadordelif = tokenactual.id_token;
                match("TK_mayor");
            }
            if (tokenactual.id_token.Equals("TK_mayor_igual"))
            {
                operadordelif = tokenactual.id_token;
                match("TK_mayor_igual");
            }
            if (tokenactual.id_token.Equals("TK_menor"))
            {
                operadordelif = tokenactual.id_token;
                match("TK_menor");
            }
            if (tokenactual.id_token.Equals("TK_menor_igual"))
            {
                operadordelif = tokenactual.id_token;
                match("TK_menor_igual");
            }
            if (tokenactual.id_token.Equals("TK_diferencia"))
            {
                operadordelif = tokenactual.id_token;
                match("TK_diferencia");
            }



        }
        // fin de gramatica del if----------------------------------------------------
    




        //---------------------------------------------------gramatia para el for-------------------
        public void FOR() {
            if (tokenactual.id_token.Equals("TK_for"))
            {
                match("TK_for");
                match("TK_(");
                declaracion_FOR();
                match("TK_pcoma");
                condicion_if();
                match("TK_pcoma");
                match("TK_id");
                Incremento_decremento();
                match("TK_)");
                match("TK_{");
                Cuerpo_for();
                match("TK_}");
                otro_FOR();
            }
            else
            {
                //epsilon
            }
        
        }

        public void declaracion_FOR() {
            if (tokenactual.id_token.Equals("TK_var_int"))
            {
                match("TK_var_int");
                match("TK_id");
                match("TK_igual");
                asignar_valor();
            }
        
        }

        public void Incremento_decremento() {

            if (tokenactual.id_token.Equals("TK_incremento"))
            {
                match("TK_incremento");
                
                
            }
            if (tokenactual.id_token.Equals("TK_decremento"))
            {
                match("TK_decremento");
                
            }
            else
            {
                //cantidaddeerroressintacticos++;
                //Listaerroressintacticos.Add(new Error_Tokens(tokenactual.id_token, "Error sintactico, se esperaba un token de incremento (++) o decremento (--)", tokenactual.columna, tokenactual.fila));
                //contador++;
                //tokenactual = Listatokensparser[contador];

            }
        
        }
        public void Cuerpo_for() {
            contenido();
        }

        public void otro_FOR()
        {
            contenido();
        }

        //----------------------------------------------------fin gramatica para el for----------------------------

        //-------------------------------------------inicio gramatica para while----------------------------

        public void While() {
            if (tokenactual.id_token.Equals("TK_while"))
            {
                match("TK_while");
                match("TK_(");
                condicion_if();
                match("TK_)");
                match("TK_{");
                cuerpo_While();
                match("TK_}");
                Otro_while();

            }
        }
        public void cuerpo_While() {
            contenido();

        }
   
        public void Otro_while() {
            While();               
        }
        //------------------------------------------------------fin gramatica while--------------------------------------------------


        //------------------------------------------------- inicia gramatica de switch ----------------------------------------

        public void SWITCH() {
            if (tokenactual.id_token.Equals("TK_switch"))
            {
                match("TK_switch");
                match("TK_(");
                CONDICION_SWITCH();
                match("TK_)");
                match("TK_{");
                LISTA_CASE();
                match("TK_}");
            }

        }

        public void CONDICION_SWITCH() {
            if (tokenactual.id_token.Equals("TK_num_entero"))
            {
                match("TK_num_entero");
            }
            if (tokenactual.id_token.Equals("TK_id"))
            {
                match("TK_id");
            }
        }

        public void LISTA_CASE() {
            if (tokenactual.id_token.Equals("TK_case"))
            {
                match("TK_case");
                CONDICION_SWITCH();
                match("TK_dospuntos");
                contenido();
                match("TK_break");
                match("TK_pcoma");
                OTRO_CASE();
                DEFAULT();
            }

        }

        public void OTRO_CASE()
        {
            if (tokenactual.id_token.Equals("TK_case"))
            {
                LISTA_CASE();
            }
            else
            {
                //epsilon
            }
        }

        public void DEFAULT() {
            if (tokenactual.id_token.Equals("TK_default"))
            {
                match("TK_default");
                match("TK_dospuntos");
                contenido();
                match("TK_break");
                match("TK_pcoma");
            }
            else
            {
                //epsilon
            }
        
        }


        public void match(String id_token_enviado) {

            if (id_token_enviado!=tokenactual.id_token)
            {
                // si se encuentra un error lo va a obviar y va a pasar al siguiente token
                Listaerroressintacticos.Add(new Error_Tokens(tokenactual.id_token,"Error sintactico, se esperaba "+id_token_enviado,tokenactual.columna,tokenactual.fila));
                    cantidaddeerroressintacticos++;
                    contador++;
                    tokenactual = Listatokensparser[contador];
            }
            else if (id_token_enviado!="fin")
            {
                contador++;
                tokenactual = Listatokensparser[contador];
            }
        
        }


        public List<TabladeSimbolos> tabla() {

            return Tablade_simbolos;
        }

        public List<Error_Tokens> errorsintactico() {
            return Listaerroressintacticos; 
        }


        public string ejecucion() {

            return contenidodeejecucion;
        }
        
        
    }
}
