using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using HotChocolate;
using HotChocolate.Types;

namespace backend.GraphQL.Types
{
    public class UserType: ObjectType<User>
    {
       protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor.Field(x => x.Id);
            descriptor.Field(x => x.Auth0Id);
            descriptor.Field(x => x.FirstName);
            descriptor.Field(x => x.LastName);
            descriptor.Field(x => x.Email);
            descriptor.Field(x => x.EmailVerified);
            descriptor.Field(x => x.Picture);
            descriptor.Field(x => x.Roles);
            descriptor.Field(x => x.Location);
            descriptor.Field(x => x.BirthDate);
            descriptor.Field(x => x.Phone);
            descriptor.Field(x => x.PrivacyConsent);
            descriptor.Field(x => x.CreatedAt);
            descriptor.Field(x => x.UpdatedAt);
        }
    }
}