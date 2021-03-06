﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lego_api_data.Helpers
{
    public abstract class MsSQLRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected LegoDbContext _dbContext { get; set; }

        protected MsSQLRepository(LegoDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public List<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            var result = _dbContext.Set<TEntity>().Where(expression).ToList();

            return result;
        }
    }
}
