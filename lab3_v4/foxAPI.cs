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
using Newtonsoft.Json.Linq;

namespace foxclient
{
    public partial class fox : MetroForm
    {
        internal static IEngine engine = EngineFactory.Create(new EngineOptions.Builder { LicenseKey = "1BNKDJZJSCZMPTZKWHVDJ65Q3QP48T2PF7SGZB75RJ21CDIAISZO9EP7NUVTSP9CG602LH" }.Build());
        internal delegate void OAuthDelegate(string accesstoken);
        internal const string APIversion = "5.103";
        private const string APIlink = "https://api.vk.com/method/";
        private string RequestParams = String.Empty;
        private static string Token = String.Empty;
        private static string Response = String.Empty;
        private static string SelectedFriendID = String.Empty;
        private static string UserID = String.Empty;
        PollServerResponse pollresponse = new PollServerResponse();
        
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
        private void APIRequest_Click(object sender, EventArgs e) { APIRequest(); }
        private void Message_KeyDown(object sender, KeyEventArgs e) { if (Control.ModifierKeys == Keys.Shift && e.KeyCode == Keys.Return) SendMessage(); }
        private void Auth_Click(object sender, EventArgs e) { OAuthenticate(); }
        private void SendMessage_Click(object sender, EventArgs e) { SendMessage(); }
        private void Friendslist_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(friendslist.SelectedValue.ToString());
            SelectedFriendID = friendslist.SelectedValue.ToString();
            Invoke((MethodInvoker)(() => { im_log.Text = String.Empty; }));
        }
        private void AddParams(string[,] keyvalue) { for (int i = 0; i < keyvalue.Length / 2; i++) RequestParams += keyvalue[i, 0] + "=" + keyvalue[i, 1] + "&"; }
        private async void APIRequest()
        {
            RequestParams = String.Empty;
            await GET_Request($"{apirequest.Text}?{RequestParams}");
            Response = String.Empty;
        }
        private async void SendMessage()
        {
            RequestParams = String.Empty;
            await GET_Request($"messages.send?random_id={new Random().Next(999, 999999)}&peer_id={friendslist.SelectedValue.ToString()}&{RequestParams}message={message.Text}&");
            Response = message.Text = String.Empty;
        }
        private void ShowMessagesUpdates(string imupdates)
        {
            dynamic imresponse = JObject.Parse(imupdates);
            var fields = imresponse.response;
            if ((object)fields.new_pts != (object)fields.from_pts) pollresponse.response["pts"] = fields.new_pts;
            if (Convert.ToInt32((object)fields.messages.count) != 0)
                for (int i = 0; i < Convert.ToInt32((object)fields.messages.count); i++) 
                    if (fields.messages.items[i].peer_id.ToString() as string == SelectedFriendID) 
                        if (fields.messages.items[i].from_id.ToString() as string == UserID)
                            Invoke((MethodInvoker)(() => { im_log.Text += $"                {fields.messages.items[i].text}" + Environment.NewLine; }));
                        else Invoke((MethodInvoker)(() => { im_log.Text += $"{fields.messages.items[i].text}" + Environment.NewLine; }));
        }
        private async void LongPoll()
        {
            while (true)
            {
                await GET_Request($"messages.getLongPollHistory?ts={pollresponse.response["ts"]}&pts={pollresponse.response["pts"]}&preview_length=0&fields=online,screen_name&events_limit=1000&msgs_limit=200&last_n=0&lp_version=2&");
                ShowMessagesUpdates(Response);
                Response = String.Empty;
                await Task.Delay(500);
            }
        }
        private async Task FetchFriends()
        {
            await GET_Request($"friends.get?order=name&count=5000&fields=domain&");
            dynamic friendsresponse = JObject.Parse(Response);
            var fields = friendsresponse.response;
            for (int i = 0; i < Convert.ToInt32(fields.count); i++)
            {
                string fullname = $"{fields.items[i].first_name} {fields.items[i].last_name}";
                string id = fields.items[i].id;
                pollresponse.friendslist.Add(id, fullname);
            }
            pollresponse.friendslist.Add(UserID, "Me");
            Invoke((MethodInvoker)(() =>
            {
                friendslist.DisplayMember = "Value";
                friendslist.ValueMember = "Key";
                friendslist.DataSource = new BindingSource(pollresponse.friendslist, null);
                friendslist.SelectedValue = UserID;
            }));
        }
        private async void HandleToken(string accesstoken)
        {
            Token = accesstoken;
            Debug.WriteLine(Token);
            await POST_Request($"users.get?");
            dynamic obj = JObject.Parse(Response);
            var fields = obj.response[0];
            UserID = fields.id;
            await FetchFriends();
            await GET_Request($"messages.getLongPollServer?need_pts=1&lp_version=2&");
            pollresponse = JsonConvert.DeserializeObject<PollServerResponse>(Response);
            LongPoll();
        }
        private void OAuthenticate() { OAuthWindow oauth = new OAuthWindow(new OAuthDelegate(HandleToken)); oauth.Show(); }
        private static async Task POST_Request(string requestparameters)
        {
            WebRequest request = WebRequest.Create("https://api.vk.com/method/");
            byte[] parambytearray = System.Text.Encoding.UTF8.GetBytes($"{requestparameters}access_token={Token}&v={APIversion}");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = parambytearray.Length;
            request.GetRequestStream().Write(parambytearray, 0, parambytearray.Length);
            WebResponse response = await request.GetResponseAsync();
            Response = new StreamReader(response.GetResponseStream()).ReadToEnd();
            response.Close();
            GC.Collect();
        }
        private static async Task GET_Request(string requestparameters)
        {
            try
            {
                //Debug.WriteLine($"{APIlink}{requestparameters}access_token={Token}&v={APIversion}");
                WebRequest request = WebRequest.Create($"{APIlink}{requestparameters}access_token={Token}&v={APIversion}");
                WebResponse response = await request.GetResponseAsync();
                Response = new StreamReader(response.GetResponseStream()).ReadToEnd();
                response.Close();
            }
            catch { }
            GC.Collect();
        }
        public fox()
        {
            InitializeComponent();
            OAuthenticate();
        }
    }
    public class PollServerResponse
    {
        public Dictionary<string, string> response { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> friendslist { get; set; } = new Dictionary<string, string>();
    }
}
