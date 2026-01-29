using Validosik.Core.Buckets;
using Validosik.Core.Network.Types;

namespace Validosik.Core.Network.Dispatcher.Buckets
{
    public readonly struct FromClient<TDto> : IBroadcast
        where TDto : struct, IBroadcast
    {
        public readonly PlayerId Sender;
        public readonly TDto Payload;

        public FromClient(PlayerId sender, TDto payload)
        {
            Sender = sender;
            Payload = payload;
        }
    }
}