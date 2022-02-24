namespace Lexico_0
{
    /*
        Requerimiento 1: Agregar la notacion matematica a los numeros, ejemplo: 3.5e-8
        Requerimiento 2: Programar el reconocimiento de comentarios de linea sin que 
                         sea considerado como un token, ejemplo:
                         x26 = 5 //hola mundo
                         ;
    */
    public class Lexico : Token
    {
        StreamReader archivo;
        StreamWriter Log;

        public Lexico()   //Constructor
        {
            archivo = new StreamReader("C:/Users/David/OneDrive/Documentos/csharp/Lexico_0/Prueba.cpp");
            Log     = new StreamWriter("C:/Users/David/OneDrive/Documentos/csharp/Lexico_0/Prueba.log");
            Log.AutoFlush = true;
        }
        public void close()
        {
            archivo.Close();
            Log.Close();
        }
        public void nextToken()
        {
            char c;
            string buffer = "";
            bool huboDiagonal = false;

            while(char.IsWhiteSpace(c = (char) archivo.Read()));

            if (c == '/')
            {
                buffer += c;
                huboDiagonal = true;
                setClasificacion(tipos.OperadorFactor);
                if ('=' == (c = (char)archivo.Peek()))
                {
                    buffer += c;
                    setClasificacion(tipos.IncrementoFactor);
                    archivo.Read();
                }
                else if ('/' == (c = (char)archivo.Peek()))
                {
                    //Entonces es un comentario
                    //Se necesita agragar un ciclo hasta encontrar fin de linea o fin de archivo.
                    //Fin de linea = #10
                    //Fin de archivo = true
                    buffer = "";
                    huboDiagonal = false;
                    while('\n' != (c = (char) archivo.Read()) && !findArchivo());
                    return;
                }
            }

            if (huboDiagonal)
            {  
            }
            else if (char.IsLetter(c))
            {
                buffer += c;
                while(char.IsLetterOrDigit(c = (char) archivo.Peek()))
                {
                    buffer += c;
                    archivo.Read();
                }
                setClasificacion(tipos.Identificador);
            }
            else if (char.IsDigit(c))
            {
                buffer += c;
                while(char.IsDigit(c = (char) archivo.Peek()))
                {
                    buffer += c;
                    archivo.Read();
                }
                if (c == '.')
                {
                    buffer += c;
                    archivo.Read();
                    if (char.IsDigit(c = (char) archivo.Peek()))
                    {
                        while(char.IsDigit(c = (char) archivo.Peek()))
                        {
                            buffer += c;
                            archivo.Read();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error léxico: Se espera un dígito.");
                        Log.WriteLine("Error léxico: Se espera un dígito.");
                        return;
                    }
                }
                if (c == 'E' || c == 'e')
                {
                    buffer += c;
                    archivo.Read();
                    if (char.IsDigit((c = (char) archivo.Peek())))
                    {
                        while (char.IsDigit((c = (char) archivo.Peek())))
                        {
                            buffer += c;
                            archivo.Read();
                        }
                    }
                    else if ('+' == (c = (char) archivo.Peek()) || '-' == (c = (char) archivo.Peek()))
                    {
                        buffer += c;
                        archivo.Read();
                        if (char.IsDigit((c = (char) archivo.Peek())))
                        {
                            while (char.IsDigit((c = (char) archivo.Peek())))
                            {
                                buffer += c;
                               archivo.Read();
                            }
                        }
                        else 
                        {
                            Log.WriteLine("Error léxico: Se espera un dígito.");
                            Console.WriteLine("Error léxico: Se espera un dígito.");
                            return;
                        }
                    }
                    else
                    {
                        Log.WriteLine("Error léxico: Se espera un dígito.");
                        Console.WriteLine("Error léxico: Se espera un número.");
                        return;
                    }
                }
                setClasificacion(tipos.Numero);
            }
            else if (c == ';')
            {
                buffer += c;
                setClasificacion(tipos.FinSentencia);
            }
            else if (c == '=')
            {
                buffer += c;
                setClasificacion(tipos.Asignacion);
                if ((c = (char) archivo.Peek()) == '=')
                {
                    buffer += c;
                    setClasificacion(tipos.OperadorRelacional);
                    archivo.Read();
                }
            }
            else if (c == '*')
            {
                buffer += c;
                setClasificacion(tipos.OperadorFactor);
                if  ('=' == (c = (char)archivo.Peek()))
                {
                    buffer += c;
                    setClasificacion(tipos.IncrementoFactor);
                    archivo.Read();
                }
            }
            else if (c == '%')
            {
                buffer += c;
                setClasificacion(tipos.OperadorFactor);
                if  ('=' == (c = (char)archivo.Peek()))
                {
                    buffer += c;
                    setClasificacion(tipos.IncrementoFactor);
                    archivo.Read();
                }
            }
            else if (c == '+')
            {
                buffer += c;
                setClasificacion(tipos.OperadorTermino);
                if  ('+' == (c = (char)archivo.Peek()) ||'=' == (c = (char)archivo.Peek()))
                {
                    buffer += c;
                    setClasificacion(tipos.IncrementoTermino);
                    archivo.Read();
                }
            }
            else if (c == '-')
            {
                buffer += c;
                setClasificacion(tipos.OperadorTermino);
                if  ('-' == (c = (char)archivo.Peek()) ||'=' == (c = (char)archivo.Peek()))
                {
                    buffer += c;
                    setClasificacion(tipos.IncrementoTermino);
                    archivo.Read();
                }
            }
            else if (c == '&')
            {
                buffer += c;
                setClasificacion(tipos.Caracter);
                if ((c = (char) archivo.Peek()) == '&')
                {
                    buffer += c;
                    setClasificacion(tipos.OperadorLogico);
                    archivo.Read();
                }
            }
            else if (c == '|')
            {
                buffer += c;
                setClasificacion(tipos.Caracter);
                if ((c = (char) archivo.Peek()) == '|')
                {
                    buffer += c;
                    setClasificacion(tipos.OperadorLogico);
                    archivo.Read();
                }
            }
            else if (c == '!')
            {
                buffer += c;
                setClasificacion(tipos.OperadorLogico);
                if ((c = (char) archivo.Peek()) == '=')
                {
                    buffer += c;
                    setClasificacion(tipos.OperadorRelacional);
                    archivo.Read();
                }
            }
            else if (c == '>')
            {
                buffer += c;
                setClasificacion(tipos.OperadorRelacional);
                if ((c = (char) archivo.Peek()) == '=')
                {
                    buffer += c;
                    setClasificacion(tipos.OperadorRelacional);
                    archivo.Read();
                }
            }
            else if (c == '<')
            {
                buffer += c;
                setClasificacion(tipos.OperadorRelacional);
                if((c = (char) archivo.Peek()) == '=')
                {
                    buffer += c;
                    setClasificacion(tipos.OperadorRelacional);
                    archivo.Read();
                }
            }
            else if (c == '"')
            {
                buffer += c;
                setClasificacion(tipos.Cadena);
                while('"' != (c = (char) archivo.Read()))
                {
                    buffer += c;
                }
                buffer += c;
            }
            else if (c == '?')
            {
                buffer += c;
                setClasificacion(tipos.OperadorTernario);
            }
            else if (c == ':')
            {
                buffer += c;
                setClasificacion(tipos.Caracter);
                if((c = (char) archivo.Peek()) == '=')
                {
                    buffer += c;
                    setClasificacion(tipos.Inicializacion);
                    archivo.Read();
                }
            }
            else
            {
                buffer += c;
                setClasificacion(tipos.Caracter);
            }
            setContenido(buffer);
            Log.WriteLine(getContenido() + " | " + getClasificacion());
        }
        public bool findArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}