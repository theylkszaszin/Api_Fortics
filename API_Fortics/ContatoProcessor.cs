using API_Fortics.Repo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API_Fortics
{
    public class ContatoProcessor
    {
        public static void ShowContact(Contato contact)
        {
            Console.WriteLine($"Name: {contact.name}\tnumber: " +
                $"{contact.number}\tdataNasc: {contact.BENEF_DATANASC}");
        }

        public static void ShowContact(ContatoResult contatores)
        {
            foreach (var item in contatores.data)
            {
                Console.WriteLine($"Name: {item.name}\tnumber: " +
                    $"{item.number}\tdataNasc: {item.BENEF_DATANASC}\n");

            }
        }

        public static async Task<Contato> CreateContactAsync(Contato contact)
        {
            // ----  MODEL ----
            //  {
            //        "name": "Felipe Jose Cardoso de Sousa",
            //        "ddi": "55",
            //        "final_number": "18980000000",
            //        "channels": {
            //                     "Whatsapp": "5518980000000"
            //      }
            //  }

            var datacontact = new { ddi = contact.ddi, final_number = contact.final_number, name = contact.name, channels = new Channels() { WhatsappBusiness = $"{contact.ddi}{contact.final_number}"} };
            using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsJsonAsync("contacts", datacontact))
            {
                response.EnsureSuccessStatusCode();
                contact = JsonConvert.DeserializeObject<Contato>(await response.Content.ReadAsStringAsync());
                return contact;
            }
        }

        public static async Task<ContatoResult> GetContactAsync()
        {

            ContatoResult ContatoResult = null;
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(ApiHelper.ApiClient.BaseAddress + "contacts"))
            {
                ContatoResult = await response.Content.ReadAsAsync<ContatoResult>();
            } 
            return ContatoResult;
        }

        public static async Task<ContatoResult> GetContactAsync(string name)
        {
            ContatoResult ContatoResult = null;
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(ApiHelper.ApiClient.BaseAddress + $"contacts/search?name={name}"))
            {
                ContatoResult = await response.Content.ReadAsAsync<ContatoResult>();
            }
            return ContatoResult;
        }

        public static async Task<Contato> UpdateContactAsync(Contato contact)
        {
            var json = JsonConvert.SerializeObject(contact,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            contact = JsonConvert.DeserializeObject<Contato>(json);
            using (HttpResponseMessage response = await ApiHelper.ApiClient.PutAsJsonAsync($"contacts/{contact._id}/", contact)) 
            {
                response.EnsureSuccessStatusCode();
                // Deserialize the updated product from the response body.
                contact = await response.Content.ReadAsAsync<Contato>();
                return contact;
            }
            
        }

        public static async Task<HttpStatusCode> DeleteContactAsync(string id)
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.DeleteAsync($"contacts/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return response.StatusCode;
                }
                return response.StatusCode;
            }
        }
    }
}
