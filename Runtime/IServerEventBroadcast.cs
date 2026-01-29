using Validosik.Core.Buckets;
using Validosik.Core.Network.Dispatcher;

namespace Validosik.Core.Network.Dispatcher.Buckets
{
    public interface IServerEventBroadcast : IBroadcastMarker, IBroadcast { } // server -> client
}