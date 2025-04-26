using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class UserEntity
    {
        public Guid Id { get; init; }
        public required string Username { get; init; }
        public required Email Email { get; init; }
        public required string PasswordHash { get; set; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required DateTime DateOfBirth { get; init; }
        public required DateTime CreatedAt { get; init; }
        public virtual IEnumerable<UserChatEntity>? UserChatEntities { get; init; }
        public virtual IEnumerable<UserContactEntity>? Contacts { get; init; }
    }
}
