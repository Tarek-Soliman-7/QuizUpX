namespace Shared.Dtos
{
    public record LoginResponseDto
    {
        public Guid Id { get; set; }
        public string Token { get; set; }= string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
