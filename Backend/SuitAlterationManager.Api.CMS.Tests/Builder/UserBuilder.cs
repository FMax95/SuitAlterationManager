using System;
using System.Collections.Generic;
using System.Linq;
using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;

namespace SuitAlterationManager.Api.CMS.Tests.Builder
{
    public class UserBuilder
    {
        private string email = "test@mail.it";
        private string password = "password";
        private List<Guid> groups = new List<Guid>();

        public UserBuilder WithEmail(string val)
        {
            this.email = val;
            return this;
        }
        public UserBuilder WithPassword(string val)
        {
            this.password = val;
            return this;
        }


        public UserBuilder WithGroup(Guid val)
        {
            this.groups.Add(val);
            return this;
        }

        public static implicit operator User(UserBuilder builder)
        {
            if (builder.groups.Any() == false)
                builder.groups.Add(Guid.NewGuid());

            var user = new User()
            {
                Email = builder.email,
                Password = builder.password
            };
            foreach (var idGroup in builder.groups)
                user.AddGroup(new GroupID(idGroup), DateTime.Now);
            return user;
        }

    }
}
