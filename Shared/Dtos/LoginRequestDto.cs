namespace Shared.Dtos
{
    public record LoginRequestDto
    {
        public string UniversityCode { get; set; }=string.Empty;
        public string Password { get; set; }=string.Empty;
    }
}
