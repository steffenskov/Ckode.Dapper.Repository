using System;
using Ckode.Dapper.Repository.IntegrationTests.Entities;

namespace Ckode.Dapper.Repository.IntegrationTests.MySql.Repositories
{
	public class ProductListViewRepository : MyViewRepository<ProductListViewEntity>
	{
		protected override string ViewName => "current_product_list";
	}
}
