using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace AppMobile.Model
{
    class Matriz
    {
        private readonly List<Palabra> _palabras;
        private char[][] _matriz;
        private readonly string _categoriaPalabras;
        private readonly int nM;
        private readonly int toltL;
        private bool good;

        private Matriz() { good = false; }

        //metodo de revolver
        private void Shuffle<T>(IList<T> values)
        {
            var n = values.Count;
            var rnd = new Random();
            for (int i = n - 1; i > 0; i--)
            {
                var j = rnd.Next(0, i);
                var temp = values[i];
                values[i] = values[j];
                values[j] = temp;
            }
        }

        //metodo de Ordenar de mayor a menor
        private void Ordenar(string[] values, int m)
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m - 1; j++)
                {
                    if (values[j].Length < values[j + 1].Length)
                    {
                        string letra = values[j];
                        values[j] = values[j + 1];
                        values[j + 1] = letra;
                    }
                }//for j
            }//for i
        }

        /*
            1 = abajo
            2 = abajo derecha
            3 = derecha
            4 = arriba derecha
            5 = arriba
            6 = arriba izquierda
            7 = izquierda
            8 = abajo izquierda
        */
        private bool Colocar(int x, int y, int tipo, string letra)
        {
            switch (tipo)
            {
                case 1:
                    for (int i = 0; i < letra.Length; i++) //abajo
                        _matriz[x + i][y] = letra[i];
                    break;
                case 2:
                    for (int i = 0; i < letra.Length; i++) //diag abajo derecha
                        _matriz[x + i][y + i] = letra[i];
                    break;
                case 3:
                    for (int i = 0; i < letra.Length; i++) //derecha
                        _matriz[x][y + i] = letra[i];
                    break;
                case 4:
                    for (int i = 0; i < letra.Length; i++) //diag arriba derecha
                        _matriz[x - i][y + i] = letra[i];
                    break;
                case 5:
                    for (int i = 0; i < letra.Length; i++) //arriba
                        _matriz[x - i][y] = letra[i];
                    break;
                case 6:
                    for (int i = 0; i < letra.Length; i++) //diag arriba iz
                        _matriz[x - i][y - i] = letra[i];
                    break;
                case 7:
                    for (int i = 0; i < letra.Length; i++)//izquierda
                        _matriz[x][y - i] = letra[i];
                    break;
                case 8:
                    for (int i = 0; i < letra.Length; i++) //diagonal abajo izq
                        _matriz[x + i][y - i] = letra[i];
                    break;
            }//switch(tipo)

            if (tipo > 0 && tipo < 9)
                return true;

            return false;
        }

        private bool Casilla(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < nM && y < nM)
                return true;

            return false;
        }

        /*
            1 = abajo
            2 = abajo derecha
            3 = derecha
            4 = arriba derecha
            5 = arriba
            6 = arriba izquierda
            7 = izquierda
            8 = abajo izquierda
        */
        private int Posible(int letra, int x, int y)
        {
            int cont = 0;
            int[] v = { 1, 2, 3, 4, 5, 6, 7, 8 };
            Shuffle(v);
            for (int k = 0; k < 8; k++)
            {
                int a = 1, b = 1;
                switch (v[k])
                {
                    case 1: b = 0; break;//y=b=0 abajo
                    case 2: break;//abajo derecha
                    case 3: a = 0; break;//derecha
                    case 4: a = -1; break;//derecha arriba
                    case 5: a = -1; b = 0; break;//arriba
                    case 6: a = -1; b = -1; break;//arriba iz
                    case 7: a = 0; b = -1; break;//iz
                    case 8: b = -1; break;//abaj iz
                }
                for (int i = 0; i < letra; i++)
                {
                    if (Casilla(x + a * (i), y + b * (i)) == true)
                    {
                        if (_matriz[x + a * (i)][y + b * (i)] == ' ')
                            ++cont;
                        else
                            break;
                    }
                }//for i
                if (cont == letra)
                    return v[k];
                else
                    cont = 0;
            }//for k
            return -1;
        }

        public Matriz(String ruta, int nivel)
        {
            string[] datos, letras;

            //lectura de archivo
            if (Device.RuntimePlatform != Device.GTK)
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("AppMobile.CommonResources." + ruta))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        for (int i = 1; i < nivel; ++i)
                            sr.ReadLine();

                        letras = sr.ReadLine().Split();
                        _categoriaPalabras = letras[0];
                        datos = new string[letras.Length - 1];
                        Array.Copy(letras, 1, datos, 0, letras.Length - 1);
                        letras = null;
                    }//using
                }//using
            }
            else
            {
                ruta = Path.Combine(Constantes.folderPath, ruta);
                using (StreamReader sr = new StreamReader(ruta))
                {
                    for (int i = 1; i < nivel; ++i)
                        sr.ReadLine();

                    letras = sr.ReadLine().Split();
                    _categoriaPalabras = letras[0];
                    datos = new string[letras.Length - 1];
                    Array.Copy(letras, 1, datos, 0, letras.Length - 1);
                    letras = null;
                }//using
            }

            if (datos != null && nivel > 0 && nivel < 7)
            {
                //inicializar datos 
                _palabras = new List<Palabra>();
                Shuffle(datos);
                nM = 6 + nivel;
                toltL = 4 + (2 * nivel);
                int aux = nM - 1;
                int total = toltL;
                letras = new string[toltL];
                int cont = 0;
                //tomar letras de datos
                for (int i = 0; i < datos.Length; i++)
                {
                    if (datos[i].Length < aux && total != 0)
                    {
                        letras[cont] = datos[i];
                        ++cont;
                        total--;
                    }
                    else if (total == 0)
                        break;
                }//for i

                //ordenar por tamaño
                Ordenar(letras, toltL);
                for (int i = 0; i < letras.Length; i++) //copiar en la lista de palabras las letras
                {
                    _palabras.Add(new Palabra(letras[i]));
                }

                //colocar palabras en matriz
                _matriz = new char[nM][];
                good = false;
                while (!good)
                {
                    for (int i = 0; i < nM; i++)
                    {
                        _matriz[i] = new char[nM];
                        for (int j = 0; j < nM; j++)
                        {
                            _matriz[i][j] = ' ';
                        }//for j
                    }//for i

                    int totaL = letras.Length; cont = 0;
                    int error = 0;
                    while (cont != totaL)
                    {
                        for (int i = 0; i < nM; i++)
                        {
                            for (int j = 0; j < nM; j++)
                            {
                                if (cont != totaL)
                                {
                                    int res;
                                    res = Posible(letras[cont].Length, i, j);//posicion posible en el mapa
                                    if (res != -1) //colocar palabra
                                    {
                                        _palabras[cont].Posicion(i, j, res);
                                        Colocar(i, j, res, letras[cont]);
                                        ++cont;
                                        ++j;
                                    }//if
                                }//if
                            }//for j
                        }//for i
                        ++error;
                        if (error == 3)
                            break;
                    }//while (cont != total)
                    if (error == 3)
                        good = false;
                    else
                        good = true;
                }//while (good)

                //terminar de rellenar matriz
                Random valor = new Random();
                int num;
                for (int i = 0; i < nM; i++)
                {
                    for (int j = 0; j < nM; j++)
                    {
                        if (_matriz[i][j] == ' ')
                        {
                            num = valor.Next(65, 91);
                            if (num == 91)
                                num = 209;
                            _matriz[i][j] = (char)num;
                        }
                    }//for j
                }//for i

                //ordenar lista de palabras final
                _palabras.Sort((x, y) => x.Texto.CompareTo(y.Texto));
            }//if
            else
            {
                good = false;
            }//else
        }

        public ref char[][] GetMatriz() { return ref _matriz; }
        public string GetCategoriaPalabras() { return _categoriaPalabras; }
        public List<Palabra> GetPalabras() { return _palabras; }

    }
}
