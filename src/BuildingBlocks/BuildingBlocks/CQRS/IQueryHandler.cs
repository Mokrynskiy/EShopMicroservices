using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface IQueryHandler<in TQuary, TResponce>
        :IRequestHandler<TQuary, TResponce>
        where TQuary : IQuery<TResponce>
        where TResponce : notnull
    {
    }
}
