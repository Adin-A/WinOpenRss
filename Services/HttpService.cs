using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OpenRSS_1.Services
{
    public class HttpService
    {
        private HttpClient client = new HttpClient();
        public int feedId;
        public string? url { get; set; }

        public async Task<string> ConnectFeed()
        {
            if (url == null) return "";
            
            Uri uri = new Uri(url);
            var result = await client.GetAsync(uri);
            Debug.WriteLine(result.StatusCode);
            string resultContent = await result.Content.ReadAsStringAsync();
            return resultContent;            
        }
    }

}
