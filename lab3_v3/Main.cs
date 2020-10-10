using System;
using System.Windows.Forms;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace lab3_v3
{
    public partial class main : Form
    {
        private const string apirequest = "https://api.vk.com/method/";
        private static string authtoken = String.Empty;
        private static string server_response = String.Empty;
        private void Get(string uri) { server_response = new WebClient { Encoding = Encoding.UTF8 }.DownloadString(uri); }
        private void GetProfileInfo() 
        {
            Get($"{apirequest}account.getProfileInfo?access_token={authtoken}&v=5.103");
            dynamic decodedresponse = JObject.Parse(server_response);
            var decoded = decodedresponse.response;
            profileinfo.Text += $"{ decoded.first_name } {decoded.last_name}{Environment.NewLine}Status: {decoded.status}{Environment.NewLine}Birthday: {decoded.bdate}{Environment.NewLine}Screen name (link): {decoded.screen_name}";
        }
        private void GetFriends()
        {
            Get($"{apirequest}friends.get?order=name&count=5000&fields=domain&access_token={authtoken}&v=5.103");
            dynamic friendslist = JObject.Parse(server_response);
            var friendsl = friendslist.response;
            for (int i = 0; i < Convert.ToInt32(friendsl.count); i++) friends.Items.Add($"{friendsl.items[i].first_name} {friendsl.items[i].last_name}");
        }
        private void LogInVK() { using (auth authForm = new auth()) { if (authForm.ShowDialog() == DialogResult.Yes) authtoken = authForm.Token; GetProfileInfo(); GetFriends(); } }
        private void SetScreenName(object sender, EventArgs e) { Get($"{apirequest}account.saveProfileInfo?screen_name={screenname.Text}&access_token={authtoken}&v=5.103"); }
        public main()
        {
            InitializeComponent();
            LogInVK();
        }
    }
}
