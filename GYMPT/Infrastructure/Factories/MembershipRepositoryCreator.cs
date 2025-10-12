﻿using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Persistence;

namespace GYMPT.Infrastructure.Factories
{

    public class MembershipRepositoryCreator : RepositoryCreator<Membership>
    {
        public override IRepository<Membership> CreateRepository()
        {
            return new MembershipRepository();
        }
    }
}