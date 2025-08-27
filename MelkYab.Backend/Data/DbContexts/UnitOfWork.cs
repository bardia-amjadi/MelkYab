using MelkYab.Backend.Data.Interfaces;
using MelkYab.Backend.Data.Services;
using MelkYab.Backend.Data.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MelkYab.Backend.Data.DbContexts
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly SignInManager<User> SignInManager;
        private readonly UserManager<User> UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public UnitOfWork(SignInManager<User> signInManager,UserManager<User> userManager,RoleManager<IdentityRole> roleManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            RoleManager = roleManager;
        }
        public UnitOfWork(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            RoleManager = roleManager;
            _context = context;

        }


        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var repoInstance = new Repository<T>(_context);
                _repositories.Add(type, repoInstance);
            }

            return (IRepository<T>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}