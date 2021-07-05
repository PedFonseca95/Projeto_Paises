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

            var path = @"Data\PrecoCombustiveis.sqlite";

            try
            {
                _connection = new SQLiteConnection("Data Source =" + path);
                _connection.Open();

                string sqlcommand = "create table if not exists PrecoCombustiveis " +
                    "(NomePais varchar(100)," +
                    "PrecoGasolina varchar(10)," +
                    "PrecoGasoleo varchar(10))";

                _command = new SQLiteCommand(sqlcommand, _connection);

                _command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _dialogService.ShowMessage("Erro", e.Message);
            }
        }

        public async Task SaveData(List<Fuel> Combustiveis, IProgress<int> progress)
        {
            int counter = 250;

            try
            {
                foreach (var combustivel in Combustiveis)
                {
                    string sql = string.Format("insert into PrecoCombustiveis " +
                        "(NomePais, PrecoGasolina, PrecoGasoleo) values ('{0}', '{1}', '{2}')", 
                        combustivel.NomePais.Replace("'","''"), combustivel.PrecoGasolina, combustivel.PrecoGasoleo);

                    counter++;
                    _command = new SQLiteCommand(sql, _connection);

                    progress.Report(counter);
                    await _command.ExecuteNonQueryAsync();
                }

                _connection.Close();
            }
            catch (Exception e)
            {
                _dialogService.ShowMessage("Erro", e.Message);
            }
        }

        public List<Fuel> GetData()
        {
            List<Fuel> combustiveis = new List<Fuel>();

            try
            {
                string sql = "select * from PrecoCombustiveis";

                _command = new SQLiteCommand(sql, _connection);

                SQLiteDataReader reader = _command.ExecuteReader();

                while (reader.Read())
                {
                    combustiveis.Add(new Fuel
                    {
                        NomePais = (string)reader["NomePais"],
                        PrecoGasolina = (string)reader["PrecoGasolina"],
                        PrecoGasoleo = (string)reader["PrecoGasoleo"]
                    });
                }

                _connection.Close();

                return combustiveis;
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
                string sql = "delete from PrecoCombustiveis";

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
