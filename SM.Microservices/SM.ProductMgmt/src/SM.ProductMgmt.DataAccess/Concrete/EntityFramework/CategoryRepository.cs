using SM.Core.DataAccess.EntityFramework;
using SM.ProductMgmt.DataAccess.Abstract;
using SM.ProductMgmt.DataAccess.Contexts;
using SM.ProductMgmt.Domain.Concrete;

namespace SM.ProductMgmt.DataAccess.Concrete.EntityFramework
{
    public class CategoryRepository : EfEntityRepositoryBase<Category, ProductDbContext>, ICategoryRepository
    {
        public CategoryRepository(ProductDbContext context) : base(context)
        {
        }
    }
}
