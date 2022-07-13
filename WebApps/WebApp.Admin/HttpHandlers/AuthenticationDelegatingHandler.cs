namespace WebApp.Admin.HttpHandlers
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationDelegatingHandler(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");

            request.Headers.Add("Authorization", $"Bearer {token}");

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
