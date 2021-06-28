using Projeto_Paises.Modelos;
using Projeto_Paises.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projeto_Paises
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Atributos

        private NetworkService _networkService;

        private ApiService _apiService;

        private List<Pais> _paises;

        private DialogService _dialogService;

        private DataService _dataService;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            _networkService = new NetworkService();
            _apiService = new ApiService();
            _dialogService = new DialogService();
            _dataService = new DataService();
            LoadPaises();
        }

        #region Eventos

        /// <summary>
        /// Mostra informações sobre o autor do projeto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_sobre_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Formando: Pedro Ricardo Pereira Fonseca" +
                "\nTurma: CET57" +
                "\nFormador: Rafael Santos" +
                "\nData: 27/06/2021" +
                "\nVersão: 1.0.0"
                ,"Sobre", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega uma lista de paises utilizando uma API ou base de dados local caso não exista ligação à internet
        /// </summary>
        private async void LoadPaises()
        {
            bool load; // Para saber se foi carregado ou não

            lbl_estado.Content = "Estado: \nA atualizar lista de países";

            var connection = _networkService.CheckConnection(); // Vai testar a conexão à internet

            if (!connection.IsSuccess) // Se a conexão não tiver sido feita com sucesso
            {
                LoadLocalPaises(); // Conecta-se à base de dados local
                load = false;
            }
            else // Se tiver conexão
            {
                await LoadApiPaises(); // Conecta-se à Api que vai estabelecer ligação à base de dados online
                load = true;
            }

            if (_paises.Count == 0) // Se a lista de paises não tiver sido carregada ou estiver vazia
            {
                lbl_estado.Content = "Estado: \nUtilizar ligação à internet no primeiro uso!";
                lbl_origem.Content = "Origem dos dados: \nSem dados";
                return; // Termina a execução do método LoadRates()
            }

            foreach (Pais pais in _paises)
            {
                lb_paises.Items.Add(pais); // Apresentar os valores da lista criada na listbox
                lb_paises.DisplayMemberPath = "Name";
            }

            if (load) // Se a Api carregar
            {
                lbl_estado.Content = string.Format("Estado: \nDados carregados às {0:F}", DateTime.Now.ToLongTimeString());
                lbl_origem.Content = "Origem:\nhttp://restcountries.eu/rest/v2/all";
            }
            else // Se for carregado através da base de dados local
            {
                lbl_estado.Content = string.Format("Estado: \nDados carregados às {0:F}", DateTime.Now.ToLongTimeString());
                lbl_origem.Content = string.Format("Origem: \nBase de dados local.");
            }

            pbar_load.Value = 100; // Progressbar fica a 100%
        }

        private void LoadLocalPaises()
        {
            _paises = _dataService.GetData();
        }

        private async Task LoadApiPaises()
        {
            pbar_load.Value = 0;

            // Definir endereço base/principal e controlador da API
            Response response = await _apiService.GetPaises("http://restcountries.eu", "/rest/v2/all");
            // async e await serve para que a aplicação continue a correr enquanto são carregadas as taxas - Tarefa asincrona  

            _paises = (List<Pais>)response.Result;

            _dataService.DeleteData();

            _dataService.SaveData(_paises);
        }

        #endregion

        private void lb_paises_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
