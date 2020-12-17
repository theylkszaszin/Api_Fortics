using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace API_Fortics.Repo
{
    public class Contato
    {
        public string _id { get; set; }
        public string name { get; set; }

        //public string email { get; set; }
        public List<PlatformModel> platforms { get; set; }
        //public string[] groups { get; set; }
        public Channels channels { get; set; }
        public string ddi { get; set; }
        public string final_number { get; set; }
        public string BENEF_NOME { get; set; }
        public string number => $"{ddi}{final_number}";
        public string BENEF_DATANASC { get; set; }
        public string BENEF_CARTEIRINHA { get; set; }
        public string BENEF_TARGET { get; set; } //S OU N
        public string default_language { get; set; }
                
    }
}
