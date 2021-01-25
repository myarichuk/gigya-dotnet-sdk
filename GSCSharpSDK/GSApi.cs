using Polly;
using Polly.Extensions.Http;
using Simple.HttpClientFactory;
using System;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace Gigya.Socialize.SDK
{
    public class GSApi : IDisposable
    {
        private readonly HttpClient _client;

        public GSApi(TimeSpan? timeout = null)
        {
            var builder = HttpClientFactory.Create();

#pragma warning disable HAA0101 // Array allocation for params parameter
            builder.WithDefaultHeader("X-Origin", "GigyaSDK")
                .WithPolicy(
                    Policy.TimeoutAsync(timeout ?? TimeSpan.FromSeconds(10))
                          .WrapAsync(RetryPolicy()));
#pragma warning restore HAA0101 // Array allocation for params parameter

            _client = builder.Build();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IAsyncPolicy<HttpResponseMessage> RetryPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        public void Dispose() => _client.Dispose();
    }
}
