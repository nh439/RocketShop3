namespace RocketShop.Identity.Configuration
{
    public class OauthConfiguration
    {
        public string Authority { get; set; }
        public string ClientId {  get; set; }
        public string ClientSecret { get; set; }
        public string[] Scopes { get; set; }
        public string RedirectUrl { get; set; }
        public OauthConfiguration() { }
        public void SetScopes(params string[] scopes) =>
            Scopes = scopes;
    }
}
