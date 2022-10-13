using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Repositories
{
    public class RiderRepository : IRiderRepository
    {
        private readonly MyDbContext _context;
        public RiderRepository(MyDbContext context)
        {
            this._context = context;
        }

        public async Task AddRider(Rider rider)
        {
            var isZoneIdExist = await _context.Zones.FirstOrDefaultAsync(x => x.ZoneId == rider.ZoneId);

            if (isZoneIdExist != null)
            {
                await _context.Riders.AddAsync(rider);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Zone with ID: \"{rider.ZoneId}\" does not exist!");
            }

        }

        public async Task DeleteRider(int id)
        {
            var existRider = await _context.Riders.FirstOrDefaultAsync(x => x.RiderId == id);
            if(existRider != null)
            {
                _context.Riders.Remove(existRider);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Rider with ID: \"{id}\" has not been found!");
            }
        }

        public async Task<IEnumerable<Rider>> GetAll()
        {
            return await _context.Riders
                .Include(x => x.Zone)
                .ToListAsync();
        }

        public async Task<Rider> GetById(int id)
        {
            var existRider = await _context.Riders
                .Include(_x => _x.Zone)
                .FirstOrDefaultAsync(x => x.RiderId == id);
            if(existRider == null)
            {
                return null;
            }
            return existRider;
        }

        public async Task UpdateRider(Rider rider, int id)
        {
            var result = await _context.Riders.FirstOrDefaultAsync(x => x.RiderId == id);
            var checkZoneId = await _context.Zones.FirstOrDefaultAsync(x => x.ZoneId == rider.ZoneId);
            if (result == null)
            {
                throw new Exception($"Rider with ID: \"{id}\" does not exist!");
            }
            else if(result != null && checkZoneId != null)
            {
                result.RiderName = rider.RiderName;
                result.ZoneId = rider.ZoneId;
                await _context.SaveChangesAsync();
            }
            else
             {
                throw new Exception($"Zone with ID: \"{rider.ZoneId}\" does not exist!");
             }
        }  
     }
 }

