using MediatR;

public record DeleteAssetWithOcrCommand(Guid AssetId) : IRequest<bool>;
