using System;
using System.Collections;

namespace AppMobile.ViewModels
{
    public class InstruccionesText
    {
        public string Texto { get; set; }

        public InstruccionesText(int i)
        {
            switch (i)
            {
                case 1:
                    Texto = "Para poder marcar las palabras se debe tocar en orden letra " +
                "a letra hasta formarla, luego se debe presionar el botón de comprobar " +
                "(palomita de confirmado) para verificar si es correcta o no.";
                    break;
                case 2:
                    Texto = "El botón de limpiar (una x) se usa para desactivar " +
                "las letras que se encuentran marcadas.";
                    break;
                case 3:
                    Texto = "En la parte superior izquierda se encuentra el botón para " +
                "acceder al menú de pistas, donde se encuentran los botones para activar " +
                "dichas pistas y también está la lista de palabras que se están buscando.";
                    break;
                case 4:
                    Texto = "En la parte superior derecha se encuentra el botón de " +
                "configuraciones, donde se puede reiniciar el tablero o volver al menú " +
                "(no guarda el tablero actual ni el puntaje actual).";
                    break;
                case 5:
                    Texto = "Abajo se encuentra el tema de las palabras del nivel actual, el " +
                "puntaje actual, y el puntaje requerido para pasar al siguiente nivel.";
                    break;
            }//switch
        }//public InstruccionesText()
    }//class InstruccionesText
}
