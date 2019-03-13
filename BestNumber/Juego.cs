using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BestNumber
{
    class Juego
    {
        private int numero_a;
        private int numero_b;
        private int numero_c;
        private int numero_d;
        private char[] numero;
        public int Num_intentos;
        Estadisticas estad = new Estadisticas();
        SQLiteConnection conn = new SQLiteConnection("Juego6.sqlite");       
        public Juego()
        {
            conn.CreateTable<Datos_juego>(); 
        }
        
        public char[] Numero
        {
            get
            {
                if(estad.Juego_terminado())
                {
                    el_numero();
                    char a = Convert.ToChar(numero_a);
                    char b = Convert.ToChar(numero_b);
                    char c = Convert.ToChar(numero_c);
                    char d = Convert.ToChar(numero_d);
                    numero = new char[] { a, b, c, d };
                    return numero;
                }
                else
                {
                    char a = Convert.ToChar(numero_a);
                    char b = Convert.ToChar(numero_b);
                    char c = Convert.ToChar(numero_c);
                    char d = Convert.ToChar(numero_d);
                    numero = new char[] { a, b, c, d };
                    return numero;
                }
            }
        }
        private void el_numero()
        {
            Random numero = new Random();
            int r = 0;
            while (r == 0)
            {
                numero_a = numero.Next(1, 10);
                numero_b = numero.Next(0, 10);
                numero_c = numero.Next(0, 10);
                numero_d = numero.Next(0, 10);
                r = 1;
                if ((numero_a == numero_b) || (numero_a == numero_c) || (numero_a == numero_d) || (numero_b == numero_c) || (numero_b == numero_d) || (numero_c == numero_d))
                    r = 0;
            }
        }
        public void Guardar_juego(string contenido)
        {
            var s = conn.Insert(new Datos_juego()
            {
                numero1 = numero_a,
                numero2 = numero_b,
                numero3 = numero_c,
                numero4 = numero_d,
                Numero_de_intentos = Num_intentos,
                contenido_de_textBlock = contenido

            });
        }
        public string Leer_juego()
        {
            var query = conn.Table<Datos_juego>();
            string contenido = "";
            foreach (var message in query)
            {
                numero_a = message.numero1;
                numero_b = message.numero2;
                numero_c = message.numero3;
                numero_d = message.numero4;
                Num_intentos = message.Numero_de_intentos;
                contenido = message.contenido_de_textBlock;
            }
            estad.No_Terminar_juego();
            return contenido;
        }
       
    }
    public class Datos_juego
    {
        public int numero1 { get; set; }
        public int numero2 { get; set; }
        public int numero3 { get; set; }
        public int numero4 { get; set; }
        public int Numero_de_intentos { get; set; }
        public string contenido_de_textBlock { get; set; }
    }
    
    
}
