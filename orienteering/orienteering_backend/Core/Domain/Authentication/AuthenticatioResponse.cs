namespace orienteering_backend.Core.Domain.Authentication
{
    //kilde 2/2/23: https://www.endpointdev.com/blog/2022/06/implementing-authentication-in-asp.net-core-web-apis/
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
