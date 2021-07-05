﻿using Projeto_Paises.Modelos;
using Projeto_Paises.Servicos;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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

        private Pais _pais;

        private DataService _dataService;

        private DialogService _dialogService;

        private FuelApiService _fuelApiService;

        private FuelDataService _fuelDataService;

        private List<Fuel> _fuels;

        #endregion

        #region Construtor

        public MainWindow()
        {
            InitializeComponent();
            _networkService = new NetworkService();
            _apiService = new ApiService();
            _dataService = new DataService();
            _dialogService = new DialogService();
            _fuelApiService = new FuelApiService();
            _fuelDataService = new FuelDataService();
            LoadPaises();


        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que mostra informações sobre o autor do projeto
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
                , "Sobre", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Evento que carrega as informações do pais escolhido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lb_paises_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lb_informacoes.Items.Clear();

            _pais = (Pais)lb_paises.SelectedItem;

            // Apresentar as informações do pais
            lb_informacoes.Items.Add("Informações Principais");

            // Nome
            lb_informacoes.Items.Add("Nome: " + _pais.Name);

            // Capital
            if (_pais.Capital == "" || _pais.Capital == null)
            {

                lb_informacoes.Items.Add("Capital: Informação indisponivel");
            }
            else
            {
                lb_informacoes.Items.Add("Capital: " + _pais.Capital);
            }

            // Região
            if (_pais.Region == "" || _pais.Region == null)
            {
                lb_informacoes.Items.Add("Região: Informação indisponivel");
            }
            else
            {
                lb_informacoes.Items.Add("Região: " + _pais.Region);
            }

            // Subregião
            if (_pais.Subregion == "" || _pais.Subregion == null)
            {
                lb_informacoes.Items.Add("Subregião: Informação indisponivel");
            }
            else
            {
                lb_informacoes.Items.Add("Subregião: " + _pais.Subregion);
            }

            // População
            if (_pais.Population.ToString() == "" || _pais.Population.ToString() == null)
            {
                lb_informacoes.Items.Add("População: Informação indisponivel");
            }
            else
            {
                lb_informacoes.Items.Add("População: " + _pais.Population.ToString());
            }

            // Gini
            if (_pais.Gini == "" || _pais.Gini == null)
            {
                lb_informacoes.Items.Add("Gini: Informação indisponivel");
            }
            else
            {
                lb_informacoes.Items.Add("Gini: " + _pais.Gini);
            }

            string path = Assembly.GetExecutingAssembly().Location.Remove(Assembly.GetExecutingAssembly().Location.Length - 18);
            var imgPNG = $"Bandeiras/{_pais.Name}.png";

            // Bandeira
            if (File.Exists(imgPNG))
            {
                var caminho = path + imgPNG;

                var uriSource = new Uri(caminho);

                img_bandeira.Source = new BitmapImage(uriSource);
            }
            else
            {
                var caminho = Assembly.GetExecutingAssembly().Location.Remove(Assembly.GetExecutingAssembly().Location.Length - 43) + "Resources/bandeira.png";
                var uriSource = new Uri(caminho);
                img_bandeira.Source = new BitmapImage(uriSource);
            }

            // API - Preço dos combustiveis por pais
            lb_informacoes.Items.Add("");
            lb_informacoes.Items.Add("Informações da API criada - Preço dos combustiveis");

            foreach (Fuel fuel in _fuels)
            {
                if (fuel.NomePais == _pais.Name)
                {
                    if (fuel.PrecoGasolina == null || fuel.PrecoGasolina == "")
                    {
                        lb_informacoes.Items.Add("Preço da gasolina: Informação indisponivel");
                    }
                    else
                    {
                        lb_informacoes.Items.Add("Preço da gasolina: " + fuel.PrecoGasolina + " €/litro");
                    }

                    if (fuel.PrecoGasoleo == null || fuel.PrecoGasoleo == "")
                    {
                        lb_informacoes.Items.Add("Preço do gasoleo: Informação indisponivel");
                    }
                    else
                    {
                        lb_informacoes.Items.Add("Preço do gasoleo: " + fuel.PrecoGasoleo + " €/litro");
                    }
                }
            }

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
                pbar_load.Value = 0;
                await Task.Run(() => LoadLocalPaises()); // Conecta-se à base de dados local
                await Task.Run(() => LoadLocalCombustiveis());
                load = false;
                pbar_load.Value = 750;
            }
            else // Se tiver conexão
            {
                await LoadApiPaises(); // Conecta-se à Api que vai estabelecer ligação à base de dados online
                await LoadApiFuelPrices();

                var progress = new Progress<int>(x => pbar_load.Value = x);
                await Task.Run(()=>DownloadBandeiras(_paises,progress));
                load = true;
            }

            if (_paises.Count == 0) // Se a lista de paises não tiver sido carregada ou estiver vazia
            {
                lbl_estado.Content = "Estado: \nUtilizar ligação à internet no primeiro uso!";
                lbl_origem.Content = "Origem dos dados: \nSem dados";
                return; // Termina a execução do método LoadPaises()
            }

            foreach (Pais pais in _paises)
            {
                lb_paises.Items.Add(pais); // Apresentar os valores da lista criada na listbox
                lb_paises.DisplayMemberPath = "Name";
            }

            if (load) // Se a Api carregar
            {
                lbl_estado.Content = string.Format("Estado: \nDados carregados às {0:F}", DateTime.Now.ToLongTimeString());
                lbl_origem.Content = "Origem:\nhttp://restcountries.eu/rest/v2/all\nhttp://fuelprices.somee.com/api/FuelPrice";
            }
            else // Se for carregado através da base de dados local
            {
                lbl_estado.Content = string.Format("Estado: \nDados carregados às {0:F}", DateTime.Now.ToLongTimeString());
                lbl_origem.Content = string.Format("Origem: \nBase de dados local.");
            }
        }

        private void LoadLocalCombustiveis()
        {
            _fuels = _fuelDataService.GetData();
        }

        /// <summary>
        /// Vai buscar as informações à base de dados local
        /// </summary>
        private void LoadLocalPaises()
        {
            _paises = _dataService.GetData();
        }

        /// <summary>
        /// Vai buscar as informações à API
        /// </summary>
        /// <returns></returns>
        private async Task LoadApiPaises()
        {
            pbar_load.Value = 0;

            var progress = new Progress<int>(x => pbar_load.Value = x);

            // Definir endereço base/principal e controlador da API
            Response response = await _apiService.GetPaises("http://restcountries.eu", "/rest/v2/all");
            // async e await serve para que a aplicação continue a correr enquanto são carregadas as taxas - Tarefa asincrona  

            _paises = (List<Pais>)response.Result;

            _dataService.DeleteData();

            await Task.Run(() => _dataService.SaveData(_paises, progress));
        }

        /// <summary>
        /// Vai buscar as informações da API criada
        /// </summary>
        /// <returns></returns>
        private async Task LoadApiFuelPrices()
        {
            var progress = new Progress<int>(x => pbar_load.Value = x);

            // Definir endereço base/principal e controlador da API
            Response response = await _fuelApiService.GetFuels("http://fuelprices.somee.com", "/api/FuelPrice");
            // async e await serve para que a aplicação continue a correr enquanto são carregadas as taxas - Tarefa asincrona

            _fuels = (List<Fuel>)response.Result;

            _fuelDataService.DeleteData();

            await Task.Run(() => _fuelDataService.SaveData(_fuels, progress));
        }

        /// <summary>
        /// Faz download da imagem da bandeira do pais escolhido caso tenha
        /// </summary>
        /// <param name="pais"></param>
        private void DownloadBandeiras(List<Pais> paises, IProgress<int> progress)
        {
            int counter = 500;

            foreach (Pais pais in paises)
            {
                string svgFileName = pais.Flag;

                using (WebClient webClient = new WebClient())
                {
                    string path = Assembly.GetExecutingAssembly().Location.Remove(Assembly.GetExecutingAssembly().Location.Length - 18);
                    var imgSVG = $"Bandeiras/{pais.Name}.svg";
                    var imgPNG = $"Bandeiras/{pais.Name}.png";

                    if (!Directory.Exists(path + "Bandeiras"))
                    {
                        Directory.CreateDirectory(path + "Bandeiras");
                    }

                    try
                    {
                        // Se a bandeira ainda não tiver sido transferida
                        if (!File.Exists(imgSVG))
                        {
                            // Transfere em formato svg
                            webClient.DownloadFile(svgFileName, imgSVG);

                            // Faz a conversao -> bytes -> png
                            var byteArray = Encoding.ASCII.GetBytes(imgSVG);

                            using (var stream = new MemoryStream(byteArray))
                            {
                                var svgDocument = SvgDocument.Open(imgSVG);

                                var bitmap = svgDocument.Draw();

                                bitmap.Save(imgPNG, ImageFormat.Png);
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }

                    counter++;
                    progress.Report(counter);
                }
            }
        }

        #endregion
    }
}
