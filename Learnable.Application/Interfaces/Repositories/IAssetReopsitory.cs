using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Interfaces.Repositories
{
    public interface IAssetReopsitory : IGenericRepository<Asset>
    {
        Task<Asset?> GetAssetWithOcrAsync(Guid assetId);
        Task<Asset> AddAssetWithOcrAsync(Asset asset);
        Task<bool> DeleteAssetWithOcrAsync(Guid assetId);
    }
}
