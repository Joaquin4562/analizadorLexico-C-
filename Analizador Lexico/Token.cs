using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizador_Lexico
{
    class Token
    {
        String patron;
        String token_name;
        bool ignore;
        public Token(String patron, String token_name, bool ignore)
        {
            this.token_name = token_name;
            this.patron = patron;
            this.ignore = ignore;
        }
        public String getPatron()
        {
            return patron;
        }
        public String getToken_name()
        {
            return token_name;
        }

        public bool getIgnore()
        {
            return ignore;
        }
      
    }
}
