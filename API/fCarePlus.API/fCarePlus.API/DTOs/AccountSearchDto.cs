namespace fCarePlus.API.DTOs
{
    public class AccountSearchDto
    {
        public Guid Id { get; set; }

        public required string NameAR { get; set; }

        public required string Number { get; set; }

        public string FullAccountName => $"{NameAR} - {Number}";
    }
}