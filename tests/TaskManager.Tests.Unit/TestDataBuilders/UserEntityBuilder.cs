using TaskManager.Domain.Entities;

namespace TaskManager.Tests.Unit.TestDataBuilders
{
    public class UserEntityBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _userName = "DefaultUser";
        private string _email = "default@email.com";
        private DateTime _createdAt = DateTime.UtcNow;

        public UserEntityBuilder WithId(Guid id) { _id = id; return this; }
        public UserEntityBuilder WithUserName(string userName) { _userName = userName; return this; }
        public UserEntityBuilder WithEmail(string email) { _email = email; return this; }
        public UserEntityBuilder WithCreatedAt(DateTime createdAt) { _createdAt = createdAt; return this; }

        public UserEntity Build()
        {
            return new UserEntity
            {
                Id = _id,
                UserName = _userName,
                Email = _email,
                CreatedAt = _createdAt
            };
        }
    }
}