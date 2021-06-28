using Projeto_Paises.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Paises.Servicos
{
    public class NetworkService
    {
        // Vai verificar que existe conexão à internet para depois fazer ligação diretamente à API ou à base de dados local

        public Response CheckConnection() // Método que vai server para verificar se existe conexão à internet
        {
            var client = new WebClient(); // Variavel que vamos utilizar para testar a ligação à internet

            try // Vai tentar estabelecer uma conexão à internet
            {
                using (client.OpenRead("http://clients3.google.com/generate_204")) // Se a conexão for feita com sucesso
                {
                    return new Response
                    {
                        IsSuccess = true, // Retorna uma nova resposta com valor true na propriedade IsSuccess                    
                    };
                }
            }
            catch // Caso algo corra mal/não seja estabelecida a ligação à internet
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Verifique a sua ligação à Internet"
                };
            }
        }
    }
}
