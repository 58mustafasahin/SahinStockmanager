﻿using SM.Core.DataAccess;
using SM.ProductMgmt.Domain.Concrete;

namespace SM.ProductMgmt.DataAccess.Abstract
{
    public interface ICategoryRepository : IEntityDefaultRepository<Category>
    {
    }
}
