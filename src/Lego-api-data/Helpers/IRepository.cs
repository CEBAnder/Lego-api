using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Lego_api_data.Helpers
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> Find(Expression<Func<TEntity, bool>> expression);
    }
}
