using AppServer.Business.Core;
using AppServer.Business.Database;
using AppServer.Business.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppServer.Business.Service
{
    public interface IUserService
    {
        Task<UserEntity?> FindAsync(Expression<Func<UserEntity, bool>> predicate);
        Task<IEnumerable<UserEntity>> GetAsync(Expression<Func<UserEntity, bool>> predicate = null!);
        Task<Guid> CreateAsync(UserEntity model);
        Task UpdateAsync(Guid id, UserEntity model);
        Task DeleteAsync(Guid id);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserEntity>> GetAsync(Expression<Func<UserEntity, bool>> predicate = null!)
        {
            var query = _context.Set<UserEntity>().AsQueryable();

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync();
        }

        public async Task<Guid> CreateAsync(UserEntity model)
        {
            var existing = await FindAsync(x => x.Email.Equals(model.Email));
            if (existing is not null)
            {
                throw new AppException($"Email [{model.Email}] đã tồn tại, vùi lòng chọn email khác.]");
            }

            model.Id = Guid.NewGuid();
            model.Avatar = "_content/MudThemeLibrary/images/default-girl.png";

            _context.Set<UserEntity>().Add(model);
            await _context.SaveChangesAsync();
            return model.Id;
        }

        public async Task UpdateAsync(Guid id, UserEntity model)
        {
            var existing = await _context.Set<UserEntity>().FindAsync(id);
            if (existing is null)
            {
                throw new AppException("Không tìm thấy data.");
            }

            existing.Name = model.Name;
            existing.Email = model.Email;
            existing.Password = model.Password;
            existing.Avatar = model.Avatar;

            _context.Entry(existing).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var player = await _context.Set<UserEntity>().FindAsync(id);
            if (player is null)
            {
                throw new AppException("Không tìm thấy data.");
            }

            _context.Set<UserEntity>().Remove(player);
            await _context.SaveChangesAsync();
        }

        public async Task<UserEntity?> FindAsync(Expression<Func<UserEntity, bool>> predicate)
        {
            return await _context.Set<UserEntity>().Where(predicate).FirstOrDefaultAsync();
        }
    }
}
