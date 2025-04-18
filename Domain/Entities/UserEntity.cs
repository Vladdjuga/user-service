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
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required Email Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required DateTime CreatedAt { get; set; }
        public virtual IEnumerable<UserChatEntity>? UserChatEntities { get; set; }
    }
}
