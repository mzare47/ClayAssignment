namespace AccessControl.Api.Models.Dto
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
