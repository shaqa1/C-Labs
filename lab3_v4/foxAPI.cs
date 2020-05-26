using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;
using DotNetBrowser.Engine;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace lab3_v4
{
    public partial class fox : MetroForm
    {
        internal static IEngine engine = EngineFactory.Create(new EngineOptions.Builder { LicenseKey = "1BNKDJZJSCZMPTZKWHVDJ65Q3QP48T2PF7SGZB75RJ21CDIAISZO9EP7NUVTSP9CG602LH" }.Build());
        public delegate void OAuthDelegate(string accesstoken);
        internal const string APIversion = "5.103";
        private const string APIlink = "https://api.vk.com/method/";
        private string GetRequestURI = "https://api.vk.com/method/";
        private string GetRequestParams = String.Empty;
        private static string Token = String.Empty;
        private static string JSON = String.Empty;
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (WindowState == FormWindowState.Minimized) { Hide(); Tray_Icon.BalloonTipTitle = "Hey, I'm here!"; Tray_Icon.BalloonTipText = "Click here to restore"; Tray_Icon.ShowBalloonTip(1000); }
        }
        private void Tray_Icon_BalloonTipClicked(object sender, EventArgs e) { Show(); WindowState = FormWindowState.Normal; }
        private void Tray_Icon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) { Show(); WindowState = FormWindowState.Normal; }
            else if (WindowState == FormWindowState.Normal) WindowState = FormWindowState.Minimized;
        }
        private void AddParams_Click(object sender, EventArgs e)
        {
            AddParams(new string[,] { { apirequestkey.Text, apirequestvalue.Text } });
            apirequestkey.Text = apirequestvalue.Text = "";
        }
        private async void APIRequest_Click(object sender, EventArgs e) { APIRequest(); }
        private void Message_KeyDown(object sender, KeyEventArgs e) { if (Control.ModifierKeys == Keys.Shift && e.KeyCode == Keys.Return) SendMessage(); }
        private void Auth_Click(object sender, EventArgs e) { OAuthWindow oauth = new OAuthWindow(new OAuthDelegate(HandleToken)); oauth.Show(); }
        private void SendMessage_Click(object sender, EventArgs e) { SendMessage(); }
        private async void SendMessage()
        {
            GetRequestURI = $"{APIlink}messages.send?random_id={new Random().Next(999, 999999)}&peer_id={peer_id.Text}&{GetRequestParams}message={message.Text}&access_token={Token}&v={APIversion}";
            await GET_Request();
            //tbLog.Text = JSON;
            //foreach (KeyValuePair<string, string> pair in JsonConvert.DeserializeObject<Dictionary<string, string>>(JSON)) tbLog.Text += pair.Key + " - " + pair.Value + "\n"; //Debug.WriteLine(pair.Key, " - ", );
            JSON = message.Text = String.Empty;
        }
        private async void APIRequest()
        {
            GetRequestURI += $"{apirequest.Text}?{GetRequestParams}access_token={Token}&v={APIversion}";
            Debug.WriteLine(GetRequestURI);
            await GET_Request();
            foreach (KeyValuePair<string, string> pair in JsonConvert.DeserializeObject<Dictionary<string, string>>(JSON)) tbLog.Text += pair.Key + " - " + pair.Value + "\n"; //Debug.WriteLine(pair.Key, " - ", );
            JSON = String.Empty;
        }
        private async Task GET_Request()
        {
            try
            {
                WebRequest request = WebRequest.Create(GetRequestURI);
                WebResponse response = await request.GetResponseAsync();
                JSON = new StreamReader(response.GetResponseStream()).ReadToEnd();
                response.Close();
                Debug.WriteLine(GetRequestURI);
                GetRequestURI = "https://api.vk.com/method/";
                GetRequestParams = String.Empty;
            }
            catch { }
        }
        private void AddParams(string[,] keyvalue) { for (int i = 0; i < keyvalue.Length / 2; i++) GetRequestParams += keyvalue[i, 0] + "=" + keyvalue[i, 1] + "&"; }
        private void HandleToken(string accesstoken) { Token = accesstoken; }
        public fox()
        {
            InitializeComponent();
        }
    }
}
