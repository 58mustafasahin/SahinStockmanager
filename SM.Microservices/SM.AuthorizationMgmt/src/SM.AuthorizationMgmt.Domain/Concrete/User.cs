using SM.Core.Domain.Entities;

namespace SM.AuthorizationMgmt.Domain.Concrete
{
    public class User : EntityDefault
    {
        public long? Id { get; set; }
        public long? CitizenId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string BirthPlace { get; set; }
        public DateTime BirthDate { get; set; }
        public string Username { get; set; }
        public Guid UserCode { get; set; }
        public int GenderId { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }

        public string GetFullName()
        {
            return (Name + " " + Surname).Trim();
        }
    }
}
