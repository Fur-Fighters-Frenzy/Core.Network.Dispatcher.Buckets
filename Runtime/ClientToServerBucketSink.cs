using Validosik.Core.Buckets;
using Validosik.Core.Network.Dispatcher.Client;
using Validosik.Core.Network.Types;

namespace Validosik.Core.Network.Dispatcher.Buckets
{
    public sealed class ClientToServerBucketSink<TKind> : IClientParsedSink<TKind, IClientEventBroadcast>
        where TKind : unmanaged, System.Enum
    {
        public void OnParsed<TDto>(TKind kind, PlayerId sender, in TDto dto)
            where TDto : struct, IClientEventBroadcast
        {
            BucketHub.Publish(new FromClient<TDto>(sender, dto));
        }
    }
}