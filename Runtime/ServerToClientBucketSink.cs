using Validosik.Core.Buckets;
using Validosik.Core.Network.Dispatcher.Server;

namespace Validosik.Core.Network.Dispatcher.Buckets
{
    public sealed class ServerToClientBucketSink<TKind> : IServerParsedSink<TKind, IServerEventBroadcast>
        where TKind : unmanaged, System.Enum
    {
        public void OnParsed<TDto>(TKind kind, in TDto dto)
            where TDto : struct, IServerEventBroadcast
        {
            BucketHub.Publish(dto);
        }
    }
}