using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projeto_Paises.Servicos
{
    public class DialogService
    {
        // Serve para apresentar as mensagens de erro

        public void ShowMessage(string title, string message)
        {
            MessageBox.Show(message, title);
        }
    }
}
