using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class FuelPriceController : ApiController
    {
        DataClassesDataContext dc = new DataClassesDataContext();

        // GET: api/FuelPrice
        public List<FuelPrice> Get()
        {
            var lista = from FuelPrice in dc.FuelPrices select FuelPrice;
            return lista.ToList();
        }

        // GET: api/FuelPrice/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/FuelPrice
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/FuelPrice/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/FuelPrice/5
        public void Delete(int id)
        {
        }
    }
}
