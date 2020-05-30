using System;
using System.Windows.Forms;
using MetroFramework.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Navigation;
using static foxclient.fox;

namespace foxclient
{
    public partial class OAuthWindow : MetroForm
    {
        private IBrowser browser;
        private INavigation navigation;
        private static string accesstoken = String.Empty;
        private const string OAuthURI = "https://oauth.vk.com/authorize";
        private const string RedirectURI = "https://oauth.vk.com/blank.html";
        private const int APPid = 2685278; //7482854; //currently using kate mobile appid
        private const string ScopeParameters = "‭notify,friends,photos,audio,video,stories,pages,status,notes,messages,wall,ads,offline,docs,groups,notifications,stats,email,market";
        private const string OAuthType = "mobile";
        internal OAuthWindow(OAuthDelegate sender)
        {
            InitializeComponent();
            browser = fox.engine.CreateBrowser();
            authwindow.InitializeFrom(browser);
            navigation = browser.Navigation;
            navigation.LoadUrl($"{OAuthURI}?client_id={APPid}&scope={ScopeParameters}&redirect_uri={RedirectURI}&display={OAuthType}&response_type=token&v={APIversion}"); //&revoke=1
            navigation.NavigationRedirected += (pageobj, redirect) =>
            {
                string uri = redirect.Url;
                if (!uri.StartsWith(RedirectURI)) return;
                accesstoken = uri.Split('#')[1].Split('&')[0].Split('=')[1];
                sender(accesstoken);
                Invoke((MethodInvoker)(() => Close()));
            };  
        }
    }
}
