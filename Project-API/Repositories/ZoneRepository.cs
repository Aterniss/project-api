using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Repositories
{
    public class ZoneRepository : IZoneRepository
    {
        private readonly MyDbContext _context;
        public ZoneRepository(MyDbContext context)
        {
            this._context = context;
        }

        public async Task AddZone(Zone zone)
        {
            var checkZoneName = await _context.Zones.FirstOrDefaultAsync(x => x.ZoneName == zone.ZoneName);
            if(checkZoneName != null)
            {
                throw new Exception($"Zone with zone name: \"{zone.ZoneName}\" already exist!");
            }
            else
            {
                await _context.Zones.AddAsync(zone);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteZone(int id)
        {
            var existZone = await _context.Zones.FirstOrDefaultAsync(x => x.ZoneId == id);
            if(existZone == null)
            {
                throw new Exception($"Zone with ID: \"{id}\" has not been found!");
            }
            else
            {
                _context.Zones.Remove(existZone);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Zone>> GetAll()
        {
            return await _context.Zones.ToListAsync();
        }

        public async Task<Zone> GetById(int id)
        {
            var result = await _context.Zones.FirstOrDefaultAsync(x => x.ZoneId == id);
            if(result == null)
            {
                return null;
            }
            return result;
        }

        public async Task UpdateZone(Zone zone, int id)
        {
            var existZone = await _context.Zones.FirstOrDefaultAsync(x => x.ZoneId == id);
            var existZoneName = await _context.Zones.FirstOrDefaultAsync(x => x.ZoneName == zone.ZoneName);
            if(existZone != null && existZoneName == null)
            {
                existZone.ZoneName = zone.ZoneName;
            }
            else if(existZone != null && existZoneName != null)
            {
                throw new Exception($"Zone with name: \"{zone.ZoneName}\" already exist!");
            }
            else
            {
                throw new Exception($"Zone with ID: \"{id}\" does not exist!");
            }
        }
    }
}
