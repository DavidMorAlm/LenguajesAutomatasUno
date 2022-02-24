using System;

namespace Lexico_0
{
    public class Program
    {
        static void Main(string[] args)
        {     
            Lexico a = new Lexico();
            while(!a.findArchivo())
            {
                a.nextToken();
            }
            a.close();
        }
    }
}
