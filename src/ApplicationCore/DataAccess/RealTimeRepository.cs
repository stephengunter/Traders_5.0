using Infrastructure.DataAccess;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace ApplicationCore.DataAccess
{
	
	public interface IRealTimeRepository<T> : IRepository<T>, IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
	{

	}
	
	public class RealTimeRepository<T> : EfRepository<T>, IRealTimeRepository<T> where T : BaseEntity, IAggregateRoot
	{
		public RealTimeRepository(RealTimeContext context) : base(context)
		{

		}
	}
}
