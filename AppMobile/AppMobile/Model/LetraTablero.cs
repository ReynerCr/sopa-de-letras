using System;
using System.Collections.Generic;
using System.Text;

namespace AppMobile.Model
{
    class LetraTablero : ToggleButton
    {
        public int I
        {
            get;
            private set;
        }

        public int J
        {
            get;
            private set;
        }

        public LetraTablero(int i, int j, char t) : base()
        {
            I = i;
            J = j;
            Text = t.ToString();
        }
    }
}
