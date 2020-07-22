using System;
using System.IO;

namespace AppMobile.Model
{
    public sealed class JugadorData
    {
        private int _nivel;
        private int _tiempo;

        public string Nombre { get; set; }

        public int Nivel
        {
            get { return _nivel; }
            set { _nivel = value; }
        }

        public int Tiempo
        {
            get { return _tiempo; }
            set { _tiempo = value; }
        }

        private static JugadorData _instance = null;
        public static JugadorData Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new JugadorData();

                return _instance;
            }
        }

        private JugadorData()
        {
            //cargar save.dat y revisar si esta corrupto o no
            bool cargado = CargarGuardado();
            if (!cargado)
            {
                if (File.Exists(Constantes._savePath))
                    EliminarProgreso();

                _nivel = 1;
                Nombre = string.Empty;
                _tiempo = 0;
            }//if save no cargado
        }

        private bool CargarGuardado()
        {
            try
            {
                string text = File.ReadAllText(Constantes._savePath);
                string[] lines = text.Split();

                //cargando nombre
                string l = lines[0];
                if (l.Length < 3 || l.Length > 10) //nombre no cumple con los requisitos
                    return false;
                Nombre = l;

                //cargando nivel
                l = lines[1];
                if (!int.TryParse(l, out _nivel)) //Si no es NaN
                    return false;

                if (_nivel < 0 || _nivel > 6)
                    return false;

                //cargando tiempo si lo hay
                l = lines[2];
                if (!int.TryParse(l, out _tiempo))
                    return false;

                if (_tiempo < 0)
                    return false;

            }//try
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool GuardarProgreso()
        {
            try
            {
                File.WriteAllText(Constantes._savePath,
                                  Nombre + " " + _nivel + " " + _tiempo);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool EliminarProgreso()
        {
            try
            {
                Nombre = string.Empty;
                _nivel = 1;
                _tiempo = 0;
                File.Delete(Constantes._savePath);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }//public bool EliminarProgreso()

    }
}
