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

        // =====================================================
        // ⭐ Get Asset Details with OCR PDF list
        // =====================================================
        public async Task<Asset?> GetAssetWithOcrAsync(Guid assetId)
        {
            return await _context.Assets
                .Include(a => a.OcrPdfs)   // Load OCR Chunks
                .FirstOrDefaultAsync(a => a.AssetsProfileId == assetId);
        }

        // =====================================================
        // ⭐ Add Asset with OCR PDF list
        // =====================================================
        public async Task<Asset> AddAssetWithOcrAsync(Asset asset, List<OcrPdf> ocrPdfs)
        {
            // Assign OCR list to Asset
            asset.OcrPdfs = ocrPdfs;

            // Add Asset (cascades OcrPdf because of relationship)
            await _context.Assets.AddAsync(asset);

            // Save changes
            await _context.SaveChangesAsync();

            return asset;
        }

        public async Task<bool> DeleteAssetWithOcrAsync(Guid assetId)
        {
            // Find asset including OCR chunks
            var asset = await _context.Assets
                .Include(a => a.OcrPdfs)
                .FirstOrDefaultAsync(a => a.AssetsProfileId == assetId);

            if (asset == null)
                return false; // Asset not found

            // Remove asset (OcrPdfs will be deleted because of Cascade delete)
            _context.Assets.Remove(asset);

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
