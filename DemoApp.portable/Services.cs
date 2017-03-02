using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
//Nuevo
using System.Net.Http;
using Newtonsoft.Json;

namespace DemoApp.portable
{
    public class Services
    {
        HttpClient http = new HttpClient();
        Persona persona = new Persona();
        List<Persona> listPersonas = new List<Persona>();

        const string url = "http://webapirestdemoblog.azurewebsites.net/api/Personas";

        public async Task<List<Persona>> GetPersonas()
        {

            var response = await http.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {

                var content = await response.Content.ReadAsStringAsync();
                listPersonas = JsonConvert.DeserializeObject<List<Persona>>(content);

            }

            return listPersonas;

        }

        public async Task<Persona> GetPersonasId(int id)
        {

            var getUrl = url + $"/{id}";

            var response = await http.GetAsync(getUrl);

            if (response.IsSuccessStatusCode)
            {

                var content = await response.Content.ReadAsStringAsync();
                persona = JsonConvert.DeserializeObject<Persona>(content);
                return persona;

            }
            else
            {
                return null;
            }


        }

        public async Task<Persona> DeletePersona(int id)
        {

            var deleteUrl = url + $"/{id}";

            var response = http.DeleteAsync(deleteUrl);

            var content = await response.Result.Content.ReadAsStringAsync();

            if (response.IsCompleted)
            {

                persona = JsonConvert.DeserializeObject<Persona>(content);
                return persona;

            }
            else
            {
                return null;
            }


        }

        public async Task<String> PostPersona(Persona per)
        {

            var serializer = JsonConvert.SerializeObject(per);

            var content = new StringContent(serializer, Encoding.UTF8, "application/json");

            var response = await http.PostAsync(url, content);

            string msg = (response.IsSuccessStatusCode) ? "Registrado" : "Error";

            return msg;

        }



    }
}
