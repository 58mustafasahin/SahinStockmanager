using SM.Core.Domain.Enums;

namespace SM.Core.Domain.Dtos
{
    public record TokenDto
    {
        public UserDto User { get; init; }
        public long SessionId { get; init; }
        public SessionType SessionType { get; init; }
    }
}
