using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Ncov_BLL
{
    public sealed class GetData
    {
        #region Singleton
        private static GetData instance = null;

        public static GetData Instance
        {
            get
            {
                if (instance == null)
                    instance = new GetData();
                return instance;
            }
            private set { GetData.instance = value; }
        }
        private GetData() { }
        #endregion

        #region Fields
        private Dictionary<string, string> codeCountries = new Dictionary<string, string>()
        {
            {"Việt Nam", "VN"},
            {"Thụy Điển", "SE"},
            {"Vương quốc Anh", "GB"},
            {"Bra-xin", "BR"},
            {"Hoa Kỳ", "US"},
            {"Pháp", "FR"},
            {"Đan Mạch", "DK"},
            {"Cộng hòa Nam Phi", "ZA"},
            {"Canada", "CA"},
            {"Đức", "DE"},
            {"Lát-vi-a", "LV"},
            {"Cộng hòa Séc", "CZ"},
            {"Ai-xơ-len", "IS"},
            {"Trung Quốc", "CN"}
        };
        public Dictionary<string, string> CodeCountries 
        {   get => codeCountries; 
            private set => codeCountries = value; 
        }

        #endregion

        #region Methods
        public string GetData_FromWeb(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.AutomaticDecompression = DecompressionMethods.GZip;

            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());
            string dataString = reader.ReadToEnd();

            reader.Close();
            response.Close();

            return dataString;
        }

        public T ConvertJson_ToClass<T>(string dataString)
        {
            string data = GetData_FromWeb(dataString);

            T dataConverted = JsonConvert.DeserializeObject<T>(data);

            return dataConverted;
        }
        #endregion
    }
}
