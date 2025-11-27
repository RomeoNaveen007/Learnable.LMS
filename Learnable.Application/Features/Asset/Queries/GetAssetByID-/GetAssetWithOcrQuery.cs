using Learnable.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Asset.Queries.GetAssetByID_
{
    public class GetAssetWithOcrQuery(Guid AssetId) : IRequest<AssetDto?>
    {
        public Guid AssetId { get; } = AssetId;
    }
}
