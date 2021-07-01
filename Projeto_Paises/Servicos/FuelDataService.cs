using Projeto_Paises.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI;

namespace Projeto_Paises.Servicos
{
    public class FuelDataService
    {
        // Base de dados local

        private SQLiteConnection _connection;

        private SQLiteCommand _command;

        private DialogService _dialogService;

        public FuelDataService()
        {
            _dialogService = new DialogService();

            if (!Directory.Exists("Data")) // Se não existir uma pasta criada (onde vai estar a nossa base de dados local)
            {
                Directory.CreateDirectory("Data");
            }

            var path = @"Data\FuelPrices.sqlite";

            try
            {
                _connection = new SQLiteConnection("Data Source =" + path);
                _connection.Open();

                string sqlcommand = "create table if not exists FuelPrices " +
                    "(CountryName varchar(100)," +
                    "FuelPrice varchar(100))";

                _command = new SQLiteCommand(sqlcommand, _connection);

                _command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _dialogService.ShowMessage("Erro", e.Message);
            }
        }

        public void SaveData(List<Fuel> Combustiveis)
        {
            try
            {
                foreach (var combustivel in Combustiveis)
                {
                    string sql = string.Format("insert into FuelPrices " +
                        "(CountryName, FuelPrice) values ('{0}', '{1}')", combustivel.NomePais.Replace("'","''"), combustivel.PrecoCombustivel);

                    _command = new SQLiteCommand(sql, _connection);

                    _command.ExecuteNonQuery();
                }

                _connection.Close();
            }
            catch (Exception e)
            {
                _dialogService.ShowMessage("Erro", e.Message);
            }
        }

        public List<FuelPrice> GetData()
        {
            List<FuelPrice> paises = new List<FuelPrice>();

            try
            {
                string sql = "select * from FuelPrices";

                _command = new SQLiteCommand(sql, _connection);

                SQLiteDataReader reader = _command.ExecuteReader();

                while (reader.Read())
                {
                    paises.Add(new FuelPrice
                    {
                        nomePais = (string)reader["CountryName"],
                        precoCombustivel = (string)reader["FuelPrice"]
                    });
                }

                _connection.Close();

                return paises;
            }
            catch (Exception e)
            {
                _dialogService.ShowMessage("Erro", e.Message);
                return null;
            }
        }

        public void DeleteData()
        {
            try
            {
                string sql = "delete from FuelPrices";

                _command = new SQLiteCommand(sql, _connection);

                _command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _dialogService.ShowMessage("Erro", e.Message);
            }
        }
    }
}
