using Projeto_Paises.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Paises.Servicos
{
    public class DataService
    {
        // Base de dados local

        private SQLiteConnection _connection;

        private SQLiteCommand _command;

        private DialogService _dialogService;

        public DataService()
        {
            _dialogService = new DialogService();

            if (!Directory.Exists("Data")) // Se não existir uma pasta criada (onde vai estar a nossa base de dados local)
            {
                Directory.CreateDirectory("Data");
            }

            var path = @"Data\Paises.sqlite";

            try
            {
                _connection = new SQLiteConnection("Data Source =" + path);
                _connection.Open();

                string sqlcommand = "create table if not exists Paises " +
                    "(Name varchar(100), " +
                    "Capital varchar(100), " +
                    "Region varchar(100), " +
                    "Subregion varchar(100), " +
                    "Population int, " +
                    "Gini varchar(100), " +
                    "Flag varchar(100))";

                _command = new SQLiteCommand(sqlcommand, _connection);

                _command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _dialogService.ShowMessage("Erro", e.Message);
            }
        }

        public async Task SaveData(List<Pais> Paises, IProgress<int> progress)
        {
            int counter = 0;

            try
            {
                foreach (var pais in Paises)
                {
                    string sql = string.Format("insert into Paises " +
                        "(Name, Capital, Region, Subregion, Population, Gini, Flag) " +
                        "values ('{0}', '{1}', '{2}', '{3}', {4}, '{5}', '{6}')", 
                        pais.Name.Replace("'","''"), pais.Capital.Replace("'", "''"), pais.Region.Replace("'", "''"), pais.Subregion.Replace("'", "''"), pais.Population, pais.Gini, pais.Flag);

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

        public List<Pais> GetData()
        {
            List<Pais> paises = new List<Pais>();

            try
            {
                string sql = "select * from Paises";

                _command = new SQLiteCommand(sql, _connection);

                SQLiteDataReader reader = _command.ExecuteReader();

                while (reader.Read())
                {
                    paises.Add(new Pais
                    {
                        Name = (string)reader["Name"],
                        Capital = (string)reader["Capital"],
                        Region = (string)reader["Region"],
                        Subregion = (string)reader["Subregion"],
                        Population = (int)reader["Population"],
                        Gini = (string)reader["Gini"],
                        Flag = (string)reader["Flag"]
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
                string sql = "delete from Paises";

                _command = new SQLiteCommand(sql, _connection);

                _command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _dialogService.ShowMessage("Erro", e.Message);
            }



        }

        internal void SaveData(List<Fuel> fuels)
        {
            throw new NotImplementedException();
        }
    }
}
