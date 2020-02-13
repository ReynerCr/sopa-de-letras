using System.Collections.Generic;

namespace AppMobile.Model
{
    class Tablero
    {
        private char[][] _matriz;
        private string _categoriaPalabras;
        private List<Palabra> _palabras;

        public ref char[][] GetMatriz() { return ref _matriz; }

        public Tablero(int nivel, int lista)
        {
            lista = 1;
            Inicializar(nivel, lista);
        }//public Tablero(int nivel, int lista)

        public void Inicializar(int nivel, int lista)
        {
            Matriz mapa = new Matriz("categoria" + lista + ".dat", nivel);
            _matriz = mapa.GetMatriz();
            if (_palabras != null)
                _palabras.Clear();
            _palabras = mapa.GetPalabras();
            _categoriaPalabras = mapa.GetCategoriaPalabras();
            _palabras.TrimExcess();
            mapa = null;
        }//public void Reiniciar(int nivel, int lista)

        public string GetCategoriaPalabras() { return _categoriaPalabras; }
        public int GetTamanyoLista() { return _palabras.Count; }
        
        public Palabra GetPalabra(int i)
        {
            if (i < 0 || i > (_palabras.Count - 1))
                return null;
            return _palabras[i];
        }//public string GetPalabra(int i)

        public Palabra GetPalabraActiva()
        {
            int i = 0;
            Palabra pal = null;
            while (i < _palabras.Count)
            {
                if (_palabras[i].activo)
                {
                    pal = _palabras[i];
                    break;
                }
                ++i;
            }
            return pal;
        }//public Palabra GetPalabraActiva()
    }//class Tablero
}
