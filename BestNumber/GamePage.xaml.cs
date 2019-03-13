using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.UI.Popups;



// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace BestNumber
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        Juego mi_juego = new Juego();
        Estadisticas estdcas = new Estadisticas();
        private int numeroA;
        private int numeroB;
        private int numeroC;
        private int numeroD;
        public GamePage()
        {
            this.InitializeComponent();
            estdcas.Compatibilidad();
            if (estdcas.Juego_terminado())    
            {
                Starting();
               
            }
            else          //     Retomando juego
            {
                Resultados_textblock.Text = mi_juego.Leer_juego();
                NumbersBox.Text = " ? ? ? ? ";
                char[] mis_numeros = new char[4];
                mis_numeros = mi_juego.Numero;
                numeroA = mis_numeros[0];
                numeroB = mis_numeros[1];
                numeroC = mis_numeros[2];
                numeroD = mis_numeros[3];
                Estadisticas_textblock.Text = estdcas.Mostrar_estadisticas();
            }
        }
        public void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            MainPage Mi_HelpPage = new MainPage();

            this.Frame.Navigate(typeof(MainPage));
        }
        private void Starting()
        {
            NumbersBox.Text = " ? ? ? ? ";
            Resultados_textblock.Text = "";
            char[] mis_numeros = new char[4];
            mis_numeros = mi_juego.Numero;
            numeroA = mis_numeros[0];
            numeroB = mis_numeros[1];
            numeroC = mis_numeros[2];
            numeroD = mis_numeros[3];
            mi_juego.Num_intentos = 0;
            estdcas.No_Terminar_juego();
            Estadisticas_textblock.Text = estdcas.Mostrar_estadisticas();
            //TextBl_VerNumero.Text = Convert.ToString(numeroA) + numeroB + numeroC + numeroD;
        }
        private async void Boton_Test_Click(object sender, RoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            string t = Resultados_textblock.Text;
            if (estdcas.Juego_terminado())
              await  mostrar_cuadro_dialogo(loader.GetString("Start_button"));      //Presiona boton Comenzar                         
            else
            {
                try
                {
                    int[] numeros = await Comprobar(NumbersBox.Text);  
                    int toros = Contador_toros(numeros[0], numeros[1], numeros[2], numeros[3]);
                    int vacas = Contador_vacas(numeros[0], numeros[1], numeros[2], numeros[3]);
                    string el_numero =Convert.ToString(numeros[0]) + Convert.ToString(numeros[1]) + Convert.ToString(numeros[2]) + Convert.ToString(numeros[3]);
                    if (toros == 4)      // you win
                    {
                        mi_juego.Num_intentos++;
                        t = t + Environment.NewLine + el_numero + loader.GetString("4_toros");
                        estdcas.Terminar_juego();
                        estdcas.Actualizar();
                        estdcas.Actualizar_Juego_ganado(mi_juego.Num_intentos);
                        estdcas.Escribir_estadisticas();
                        Estadisticas_textblock.Text = estdcas.Mostrar_estadisticas();
                        //************************************************** Mensaje de dialogo
                        string message = estdcas.Mostrar_estadisticas();
                        string titulo = loader.GetString("You_win") + loader.GetString("Adivinaste") + numeroA + numeroB + numeroC + numeroD;
                        MessageDialog ganamos = new MessageDialog(message, titulo);
                        ganamos.Commands.Add(new UICommand(loader.GetString("Continuar")));
                        await ganamos.ShowAsync();
                        //*********************************************************************
                    }
                    else
                    {
                        if (mi_juego.Num_intentos == 0)
                        {
                            if ((toros > 1) && (vacas > 1))
                                t = t + " " + el_numero + loader.GetString("There_are") + toros + loader.GetString("bulls_and") + vacas + loader.GetString("cows");
                            if ((toros <= 1) && (vacas <= 1))
                                t = t + " " + el_numero + loader.GetString("There_are") + toros + loader.GetString("bull_and") + vacas + loader.GetString("cow");
                            if ((toros <= 1) && (vacas > 1))
                                t = t + " " + el_numero + loader.GetString("There_are") + toros + loader.GetString("bull_and") + vacas + loader.GetString("cows");
                            if ((toros > 1) && (vacas <= 1))
                                t = t + " " + el_numero + loader.GetString("There_are") + toros + loader.GetString("bulls_and") + vacas + loader.GetString("cow");
                        }
                        else
                        {
                            if ((toros > 1) && (vacas > 1))
                                t = t + Environment.NewLine + " " + el_numero + loader.GetString("There_are") + toros + loader.GetString("bulls_and") + vacas + loader.GetString("cows");
                            if ((toros <= 1) && (vacas <= 1))
                                t = t + Environment.NewLine + " " + el_numero + loader.GetString("There_are") + toros + loader.GetString("bull_and") + vacas + loader.GetString("cow");
                            if ((toros <= 1) && (vacas > 1))
                                t = t + Environment.NewLine + " " + el_numero + loader.GetString("There_are") + toros + loader.GetString("bull_and") + vacas + loader.GetString("cows");
                            if ((toros > 1) && (vacas <= 1))
                                t = t + Environment.NewLine + " " + el_numero + loader.GetString("There_are") + toros + loader.GetString("bulls_and") + vacas + loader.GetString("cow");
                        }
                        mi_juego.Num_intentos++;
                        if (mi_juego.Num_intentos > 15)
                        {
                            estdcas.Terminar_juego();
                            estdcas.Actualizar();
                            estdcas.Escribir_estadisticas();
                            t = t + Environment.NewLine + el_numero + "\n" +
                                loader.GetString("perdiste") +"\n" + " " + loader.GetString("perdiste2");
                            estdcas.Escribir_estadisticas();
                            Estadisticas_textblock.Text = estdcas.Mostrar_estadisticas();
                        }
                        else
                        {
                            mi_juego.Guardar_juego(t);
                            estdcas.No_Terminar_juego();
                        }
                    }
                    Resultados_textblock.Text = t;
                }
                catch(Exception_Vacio)
                {

                }
            }
        }
        private async Task<int[]> Comprobar(string texto)
        {
            string text = texto;
            text = text.Trim();
            char [] Numero = new char[4];
            int[] Numeros = new int[4];
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            try
            {
                if ((text.Length < 4) || (text.Length > 4))
                    throw new Exception_4_digitos();             //Deben ser 4 dígitos
                int text_int_version = 0;
                text_int_version = Convert.ToInt32(text);          // No letras
                text = Convert.ToString(text_int_version);
                if (text.Length < 4)
                    throw new Exception_No_comienza_con_0();    // No 0
                Numero = text.ToCharArray();
                string a = Convert.ToString(Numero[0]);
                string b = Convert.ToString(Numero[1]);
                string c = Convert.ToString(Numero[2]);
                string d = Convert.ToString(Numero[3]);
                if ((a == b) || (a == c) || (a == d) || (b == c) || (b == d) || (c == d))
                    throw new Exception_No_repetir_digitos();
                Numeros[0] = Convert.ToInt32(a); Numeros[1] = Convert.ToInt32(b); Numeros[2] = Convert.ToInt32(c); Numeros[3] = Convert.ToInt32(d);
            }
            catch(Exception_4_digitos)
            {
              await  mostrar_cuadro_dialogo(loader.GetString("4_digitos"));
                throw new Exception_Vacio();
            }
            catch(Exception_No_comienza_con_0)
            {
                await mostrar_cuadro_dialogo(loader.GetString("No_comienza_con_0"));
                throw new Exception_Vacio();
            }
            catch(Exception_No_repetir_digitos)
            {
                await mostrar_cuadro_dialogo(loader.GetString("No_repetir_digitos"));
                throw new Exception_Vacio();
            }
            catch
            {
                await mostrar_cuadro_dialogo(loader.GetString("No_letras"));
                throw new Exception_Vacio();
            }
            return Numeros ;
        }
        private int Contador_vacas(int un_numeroA, int un_numeroB, int un_numeroC, int un_numeroD)
        {
           
            int contador_de_vacas = 0;
            if ((un_numeroA == numeroB) || (un_numeroA == numeroC) || (un_numeroA == numeroD))
                contador_de_vacas++;
            if ((un_numeroB == numeroA) || (un_numeroB == numeroC) || (un_numeroB == numeroD))
                contador_de_vacas++;
            if ((un_numeroC == numeroA) || (un_numeroC == numeroB) || (un_numeroC == numeroD))
                contador_de_vacas++;
            if ((un_numeroD == numeroA) || (un_numeroD == numeroB) || (un_numeroD == numeroC))
                contador_de_vacas++;
            return contador_de_vacas;
        }
        private int Contador_toros(int un_numeroA, int un_numeroB, int un_numeroC, int un_numeroD)
        {
            int contador_de_toros = 0;
            if (un_numeroA == numeroA)
            {
                contador_de_toros++;
            }
            if (un_numeroB == numeroB)
            {
                contador_de_toros++;
            }
            if (un_numeroC == numeroC)
            {
                contador_de_toros++;
            }
            if (un_numeroD == numeroD)
            {
                contador_de_toros++;
            }
            return contador_de_toros;
        }
        async Task mostrar_cuadro_dialogo(string aviso)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            MessageDialog un_aviso = new MessageDialog(aviso);
            un_aviso.Commands.Add(new UICommand(loader.GetString("Continuar")));
            await un_aviso.ShowAsync();
        }
        private void Boton1_Click(object sender, RoutedEventArgs e)
        {
            if (NumbersBox.Text.Length > 3)
                NumbersBox.Text = "";
            NumbersBox.Text = NumbersBox.Text + "1";           
        }

        private void Boton2_Click(object sender, RoutedEventArgs e)
        {
            if (NumbersBox.Text.Length > 3)
                NumbersBox.Text = "";
            NumbersBox.Text = NumbersBox.Text + "2";         
        }

        private void Boton3_Click(object sender, RoutedEventArgs e)
        {
            if (NumbersBox.Text.Length > 3)
                NumbersBox.Text = "";
            NumbersBox.Text = NumbersBox.Text + "3";
        }

        private void Boton4_Click(object sender, RoutedEventArgs e)
        {
            if (NumbersBox.Text.Length > 3)
                NumbersBox.Text = "";
            NumbersBox.Text = NumbersBox.Text + "4";
        }

        private void Boton5_Click(object sender, RoutedEventArgs e)
        {
            if (NumbersBox.Text.Length > 3)
                NumbersBox.Text = "";
            NumbersBox.Text = NumbersBox.Text + "5";
        }

        private void Boton6_Click(object sender, RoutedEventArgs e)
        {
            if (NumbersBox.Text.Length > 3)
                NumbersBox.Text = "";
            NumbersBox.Text = NumbersBox.Text + "6";
        }

        private void Boton7_Click(object sender, RoutedEventArgs e)
        {
            if (NumbersBox.Text.Length > 3)
                NumbersBox.Text = "";
            NumbersBox.Text = NumbersBox.Text + "7";
        }

        private void Boton8_Click(object sender, RoutedEventArgs e)
        {
            if (NumbersBox.Text.Length > 3)
                NumbersBox.Text = "";
            NumbersBox.Text = NumbersBox.Text + "8";
        }

        private void Boton9_Click(object sender, RoutedEventArgs e)
        {
            if (NumbersBox.Text.Length > 3)
                NumbersBox.Text = "";
            NumbersBox.Text = NumbersBox.Text + "9";
        }

        private void Boton0_Click(object sender, RoutedEventArgs e)
        {
            if (NumbersBox.Text.Length > 3)
                NumbersBox.Text = "";
            NumbersBox.Text = NumbersBox.Text + "0";
        }

        private void BotonBorrar_Click(object sender, RoutedEventArgs e)
        {
            int largo = NumbersBox.Text.Length;
            if (largo != 0)
                NumbersBox.Text = NumbersBox.Text.Remove(largo - 1);
        }
        //************************************************************************************************************
        UICommandInvokedHandler CommandInvokedHandler_tool()
        {
            
            return new UICommandInvokedHandler(this.Acciones);
        }

        private void Acciones(IUICommand command)
        {
            estdcas.Actualizar();
            estdcas.Terminar_juego();
            estdcas.Escribir_estadisticas();
            Starting();
            Estadisticas_textblock.Text = estdcas.Mostrar_estadisticas();
        }
        //****************************************************************************************************************
        private async void Boton_Start_Click(object sender, RoutedEventArgs e)
        {
            if(estdcas.Juego_terminado())
            {
                Starting();
            }
            else //     Abandonando juego
            {  
                    var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                    string message = loader.GetString("Are_you_sure");
                    MessageDialog advertencia = new MessageDialog(message);
                    var bb = CommandInvokedHandler_tool();

                    advertencia.Commands.Add(new UICommand(loader.GetString("Accept"), bb));
                    advertencia.Commands.Add(new UICommand(loader.GetString("Cancel")));
                    await advertencia.ShowAsync();         
            }
        }
    }
    class Exception_4_digitos : Exception
    {
        public Exception_4_digitos()
        {

        }
    }
    class Exception_No_comienza_con_0 : Exception
    {
        public Exception_No_comienza_con_0()
        {

        }
    }
   class Exception_No_repetir_digitos : Exception
    {
        public Exception_No_repetir_digitos()
        {

        }
    }
    class Exception_Vacio : Exception
    {
        public Exception_Vacio()
        {

        }
    }
}
    
       


