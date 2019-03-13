using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Windows.Storage;


namespace BestNumber
{   
    
    class Estadisticas
    {
        int Juegos_realizados;
        int Juegos_ganados;
        int Por_ciento_ganado;
        int Mejor_juego = 20;
        int Nivel;
        int Puntos_acumulados;
        int Puntos_del_juego;    
        SQLiteConnection conn = new SQLiteConnection("Juego6.sqlite");
        public StorageFolder folder = ApplicationData.Current.LocalFolder;
        public StorageFile datos_txt;
        public Estadisticas()
        {
            conn.CreateTable<Datos>();
            conn.CreateTable<Estado>();
            
        }
        public string Mostrar_estadisticas()
        {
            var query = conn.Table<Datos>();
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            foreach (var message in query)
            {
                Juegos_realizados = message.JuegosRealizados;
                Juegos_ganados = message.JuegosGanados;
                Por_ciento_ganado = message.PorCientoGanado;
                Mejor_juego = message.MejorJuego;
                Nivel = message.NivelJugador;
                Puntos_acumulados = message.PuntosAcumulados;
            }
            string mis_estadisticas =" " + loader.GetString("Estadisticas_letreroString") + "\n" +
                                     " " + loader.GetString("Juegos_realizados") + Juegos_realizados + "\n" +
                                     " " + loader.GetString("Juegos_ganados") + Juegos_ganados + "\n" +
                                     " " + loader.GetString("Por_ciento") +  Por_ciento_ganado + "\n" +
                                     " " + loader.GetString("Mejor_juego") + Mejor_juego + loader.GetString("intentos") + "\n" +
                                     " " + loader.GetString("Nivel")+ Nivel + "\n" +
                                     " " + loader.GetString("Puntos_acumulados") + Puntos_acumulados;
            return mis_estadisticas;
        }
        public void Escribir_estadisticas()
        {
            var s = conn.Insert(new Datos()
            {
                JuegosRealizados = Juegos_realizados, 
                JuegosGanados = Juegos_ganados,
                PorCientoGanado = Por_ciento_ganado,
                MejorJuego = Mejor_juego,
                NivelJugador = Nivel,
                PuntosAcumulados = Puntos_acumulados
                
            });
        }
        public void Actualizar()
        {
            Juegos_realizados = Juegos_realizados + 1;
            Actualizar_PorCientoGanado(); 
            Actualizar_Nivel_del_jugador();          
        }
        public void Actualizar_Juego_ganado(int Num_intentos)
        {
            Juegos_ganados = Juegos_ganados + 1;
            Actualizar_Puntos(Num_intentos);
            if (Num_intentos < Mejor_juego)
            {
                Mejor_juego = Num_intentos;
                Puntos_del_juego = Puntos_del_juego + 10;
            }
        }
        private void Actualizar_PorCientoGanado()
        {
            double porCiento = 100*Juegos_ganados/Juegos_realizados;
            Por_ciento_ganado = (int)porCiento;
        }
        private void Actualizar_Puntos(int N_intentos)
        {
            int ptos = 0;
            if (N_intentos > 10)
            {
                if (N_intentos > 15)
                    ptos = 0;
                else
                    ptos = 5;
            }
            else
            {
                switch (N_intentos)
                {
                    case 10:
                        ptos = 10;
                        break;
                    case 9:
                        ptos = 10;
                        break;
                    case 8:
                        ptos = 15;
                        break;
                    case 7:
                        ptos = 16;
                        break;
                    case 6:
                        ptos = 18;
                        break;
                    case 5:
                        ptos = 19;
                        break;
                    case 4:
                        ptos = 20;
                        break;
                    case 3:
                        ptos = 50;
                        break;
                    case 2:
                        ptos = 50;
                        break;
                    case 1:
                        ptos = 50;
                        break;
                    default:
                        ptos = 0;
                        break;
                }
            }
            Puntos_del_juego = ptos;
            Puntos_acumulados = Puntos_acumulados + Puntos_del_juego;
        }
        private void Actualizar_Nivel_del_jugador()
        {
            double calculo = Por_ciento_ganado * Puntos_acumulados / 10000;
            Nivel = (int)calculo;
        }
        public void Terminar_juego()
        {
            var s = conn.Insert(new Estado()
            {
                terminado = true
            });
        }
        public void No_Terminar_juego()
        {
            var s = conn.Insert(new Estado()
            {
                terminado = false
            });
        }
        public bool Juego_terminado()
        {
            try
            {
                var query = conn.Table<Estado>();
                bool x = true;
                foreach (var message in query)
                {
                    x = message.terminado;
                }
                return x;
            }
            catch
            {
                return true;
            }
        }
        public async void Compatibilidad()
        {
            try
            {
                datos_txt = await folder.CreateFileAsync("appdata.txt", CreationCollisionOption.FailIfExists);
                try
                {
                    await folder.DeleteAsync();
                }
                catch
                {

                }
               
            }
            catch
            {
                try
                {
                    datos_txt = await folder.GetFileAsync("appdata.txt");
                    string text = await FileIO.ReadTextAsync(datos_txt);
                    //
                    Juegos_realizados = Convert.ToInt32(text.Substring(0, 10));
                    Juegos_ganados = Convert.ToInt32(text.Substring(10, 10));
                    Por_ciento_ganado = Convert.ToInt32(text.Substring(30, 3));
                    Puntos_acumulados = Convert.ToInt32(text.Substring(33, 19));
                    Nivel = Convert.ToInt32(text.Substring(52, 10));
                    Mejor_juego = Convert.ToInt32(text.Substring(62, 2));
                    Escribir_estadisticas();
                    await folder.DeleteAsync();
                }
                catch
                {

                }
            }
        }
    }
    public class Datos
    {       
        public int JuegosRealizados { get; set; }       //rango de int 2,147,483,647
        public int JuegosGanados { get; set; }
        public int PorCientoGanado { get; set; }
        public int MejorJuego { get; set; }
        public int NivelJugador { get; set; }
        public int PuntosAcumulados { get; set; }
    }
    public class Estado
    {
        public bool terminado { get; set; }
    }
}
