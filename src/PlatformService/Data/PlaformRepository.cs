using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public class PlaformRepository : IPlatformRepository
{
    private readonly AppDbContext _context;

    public PlaformRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Create(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);
        
        _context.Platforms.Add(platform);
    }

    public async Task CreateAsync(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);
        
        await _context.Platforms.AddAsync(platform);
    }

    public IEnumerable<Platform> GetAll()
    {
        return _context.Platforms.ToList();
    }

    public async Task<IEnumerable<Platform>> GetAllAsync()
    {
        return await _context.Platforms.ToListAsync();
    }

    public Platform Get(int id)
    {
        return _context.Platforms.FirstOrDefault(p=>p.Id==id);
    }

    public async Task<Platform> GetAsync(int id)
    {
        return await _context.Platforms.FirstOrDefaultAsync(p=>p.Id==id);
    }

    public bool SaveChanges()
    {
        return (_context.SaveChanges()>=0);
    }  

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync()>=0);
    }   
}
