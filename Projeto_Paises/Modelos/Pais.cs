using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Paises.Modelos
{
    public class Pais
    {
        // * -> Propriedades utilizadas no projeto

        public string Name { get; set; } // *        
        public string Capital { get; set; } // *
        public string Region { get; set; } // *
        public string Subregion { get; set; } // *
        public int Population { get; set; } // *
        public string Gini { get; set; } // *
        //public List<Currency> Currencies { get; set; } // *
        //public List<Language> Languages { get; set; } // *
        //public Translations Translations { get; set; } // *
        public string Flag { get; set; } // *
    }
}
