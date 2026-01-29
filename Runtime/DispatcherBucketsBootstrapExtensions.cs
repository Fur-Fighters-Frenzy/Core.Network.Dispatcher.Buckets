using System.Reflection;
using Validosik.Core.Network.Dispatcher.Client;
using Validosik.Core.Network.Dispatcher.Server;

namespace Validosik.Core.Network.Dispatcher.Buckets
{
    public static class DispatcherBucketsBootstrapExtensions
    {
        public readonly struct ServerBuckets { }
        public readonly struct ClientBuckets { }

        public static void InitializeBuckets<TDispatcher, TKind>(
            this TDispatcher dispatcher,
            ServerBuckets _,
            params Assembly[] assemblies)
            where TDispatcher : IServerDispatcher
            where TKind : unmanaged, System.Enum
        {
            ServerDispatcherBootstrapper<TDispatcher, TKind, IServerEventBroadcast>
                .Initialize(dispatcher, new ServerToClientBucketSink<TKind>(), assemblies);
        }

        public static void InitializeBuckets<TDispatcher, TKind>(
            this TDispatcher dispatcher,
            ClientBuckets _,
            params Assembly[] assemblies)
            where TDispatcher : IClientDispatcher
            where TKind : unmanaged, System.Enum
        {
            ClientDispatcherBootstrapper<TDispatcher, TKind, IClientEventBroadcast>
                .Initialize(dispatcher, new ClientToServerBucketSink<TKind>(), assemblies);
        }
    }
}