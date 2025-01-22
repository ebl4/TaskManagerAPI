using Microsoft.EntityFrameworkCore;
using Moq;

namespace TaskManagerAPITest.Services
{
    public abstract class BaseServiceTest
    {
        protected Mock<ApplicationDbContext> MockContext;

        protected BaseServiceTest()
        {
            MockContext = new Mock<ApplicationDbContext>();
        }

        protected Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();

            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            mockDbSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(data.ToList().Add);
            mockDbSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(t => data.ToList().Remove(t));

            return mockDbSet;
        }
    }

}
