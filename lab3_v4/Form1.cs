using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using DotNetBrowser.Engine;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace lab3_v4
{
    public partial class fox : MetroForm
    {
        internal static IEngine engine = EngineFactory.Create(new EngineOptions.Builder { LicenseKey = "1BNKDJZJSCZMPTZKWHVDJ65Q3QP48T2PF7SGZB75RJ21CDIAISZO9EP7NUVTSP9CG602LH" }.Build());
        public delegate void OAuthDelegate(string accesstoken);
        internal const string APIversion = "5.103";
        private string GetRequestURI = "https://api.vk.com/method/";
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
        private void HandleToken(string accesstoken) { Token = accesstoken; }
        private async Task GET_Request()
        {
            try
            {
                WebRequest request = WebRequest.Create(GetRequestURI);
                WebResponse response = await request.GetResponseAsync();
                JSON = new StreamReader(response.GetResponseStream()).ReadToEnd();
                response.Close();
                GetRequestURI = "https://api.vk.com/method/";
            }
            catch { tbLog.Text = "get request failed, no retry"; }
        }
        private void AddParams_Click(object sender, EventArgs e)
        {
            GetRequestURI += apirequestparam.Text + "=" + apirequestvalue.Text + "&";
            apirequestparam.Text = apirequestvalue.Text = "";
        }
        private void SetRequest_Click(object sender, EventArgs e)
        {
            GetRequestURI += apirequest.Text + "?";
            apirequest.Text = "";
        }
        private async void APIRequest_Click(object sender, EventArgs e)
        {
            GetRequestURI += "access_token=" + Token + "&v=" + APIversion;
            Debug.WriteLine(GetRequestURI);
            await GET_Request();
            //foreach (KeyValuePair<string, string> pair in JsonConvert.DeserializeObject<Dictionary<string, string>>(JSON)) tbLog.Text += pair.Key + " - " + pair.Value + "\n"; //Debug.WriteLine(pair.Key, " - ", );
            //JSON = String.Empty;
            tbLog.Text = JSON;
        }
        private void Auth_Click(object sender, EventArgs e) { OAuthWindow oauth = new OAuthWindow(new OAuthDelegate(HandleToken)); oauth.Show(); }
        public fox()
        {
            InitializeComponent();
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
