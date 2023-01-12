using OpenRSS_1.Models;
using OpenRSS_1.Services;
using System.Diagnostics;
using System.Security.Policy;

namespace OpenRSS_1
{
    public partial class Form1 : Form
    {
        private Task<string> text;

        public Form1()
        {
            InitializeComponent();
            _ = populateListViewAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async Task populateListViewAsync()
        {
            List<Subscriptions> subscriptions = await DatabaseService.GetActiveFeeds();
            foreach (Subscriptions subscription in subscriptions)
            {
                ListViewItem listViewItem = new ListViewItem();
                string name = subscription.Name;
                if (name.Length > 40)
                {
                    name = name.Substring(0, 40) + "...";
                }
                listViewItem.Text = name;
                listViewItem.Tag = subscription.Id.ToString();
                listView1.Items.Add(listViewItem);
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UrlInput urlInput = new UrlInput();
            DialogResult dialogresult = urlInput.ShowDialog();
            if (dialogresult == DialogResult.OK)
            {
                string url = urlInput.textBox1.Text;
                Debug.WriteLine(url);

                //run query and get name of RSS feed 
                ParseXml parseXml= new ParseXml();
                string name = parseXml.feedName(url);
                Subscriptions   sub = new Subscriptions();
                sub.Url = url;
                sub.Error = false;
                sub.Name = name;
                sub.DateSubscribed = DateTime.Now;
                sub.LastChecked= DateTime.Now;
                sub.LastUpdated= DateTime.Now;
                sub.Active = true;
                await DatabaseService.AddNewSubscription(sub);
            }
            else { 

            }
            urlInput.Dispose();
        }

        private async void clearDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await DatabaseService.DropDataTables();
        }

        private async void queryDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParseXml parseXml = new ParseXml();
            HttpService httpService= new HttpService();
            List<Subscriptions> subscriptions =  await DatabaseService.GetActiveFeeds();
            string htmlString = "";
            foreach (Subscriptions sub in subscriptions)
            {
                Debug.WriteLine(sub.Url);
                httpService.url = sub.Url;
                text = httpService.ConnectFeed();
                // await text;
                string v = await text;
                parseXml.XmlString = v;
                htmlString += parseXml.parseXmlString();
            }
            await webView21.EnsureCoreWebView2Async();
            webView21.NavigateToString(htmlString);
        }

        private async void listView1_Click(object sender, EventArgs e)
        {
            try {
                string id = (string)listView1.SelectedItems[0].Tag;
                var subscription = await DatabaseService.getSubscriptionInfoAsync(id);
                if (subscription != null)
                {
                    Debug.WriteLine(subscription.Url);
                    getFeed(subscription.Url);
                }
         
                
            } catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
         //   Debug.WriteLine(sender.ToString() + " --- " + e);
        }

        private async void getFeed(string url)
        {
            ParseXml parseXml = new ParseXml();
            HttpService httpService = new HttpService();
            string htmlString = "";            
            httpService.url = url;
            text = httpService.ConnectFeed();
            // await text;
            string v = await text;
            parseXml.XmlString = v;
            htmlString += parseXml.newParseXml(url);
            await webView21.EnsureCoreWebView2Async();
            webView21.NavigateToString(htmlString);
          
            //parseXml.newParseXml(url);


        }

        private void refreshListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            _ = populateListViewAsync();
        }



    }
}