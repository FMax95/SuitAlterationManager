using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using SuitAlterationManager.Domain.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuitAlterationManager.Domain.SystemManagement
{
    public class UserInformation : Entity<UserID>
    {
        public DateTime? BirthDate { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string Image { get; protected set; }

        public virtual User User { get; protected set; }

        public UserInformation()
        {

        }

        public static UserInformation Create(User user, string firstName, string lastName, DateTime? birthDate = null, string image = null)
        {
            var userInformation = new UserInformation
            {
                User = user,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                Image = image,
            };

            return userInformation;
        }

        public void Update(string firstName, string lastName, DateTime? birthDate = null, string image = null)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Image = image;
        }
    }
}
