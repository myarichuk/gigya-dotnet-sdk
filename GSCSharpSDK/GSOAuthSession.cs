using System;

namespace Gigya.Socialize.SDK
{
    //session returned from OAuth2 authentication
    public ref struct GSOAuthSession
    {
        private readonly ReadOnlySpan<char> _secret;
        private readonly ReadOnlySpan<char> _accessToken;
        private DateTimeOffset _expirationTime;

        internal GSOAuthSession(ReadOnlySpan<char> accessToken, ReadOnlySpan<char> secret, DateTimeOffset expirationTime)
        {
            _accessToken = accessToken;
            _secret = secret;
            _expirationTime = expirationTime;
        }

        public bool IsValid => _accessToken.IsEmpty == false &&
                              ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() < _expirationTime.ToUnixTimeSeconds();

        public ReadOnlySpan<char> Secret => _secret;
        public ReadOnlySpan<char> AccessToken => _accessToken;
    }
}
