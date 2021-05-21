using Infrastructure.DataAccess;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace ApplicationCore.DataAccess
{
	
	public interface IHistoryRepository<T> : IRepository<T>, IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
	{

	}
	
	public class HistoryRepository<T> : EfRepository<T>, IHistoryRepository<T> where T : BaseEntity, IAggregateRoot
	{
		public HistoryRepository(HistoryContext context) : base(context)
		{

		}
	}
}
