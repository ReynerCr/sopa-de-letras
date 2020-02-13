namespace AppMobile.Model
{
    class Palabra
    {
        private int i, j, dir;
        public bool activo;
        private readonly string _texto;

        public string Texto
        {
            get { return _texto; }
        }

        public int I
        {
            get { return i; }
        }

        public int J
        {
            get { return j; }
        }

        public int Direccion
        {
            get { return dir; }
        }


        public Palabra(string text, int ii, int jj)
        {
            i = ii;
            j = jj;
            activo = true;
            _texto = text;
        }

        public Palabra(string text)
        {
            i = 0;
            j = 0;
            activo = true;
            _texto = text;
        }

        public void Posicion(int ii, int jj, int di)
        {
            i = ii;
            j = jj;
            dir = di;
        }

        public static bool operator ==(Palabra a, Palabra b)
        {
            
            if ((ReferenceEquals(a, null) && !ReferenceEquals(b, null)) ||
               (!ReferenceEquals(a, null) && ReferenceEquals(b, null)))
            {
                return false;
            }
                
            if (ReferenceEquals(a, b))
                return true;

            if (a.activo && string.Equals(a._texto, b._texto))
            {
                //maneja casos donde se pueda generar la misma palabra dos veces
                if (a.i != b.i)
                    a.i = b.i;
                if (a.j != b.j)
                    a.j = b.j;

                return true;
            }//if
            return false;
        }//public static bool operator ==(Palabra a, Palabra b)

        public static bool operator !=(Palabra a, Palabra b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            Palabra b = (Palabra) obj;
            return (this == b);
        }

        public override int GetHashCode()
        {
            return _texto.GetHashCode();
        }
    }
}
