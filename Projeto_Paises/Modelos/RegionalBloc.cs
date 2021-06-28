using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Paises.Modelos
{
    public class RegionalBloc
    {
        public string Acronym { get; set; }
        public string Name { get; set; }
        public List<object> OtherAcronyms { get; set; }
        public List<object> OtherNames { get; set; }
    }
}
