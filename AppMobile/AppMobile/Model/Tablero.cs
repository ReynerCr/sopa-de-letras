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
            Inicializar(nivel, lista);
        }

        public void Inicializar(int nivel, int lista)
        {
            Matriz mapa = new Matriz("categoria" + lista + ".dat", nivel);
            _matriz = mapa.GetMatriz();
            if (_palabras != null)
                _palabras.Clear();
            _palabras = mapa.GetPalabras();
            _categoriaPalabras = mapa.GetCategoriaPalabras();
            _palabras.TrimExcess();
        }

        public string GetCategoriaPalabras() { return _categoriaPalabras; }
        public int GetTamanyoLista() { return _palabras.Count; }

        public Palabra GetPalabra(int i)
        {
            if (i < 0 || i > (_palabras.Count - 1))
                return null;
            return _palabras[i];
        }

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
        }

    }
}
