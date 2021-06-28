using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Paises.Modelos
{
    public class Response
    {
        // Vai ficar responsável pela resposta

        public bool IsSuccess { get; set; } // Para verificar se tudo correu bem

        public string Message { get; set; } // Caso algo corra mal, mostra a mensagem mensagem da API

        public object Result { get; set; } // Caso corra tudo bem, vai guardar as informações num object Result
    }
}
