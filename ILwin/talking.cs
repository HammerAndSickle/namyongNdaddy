using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILwin
{
    interface talking
    {
        void startmove();
        void startjump();
        void sayHello();
        void sayTime();
        void sayKeyword(Balloon balloon);
        void sayQuick(int level);
    }
}
