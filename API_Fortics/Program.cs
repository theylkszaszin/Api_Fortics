using API_Fortics.Repo;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace API_Fortics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Inicializando autenticação");
            ApiHelper.InitializeClient();

            RunAsync().GetAwaiter().GetResult();

            Console.WriteLine("\nFinalizado!");
            Console.WriteLine("=============================== F I M  ========================");
            Console.ReadKey();
        }

        static async Task RunAsync()
        {            
            try
            {
                Contato contato = new Contato {
                    name = "Felipe José Cardoso de Sousa",
                    ddi = "55",
                    final_number = "18981196588",
                    BENEF_DATANASC = "08/05/2000",
                    BENEF_CARTEIRINHA = "00440526001085016",
                    BENEF_NOME = "Felipe José Cardoso de Sousa",
                    BENEF_TARGET = "S",
                    default_language = "pt-BR",
                };

                ContatoResult contatoresult = new ContatoResult();
                contatoresult = await ContatoProcessor.GetContactAsync();
                bool contactexist = false;
                foreach (var item in contatoresult.data)
                {
                    if (item.platforms?.Find(x => x.platform?.ToUpper() == "WhatsappBusiness".ToUpper())?.platform_id == $"{contato.ddi}{contato.final_number}")
                    {
                        contato._id = ContatoProcessor.GetContactAsync(item.name).GetAwaiter().GetResult().data[0]._id ;
                        contato = await Updatecontato(contato);
                        contactexist = true;
                        break;
                    }
                }
                if (!contactexist)
                {
                    contato._id = ContatoProcessor.CreateContactAsync(contato).GetAwaiter().GetResult()._id ;
                    Console.WriteLine($"Created at {contato.name} - {contato._id}");
                    contato = await Updatecontato(contato);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static async Task<Contato> Updatecontato(Contato contato)
        {
            //if (contato.channels == null)
            //    contato.channels = new Channels { WhatsappBusiness = $"{contato.ddi}{contato.final_number}"};
            if (contato.platforms == null)
                contato.platforms = new List<PlatformModel>();

            contato.platforms.Add(new PlatformModel { platform_id = $"{contato.ddi}{contato.final_number}", platform = "WhatsappBusiness" });

            return await ContatoProcessor.UpdateContactAsync(contato);
        }
    }
}
