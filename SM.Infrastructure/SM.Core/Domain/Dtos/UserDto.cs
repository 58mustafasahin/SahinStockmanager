namespace SM.Core.Domain.Dtos
{
    public record UserDto
    {
        public long? Id { get; init; }
        public long? CitizenId { get; init; }
        public string Name { get; init; }
        public string SurName { get; init; }
        public string Email { get; init; }
        public string MobilePhone { get; init; }
        public string BirthPlace { get; init; }
        public DateTime BirthDate { get; init; }
        public string UserName { get; init; }
        public Guid UserCode { get; init; }

        public string GetFullName()
        {
            return (Name + " " + SurName).Trim();
        }
    }
}
