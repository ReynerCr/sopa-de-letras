namespace AppMobile.Model
{
    class Palabra
    {
        public bool activo;
        public string Texto { get; }
        public int I { get; private set; }
        public int J { get; private set; }
        public int Direccion { get; private set; }


        public Palabra(string text, int ii, int jj)
        {
            I = ii;
            J = jj;
            activo = true;
            Texto = text;
        }

        public Palabra(string text)
        {
            I = 0;
            J = 0;
            activo = true;
            Texto = text;
        }

        public void Posicion(int ii, int jj, int di)
        {
            I = ii;
            J = jj;
            Direccion = di;
        }

        public static bool operator ==(Palabra a, Palabra b)
        {

            if ((a is null && b is object) ||
               (a is object && b is null))
            {
                return false;
            }

            if (ReferenceEquals(a, b))
                return true;

            if (a.activo && string.Equals(a.Texto, b.Texto))
            {
                //maneja casos donde se pueda generar la misma palabra dos veces
                if (a.I != b.I)
                    a.I = b.I;
                if (a.J != b.J)
                    a.J = b.J;

                return true;
            }//if
            return false;
        }

        public static bool operator !=(Palabra a, Palabra b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            Palabra b = (Palabra)obj;
            return (this == b);
        }

        public override int GetHashCode()
        {
            return Texto.GetHashCode();
        }
    }
}
