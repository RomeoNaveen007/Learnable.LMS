using Learnable.Application.Interfaces.Repositories;
using Learnable.Domain.Entities;
using Learnable.Infrastructure.Implementations.Repositories.Generic;
using Learnable.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Learnable.Infrastructure.Implementations.Repositories
{
    public class AssetReopsitory : GenericRepository<Asset>, IAssetReopsitory
    {
        private readonly ApplicationDbContext _context;

        public AssetReopsitory(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<Asset?> GetAssetWithOcrAsync(Guid assetId)
        {
            return await _context.Assets
                .Include(a => a.OcrPdfs)  
                .FirstOrDefaultAsync(a => a.AssetsProfileId == assetId);
        }


        public async Task<Asset> AddAssetWithOcrAsync(Asset asset)
        {

            await _context.Assets.AddAsync(asset);

            await _context.SaveChangesAsync();

            return asset;
        }


        public async Task<bool> DeleteAssetWithOcrAsync(Guid assetId)
        {
            var asset = await _context.Assets
                .Include(a => a.OcrPdfs)
                .FirstOrDefaultAsync(a => a.AssetsProfileId == assetId);

            if (asset == null)
                return false; 
            _context.Assets.Remove(asset);

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
