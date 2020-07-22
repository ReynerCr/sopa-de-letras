using AppMobile.Model;
using AppMobile.View;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AppMobile.ViewModels
{
    class ManejadorJuego
    {
        private Tablero _tablero;
        private int _puntuacion;
        private int _totalPalabras;
        private int direccion;
        private readonly List<LetraTablero> Activos;
        private Grid TableroLayout;
        private readonly List<Label> LabelPalabras;
        private LetraTablero[][] tableroButtons;
        private Contenedor contenedor;

        //INSTANCIA PARA SINGLETON-----------------------------------------------
        private static ManejadorJuego _instance;
        public static ManejadorJuego Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ManejadorJuego();

                return _instance;
            }
        }

        public void LinkToContenedor()
        {
            if (contenedor == null)
                contenedor = Contenedor.Instance;
        }

        private ManejadorJuego()
        {
            LabelPalabras = new List<Label>();
            Activos = new List<LetraTablero>();
            direccion = -1;
        }

        //PROPIEDADES GETTERS Y SETTERS----------------------------------------
        public JPistasPage PistasPage { get; set; }

        public JConfigsPage ConfigsPage { get; set; }

        public int Puntuacion
        {
            get { return _puntuacion; }
            private set
            {
                if (_puntuacion != value)
                    _puntuacion = value;
            }
        }

        public int TotalPalabras
        {
            get { return _totalPalabras; }
            private set
            {
                if (_totalPalabras != value)
                    _totalPalabras = value;
            }
        }

        public string CategoriaPalabras { get { return _tablero.GetCategoriaPalabras(); } }
        public Tablero GetTablero() { return _tablero; }
        public List<Label> GetLabels() { return LabelPalabras; }
        public string GetPalabra(int i) { return _tablero.GetPalabra(i).Texto; }


        //METODOS PARA MANEJAR DESDE EL VIEW-----------------------------
        /* Metodo para aumentar la puntuacion y devuelve un bool que indica si ya
         * se puede avanzar de nivel o no. */
        public bool AumentarPuntuacion()
        {
            ++_puntuacion;
            if (_puntuacion == _totalPalabras)
                return true;
            return false;
        }

        /* Metodo que reinicia el tablero. */
        public void Reiniciar(Grid tabLayout = null)
        {
            if (contenedor.IsBusy)
            {
                Inicializar(tabLayout);
                PistasPage.Reiniciar();
                if (contenedor.Temporizador)
                    contenedor.ReiniciarTemporizador();
            }
        }

        /* Metodo utilizado para volver al menu. */
        public void Limpiar()
        {
            _tablero = null;
            LabelPalabras.Clear();
            Activos.Clear();
        }

        /* Metodo que marca una palabra buscada en el tablero. */
        public void DescubrirPalabra()
        {
            if (PistasPage.DPalabra)
            {
                //aqui metodo para descubrir una palabra
                Palabra p = _tablero.GetPalabraActiva();
                if (p != null)
                {
                    int ip, jp;
                    ip = p.I;
                    jp = p.J;

                    if (Activos.Count != 0)
                        LimpiarActivos(false);

                    p.activo = false;
                    switch (p.Direccion)
                    {
                        case 1:
                            for (int i = 0; i < p.Texto.Length; i++) //abajo
                                Marcar(tableroButtons[ip + i][jp]);
                            break;
                        case 2:
                            for (int i = 0; i < p.Texto.Length; i++) //diag abajo derecha
                                Marcar(tableroButtons[ip + i][jp + i]);
                            break;
                        case 3:
                            for (int i = 0; i < p.Texto.Length; i++) //derecha
                                Marcar(tableroButtons[ip][jp + i]);
                            break;
                        case 4:
                            for (int i = 0; i < p.Texto.Length; i++) //diag arriba derecha
                                Marcar(tableroButtons[ip - i][jp + i]);
                            break;
                        case 5:
                            for (int i = 0; i < p.Texto.Length; i++) //arriba
                                Marcar(tableroButtons[ip - i][jp]);
                            break;
                        case 6:
                            for (int i = 0; i < p.Texto.Length; i++) //diag arriba iz
                                Marcar(tableroButtons[ip - i][jp - i]);
                            break;
                        case 7:
                            for (int i = 0; i < p.Texto.Length; i++)//izquierda
                                Marcar(tableroButtons[ip][jp - i]);
                            break;
                        case 8:
                            for (int i = 0; i < p.Texto.Length; i++) //diagonal abajo izq
                                Marcar(tableroButtons[ip + i][jp - i]);
                            break;
                    }//switch(tipo)
                }//if (p != null)
            }//if
        }

        /* Metodo que marca la primera letra de una palabra buscada
           en el tablero. */
        public void DescubrirLetra()
        {
            if (PistasPage.DLetra)
            {
                Palabra p = _tablero.GetPalabraActiva();
                if (p != null)
                {
                    if (Activos.Count != 0)
                        LimpiarActivos(false);
                    Marcar(tableroButtons[p.I][p.J]);
                }
            }//if
        }

        /* Metodo que comprueba si los botones pulsados corresponden a una palabra
           valida que se encuentre en la lista de palabras buscadas, ademas aumenta
           la puntuacion si asi lo es. */
        public bool ComprobarActivos()
        {
            if (Activos.Count == 0)
                return false;

            string txt = "";
            for (int i = 0; i < Activos.Count; i++)
            {
                txt += Activos[i].Text;
            }

            bool encontrado = false;
            for (int i = 0; i < LabelPalabras.Count; i++)
            {
                if (txt == LabelPalabras[i].Text)
                {
                    if (LabelPalabras[i].TextDecorations != TextDecorations.Strikethrough)
                    {
                        LabelPalabras[i].TextColor = Color.Gray;
                        LabelPalabras[i].TextDecorations = TextDecorations.Strikethrough;
                        _tablero.GetPalabra(i).activo = false;
                        encontrado = true;
                    }
                    break;
                }
            }//for

            if (encontrado)
                LimpiarActivos(true);
            else
                LimpiarActivos(false);

            return encontrado;
        }

        /* Metodo para limpiar la lista de palabras activadas. */
        public void LimpiarActivos(bool marcar)
        {
            if (Activos.Count == 0)
                return;

            LetraTablero actual = null;
            for (int i = 0; i < Activos.Count; i++) //mas rapido que foreach
            {
                actual = Activos[i];
                actual.Scale = 1;
                actual.BorderWidth = 1;
                actual.BackgroundColor = Color.FromHex("#ffffff");
                if (marcar)
                {
                    actual.BorderColor = Color.Red;
                    actual.TextColor = Color.Red;
                }

                actual.AltToggle();
            }
            actual.FontAttributes = FontAttributes.None;
            Activos.Clear();
        }

        //METODOS PRIVADOS-----------------------------------
        /* Metodo que inicializa todo el juego: tablero, tiempo, lista de palabras,
           puntuaciones. */
        private void Inicializar(Grid tabLayout = null)
        {
            int i, nivel, numLista;
            nivel = contenedor.Nivel;
            numLista = contenedor.EstiloPalabras;
            _totalPalabras = 4 + (nivel * 2);
            //generar _tablero
            if (_tablero == null)
                _tablero = new Tablero(nivel, numLista);
            else
                _tablero.Inicializar(nivel, numLista);

            //cargar lista de palabras en etiquetas
            if (LabelPalabras.Count != 0)
            {
                PistasPage.LimpiarLabels();
                LabelPalabras.Clear();
            }
            for (i = 0; i < TotalPalabras; i++)
                LabelPalabras.Add(new Label { Text = GetPalabra(i) });

            Activos.Clear();
            LabelPalabras.TrimExcess();

            //vaciar grid de tablero
            if (TableroLayout != null)
            {
                TableroLayout.Children.Clear();
                TableroLayout.RowDefinitions.Clear();
                TableroLayout.ColumnDefinitions.Clear();
            }
            if (tabLayout != null) //en teoria es para la primera vez
                TableroLayout = tabLayout;

            //inicializar colunas y filas del grid
            i = 0;
            nivel += 6; //reutilizo n
            while (i < nivel)
            {
                TableroLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                TableroLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                ++i;
            }

            //cargar labels e insertarlas en el grid
            tableroButtons = new LetraTablero[nivel][];
            char[][] tab = GetTablero().GetMatriz();
            for (i = 0; i < nivel; i++)
            {
                tableroButtons[i] = new LetraTablero[nivel];
                for (int j = 0; j < nivel; j++)
                {
                    tableroButtons[i][j] = new LetraTablero(i, j, tab[i][j]);
                    tableroButtons[i][j].Clicked += OnTableroButtonClicked;
                    TableroLayout.Children.Add(tableroButtons[i][j], i, j);
                }//for j
            }//for i

            //actualizar la puntuacion cuando se pasa de nivel o se reinicia tablero
            _puntuacion = -1;
            contenedor.AumentarPuntuacion();
        }

        /*  Revisa donde esta el actual desde el anterior
            -1 = no determinado
            0 = no cercano
            1 = abajo
            2 = abajo derecha
            3 = derecha
            4 = arriba derecha
            5 = arriba
            6 = arriba izquierda
            7 = izquierda
            8 = abajo izquierda */
        private int DetDireccion(LetraTablero lt, int dir = 0)
        {
            if (Activos.Count == 0) //no se ha determinado direccion
                return -1;

            int i = lt.I;
            int j = lt.J;
            int max = tableroButtons.Length;

            if ((dir == 0 || dir == 1) && (j - 1) >= 0 && tableroButtons[i][j - 1] == Activos[Activos.Count - 1])
                return 1;

            if (i - 1 >= 0 && dir < 5)
            {
                --i;
                if ((dir == 0 || dir == 2) && (j - 1) >= 0 && tableroButtons[i][j - 1] == Activos[Activos.Count - 1])
                    return 2;
                if ((dir == 0 || dir == 3) && tableroButtons[i][j] == Activos[Activos.Count - 1])
                    return 3;
                if ((dir == 0 || dir == 4) && (j + 1) < max && tableroButtons[i][j + 1] == Activos[Activos.Count - 1])
                    return 4;
                ++i;
            }//revisando hacia la derecha

            //arriba
            if ((dir == 0 || dir == 5) && (j + 1) < max && tableroButtons[i][j + 1] == Activos[Activos.Count - 1])
                return 5;

            if (i + 1 < max && dir < 9)
            {
                ++i;
                if ((dir == 0 || dir == 6) && (j + 1) < max && tableroButtons[i][j + 1] == Activos[Activos.Count - 1])
                    return 6;
                if ((dir == 0 || dir == 7) && tableroButtons[i][j] == Activos[Activos.Count - 1])
                    return 7;
                if ((dir == 0 || dir == 8) && (j - 1) >= 0 && tableroButtons[i][j - 1] == Activos[Activos.Count - 1])
                    return 8;
            }//revisando hacia izquierda

            return 0; //no esta cerca
        }

        /* Valida si hay un boton activo cercano al ultimo que se pulso,
           y si va en la misma direccion */
        private bool Cercania(LetraTablero lb, bool avanza)
        {
            //si no hay nada en la lista siempre es cercano, igual cuando va deshaciendo el camino
            if (Activos.Count == 0 || (!avanza && lb == Activos[Activos.Count - 1]))
                return true;

            int dir = DetDireccion(lb, direccion);
            if (dir != 0)
                return true;

            return false;
        }

        //Marcar un boton del grid
        private void Marcar(LetraTablero lb)
        {
            lb.AltToggle();

            if (lb.IsToggled)
            {
                lb.Scale = 1.3;
                lb.BorderWidth = 1.7;
                lb.FontAttributes = FontAttributes.Bold;
                lb.BackgroundColor = Color.FromHex("#bfbfbf");
                if (Activos.Count > 0)
                {
                    LetraTablero anterior = Activos[Activos.Count - 1];
                    anterior.BackgroundColor = Color.FromHex("#e6e6e6");
                    anterior.FontAttributes = FontAttributes.None;
                }
                Activos.Add(lb);
            }
            else
            {
                Activos.RemoveAt(Activos.Count - 1);
                lb.Scale = 1;
                lb.BorderWidth = 1;
                lb.BackgroundColor = Color.FromHex("#ffffff");
                lb.FontAttributes = FontAttributes.None;
                if (Activos.Count > 0)
                {
                    LetraTablero anterior = Activos[Activos.Count - 1];
                    anterior.BackgroundColor = Color.FromHex("#bfbfbf");
                    anterior.FontAttributes = FontAttributes.Bold;
                }
            }
        }

        //Eventos de los botones en el grid
        private void OnTableroButtonClicked(object sender, EventArgs args)
        {
            LetraTablero b = (LetraTablero)sender;
            if (Activos.Count == 1)
                direccion = DetDireccion(b);

            if (Cercania(b, !b.IsToggled))
            {
                Marcar(b);
            }//if Cercania
        }

    }
}
