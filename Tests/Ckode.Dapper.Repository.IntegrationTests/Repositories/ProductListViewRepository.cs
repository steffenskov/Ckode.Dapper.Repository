using System;
using Ckode.Dapper.Repository.IntegrationTests.Entities;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public class ProductListViewRepository : MyViewRepository<ProductListViewEntity>
	{
		protected override string ViewName => "Current Product List";
	}
}
