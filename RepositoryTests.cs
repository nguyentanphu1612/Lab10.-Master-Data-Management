using ASC.DataAccess;
using ASC.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ASC.Tests
{
    public class RepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly Repository<ServiceRequest> _repository;
        private bool _disposed;

        public RepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new Repository<ServiceRequest>(_context);
        }

        [Fact]
        public async Task Repository_AddAsync_AddsEntity()
        {
            // Arrange
            var entity = new ServiceRequest
            {
                CustomerName = "Test Customer",
                ServiceType = "Engine Repair",
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            var result = await _repository.AddAsync(entity);
            await _context.SaveChangesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task Repository_GetByIdAsync_ReturnsEntity()
        {
            // Arrange
            var entity = new ServiceRequest
            {
                CustomerName = "Test Customer 2",
                ServiceType = "Brake Repair",
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(entity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity.Id, result?.Id);
            Assert.Equal("Test Customer 2", result?.CustomerName);
        }

        [Fact]
        public async Task Repository_GetAllAsync_ReturnsAllEntities()
        {
            // Arrange
            await _repository.AddAsync(new ServiceRequest { CustomerName = "Customer 1", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            await _repository.AddAsync(new ServiceRequest { CustomerName = "Customer 2", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Repository_FindAsync_ReturnsMatchingEntities()
        {
            // Arrange
            await _repository.AddAsync(new ServiceRequest { CustomerName = "John Doe", ServiceType = "Oil Change", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            await _repository.AddAsync(new ServiceRequest { CustomerName = "Jane Doe", ServiceType = "Tire Rotation", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.FindAsync(x => x.ServiceType == "Oil Change");

            // Assert
            Assert.Single(result);
            Assert.Equal("John Doe", result.First().CustomerName);
        }

        [Fact]
        public async Task Repository_UpdateAsync_UpdatesEntity()
        {
            // Arrange
            var entity = new ServiceRequest
            {
                CustomerName = "Original Name",
                ServiceType = "Test",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _context.SaveChangesAsync();

            // Act
            if (entity.CustomerName != null)
            {
                entity.CustomerName = "Updated Name";
            }
            await _repository.UpdateAsync(entity);
            await _context.SaveChangesAsync();

            // Assert
            var updatedEntity = await _repository.GetByIdAsync(entity.Id);
            Assert.Equal("Updated Name", updatedEntity?.CustomerName);
        }

        [Fact]
        public async Task Repository_DeleteAsync_RemovesEntity()
        {
            // Arrange
            var entity = new ServiceRequest
            {
                CustomerName = "To Delete",
                ServiceType = "Test",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(entity);
            await _context.SaveChangesAsync();

            // Assert
            var deletedEntity = await _repository.GetByIdAsync(entity.Id);
            Assert.Null(deletedEntity);
        }

        [Fact]
        public async Task Repository_ExistsAsync_ReturnsTrueIfExists()
        {
            // Arrange
            var entity = new ServiceRequest
            {
                CustomerName = "Exists Test",
                ServiceType = "Test",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(entity);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.ExistsAsync(x => x.CustomerName == "Exists Test");

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task Repository_CountAsync_ReturnsCorrectCount()
        {
            // Arrange
            await _repository.AddAsync(new ServiceRequest { CustomerName = "Count 1", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            await _repository.AddAsync(new ServiceRequest { CustomerName = "Count 2", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            await _repository.AddAsync(new ServiceRequest { CustomerName = "Count 3", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            await _context.SaveChangesAsync();

            // Act
            var count = await _repository.CountAsync();

            // Assert
            Assert.Equal(3, count);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
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