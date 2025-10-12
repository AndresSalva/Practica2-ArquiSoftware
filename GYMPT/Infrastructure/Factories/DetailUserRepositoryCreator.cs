﻿using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Persistence;

namespace GYMPT.Infrastructure.Factories
{

    public class DetailUserRepositoryCreator : RepositoryCreator<DetailsUser>
    {
        public override IRepository<DetailsUser> CreateRepository()
        {
            return new DetailUserRepository();
        }
    }
}