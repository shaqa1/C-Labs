using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace lab3_v2
{
    public partial class main : Form
    {
        private const string apirequest = "https://api.vk.com/method/";
        private string apiuri = "https://api.vk.com/method/";
        private static string authtoken = String.Empty;
        private void webrequest()
        {
            var client = new WebClient { Encoding = Encoding.UTF8 };
            string json = client.DownloadString(apiuri);
            Debug.WriteLine(json);
            apiuri = "https://api.vk.com/method/";
        }
        private void setstatus_Click(object sender, EventArgs e)
        {
            apiuri = $"{apirequest}status.set?text={setstatus.Text}&access_token={authtoken}&v=5.103";
            webrequest();
        }
        private void passchangeclick(object sender, EventArgs e)
        {
            apiuri = $"{apirequest}account.changePassword?old_password={oldpass.Text}&new_password={newpass.Text}&access_token={authtoken}&v=5.103";
            webrequest();
        }
        private void bdaychangeclick(object sender, EventArgs e)
        {
            apiuri = $"{apirequest}account.saveProfileInfo?bdate={bdate.Text}&access_token={authtoken}&v=5.103";
            webrequest();
        }
        private void authclick(object sender, EventArgs e)
        {
            using (auth authForm = new auth())
            {
                if (authForm.ShowDialog() == DialogResult.Yes) authtoken = authForm.Token;
            }
            Debug.WriteLine(authtoken);
        }
        public main()
        {
            InitializeComponent();
        }
    }
}
