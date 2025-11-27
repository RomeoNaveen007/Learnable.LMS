using Learnable.Application.Common.Dtos;
using Learnable.Application.Features.Asset.Commands.AddAsset;
using Learnable.Application.Features.Asset.Queries.GetAssetByID_;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Learnable.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetController : BaseController
    {
        private readonly IMediator _mediator;

        // Constructor for DI
        public AssetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ============================================
        // ⭐ POST API → Add Asset with OCR chunks
        // URL: POST /api/asset/add
        // ============================================
        [HttpPost("add")]
        public async Task<IActionResult> AddAsset([FromBody] AddAssetQuerie request, Guid id)
        {
            var result = await _mediator.Send(request);
            return Created(string.Empty, result);
        }

        // ============================================
        // ⭐ GET API → Get Asset by ID with OCR chunks
        // URL: GET /api/asset/445F80F8-3F9C-40DA-9A71-D960A7EE2732
        // ============================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsset(Guid id)
        {
            var query = new GetAssetWithOcrQuery(id);
            var result = await _mediator.Send(query);

            if (result == null) return NotFound();

            return Ok(result);
        }

        // ============================================
        // ⭐ DELETE API → Delete Asset by ID with OCR chunks
        // URL: DELETE /api/asset/{id}
        // ============================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsset(Guid id)
        {
            var command = new DeleteAssetWithOcrCommand(id);
            var success = await _mediator.Send(command);

            if (!success) return NotFound();

            return NoContent();
        }
    }
}
