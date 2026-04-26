using ASC.DataAccess;
using ASC.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ASC.Tests
{
    public class UnitOfWorkTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;
        private bool _disposed;

        public UnitOfWorkTests()
        {
            // Tạo in-memory database cho testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _unitOfWork = new UnitOfWork(_context);
        }

        [Fact]
        public async Task UnitOfWork_AddAndCommitAsync_SavesEntity()
        {
            // Arrange
            var serviceRequest = new ServiceRequest
            {
                CustomerName = "Test Customer",
                ServiceType = "Oil Change",
                Status = "Pending",
                EstimatedCost = 100m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            var repo = _unitOfWork.Repository<ServiceRequest>();
            await repo.AddAsync(serviceRequest);
            var result = await _unitOfWork.CommitAsync();

            // Assert
            Assert.Equal(1, result);
            var savedRequest = await repo.GetByIdAsync(serviceRequest.Id);
            Assert.NotNull(savedRequest);
            Assert.Equal("Test Customer", savedRequest?.CustomerName);
        }

        [Fact]
        public async Task UnitOfWork_GetRepository_ReturnsRepository()
        {
            // Act
            var repo = _unitOfWork.Repository<ServiceRequest>();

            // Assert
            Assert.NotNull(repo);
        }

        [Fact]
        public async Task UnitOfWork_AddMultipleEntities_SavesAll()
        {
            // Arrange
            var product1 = new Product
            {
                ProductCode = "P001",
                ProductName = "Product 1",
                Price = 100m,
                StockQuantity = 10,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var product2 = new Product
            {
                ProductCode = "P002",
                ProductName = "Product 2",
                Price = 200m,
                StockQuantity = 20,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            var productRepo = _unitOfWork.Repository<Product>();
            await productRepo.AddAsync(product1);
            await productRepo.AddAsync(product2);
            var result = await _unitOfWork.CommitAsync();

            // Assert
            Assert.Equal(2, result);
            var allProducts = await productRepo.GetAllAsync();
            Assert.Equal(2, allProducts.Count());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
                _unitOfWork.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}