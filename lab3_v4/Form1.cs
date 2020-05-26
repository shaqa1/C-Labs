using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Drawing.Drawing2D;
using MetroFramework.Forms;
using MetroFramework.Controls;
using NAudio.CoreAudioApi;
using Owin;
using System.Diagnostics;
using System.Net;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Navigation;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.Win32;

namespace lab3_v4
{
    public partial class fox : MetroForm
    {
        private readonly IEngine engine = EngineFactory.Create(new EngineOptions.Builder { LicenseKey = "1BNKDJZJSCZMPTZKWHVDJ65Q3QP48T2PF7SGZB75RJ21CDIAISZO9EP7NUVTSP9CG602LH" }.Build());
        private readonly IBrowser browser;
        private INavigation navigation;
        private const string OAuthURI = "https://oauth.vk.com/authorize";
        private const string RedirectURI = "https://oauth.vk.com/blank.html";
        private const int APPID = 7482854;
        private const double APIversion = 5.103;
        private string GetRequestURI = "https://api.vk.com/method/";
        private static string Token = String.Empty;
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

        private void Authbutton_Click(object sender, EventArgs e)
        {
            navigation.LoadUrl(
                OAuthURI +
                $"?client_id={APPID}" +
                $"&scope=status,offline,notify,friends,audio" +
                $"&redirect_uri={RedirectURI}" +
                $"&display=page" +
                $"&revoke=1" + 
                $"&response_type=token" +
                $"&v=5.104");

            navigation.NavigationRedirected += (s, ev) =>
            {
                string uri = ev.Url;
                Debug.WriteLine(uri);
                if (!uri.StartsWith(RedirectURI)) return;
                var parameters = (from param in uri.Split('#')[1].Split('&')
                                  let parts = param.Split('=')
                                  select new
                                  {
                                      Name = parts[0],
                                      Value = parts[1]
                                  }
                                  ).ToDictionary(v => v.Name, v => v.Value);
                Token = parameters["access_token"];

                Debug.WriteLine(Token);
                GetRequestURI = "https://api.vk.com/method/";

                /*
                string template = "https://api.vk.com/method/{0}?{1}&access_token={2}&v=5.104";
                var client = new WebClient { Encoding = Encoding.UTF8 };
                string json = client.DownloadString(string.Format(template, tbMethod.Text, "HELLO WORLD", Token));
                JObject jsonObject = JObject.Parse(json);
                //tbLog.Text = jsonObject.ToString();
                */
            };
        }
        private static async Task GET_Request_Async(string getrequest)
        {
            WebRequest request = WebRequest.Create(getrequest);
            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    Debug.WriteLine(reader.ReadToEnd());
                }
            }
            response.Close();
            getrequest = "https://api.vk.com/method/";
        }
        private void Addparams_Click(object sender, EventArgs e)
        {
            //paramvalue.Add(apirequestparam.Text, apirequestvalue.Text);
            GetRequestURI += apirequestparam.Text + "=" + apirequestvalue.Text + "&";
            apirequestparam.Text = apirequestvalue.Text = "";
            //apilink.Text = getrequest;
            //foreach (KeyValuePair<string, string> pair in paramvalue) getrequest += pair.Key + "=" + pair.Value + "&";
        }
        private async void Apirequestbutton_Click(object sender, EventArgs e)
        {
            GetRequestURI += "access_token=" + Token + "&v=" + APIversion;
            Debug.WriteLine(GetRequestURI);
            await GET_Request_Async(GetRequestURI);
        }

        private void Setrequest_Click(object sender, EventArgs e)
        {
            GetRequestURI += apirequest.Text + "?";
            //apilink.Text = getrequest;
        }
        public fox()
        {
            InitializeComponent();
            browser = engine.CreateBrowser();
            browserView1.InitializeFrom(browser);
            navigation = browser.Navigation;
        }
        //for (int i = 0; i < paramvalue.Count; i++) getrequest += methodparams[0, i] + "=" + methodparams[1, i] + "&";


        //if (apirequestparam.Text != "" && apirequestvalue.Text != "") paramvalue[]   [](apirequestparam.Text, apirequestvalue.Text);

        /*
        string url;
        if (datacheckbox.Checked)

            url = "https://api.vk.com/method/" +
                apirequest.Text +
                "?text=" +
                apirequestparameters.Text +
                "&access_token=" +
                Token +
                "&v=5.103";

        else url = "https://api.vk.com/method/" +
                apirequest.Text +
                "?" +apirequestparameters.Text+"&access_token=" +
                Token +
                "&v=5.103";

        navigation.LoadUrl(url);





    private static async Task PostRequestAsync()
    {
        //
        string ver = "5.103";
        //
        //
        WebRequest request = WebRequest.Create("https://api.vk.com/method/");
        request.Method = "POST"; // для отправки используется метод Post
        string data = "account.getProfileInfo&access_token=" + Token + "&v=" + ver; // данные для отправки
        byte[] byteArray = Encoding.UTF8.GetBytes(data); // преобразуем данные в массив байтов
        request.ContentType = "application/x-www-form-urlencoded"; // устанавливаем тип содержимого - параметр ContentType
        request.ContentLength = byteArray.Length; // Устанавливаем заголовок Content-Length запроса - свойство ContentLength
        using (Stream dataStream = request.GetRequestStream()) //записываем данные в поток запроса
        {
            dataStream.Write(byteArray, 0, byteArray.Length);
        }

        WebResponse response = await request.GetResponseAsync();
        using (Stream stream = response.GetResponseStream())
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                Debug.WriteLine(reader.ReadToEnd());
            }
        }
        response.Close();
        Debug.WriteLine("Запрос выполнен...");
    }
        */



    }
}
