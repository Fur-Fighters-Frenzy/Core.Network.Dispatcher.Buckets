# Core.Network.Dispatcher.Buckets

Buckets integration for **Core.Network.Dispatcher**.

This package provides an optional bridge that publishes parsed dispatcher DTOs into `BucketHub`.
It keeps the core dispatcher transport-agnostic and event-bus-agnostic, while offering a ready-made
integration layer for projects that use **Validosik.Core.Buckets**.

- [Core.Network.Dispatcher](https://github.com/Fur-Fighters-Frenzy/Core.Network.Dispatcher)
- [Core.Buckets](https://github.com/Fur-Fighters-Frenzy/Core.Buckets)

> **Status:** WIP

---

## What’s included

- `IBroadcastMarker`: marker interface (no Buckets reference)
- `IClientEventBroadcast`: marker for **client -> server** DTOs that should be published to `BucketHub`
- `IServerEventBroadcast`: marker for **server -> client** DTOs that should be published to `BucketHub`
- `FromClient<T>`: typed wrapper that carries `PlayerId Sender` + `T Payload`
- `ServerToClientBucketSink<TKind>`: publishes parsed server DTOs directly: `BucketHub.Publish(dto)`
- `ClientToServerBucketSink<TKind>`: publishes parsed client DTOs as `FromClient<TDto>`

Optionally:

- `InitializeBuckets(...)` extensions to bootstrap dispatchers with the provided sinks

---

## DTO requirements

A DTO must:

- be a `struct`
- implement `IEventDto<TKind>`
- implement one of the marker interfaces:
  - `IServerEventBroadcast` (server -> client)
  - `IClientEventBroadcast` (client -> server)
- expose a static parser:

```csharp
public static bool TryFromBytes(ReadOnlySpan<byte> span, out MyDto dto)
````

`TKind` is expected to have `ushort` underlying type (dispatcher uses `ushort kindId`).

---

## Usage

### Client: parse server events and publish into BucketHub

```csharp
_serverDispatcher = Ioc.Instance.Resolve<IServerEventDispatcher>();

ServerDispatcherBootstrapper<IServerEventDispatcher, ServerEventKind, IServerEventBroadcast>
    .Initialize(
        dispatcher: _serverDispatcher,
        sink: new ServerToClientBucketSink<ServerEventKind>(),
        assemblies: typeof(ServerEventKind).Assembly);
```

Or with the optional extension:

```csharp
using Validosik.Core.Network.Dispatcher.Buckets;

_serverDispatcher.InitializeBuckets<IServerEventDispatcher, ServerEventKind>(
    new DispatcherBucketsBootstrapExtensions.ServerBuckets(),
    typeof(ServerEventKind).Assembly);
```

### Server: parse client events and publish as FromClient<T>

```csharp
_clientDispatcher = Ioc.Instance.Resolve<IClientEventDispatcher>();

ClientDispatcherBootstrapper<IClientEventDispatcher, ClientEventKind, IClientEventBroadcast>
    .Initialize(
        dispatcher: _clientDispatcher,
        sink: new ClientToServerBucketSink<ClientEventKind>(),
        assemblies: typeof(ClientEventKind).Assembly);
```

Or with the optional extension:

```csharp
using Validosik.Core.Network.Dispatcher.Buckets;

_clientDispatcher.InitializeBuckets<IClientEventDispatcher, ClientEventKind>(
    new DispatcherBucketsBootstrapExtensions.ClientBuckets(),
    typeof(ClientEventKind).Assembly);
```

> Note: pass the assembly that contains the DTOs for the selected `TKind`.
> For server-side client DTO dispatch this is typically `typeof(ClientEventKind).Assembly`.

### Handling sender-wrapped broadcasts

Client-originated DTOs are published as:

```csharp
FromClient<TDto>
```

So you can subscribe like:

```csharp
BucketHub.GetOrCreate<FromClient<MyClientDto>>()
    .Invoke(new FromClient<MyClientDto>(pid, dto));
```

---

## Notes

* Marker-based filtering prevents accidental registration/publishing of DTOs.
* Reflection-based DTO discovery may require `Preserve`/`link.xml` under IL2CPP stripping.
* The sinks do not allocate per event dispatch; they publish typed payloads into `BucketHub`.
* This package is optional. If you do not use Buckets, you only need `Core.Network.Dispatcher`.

---

# Part of the Core Project

This package is part of the **Core** project, which consists of multiple Unity packages.
See the full project here: [Core](https://github.com/Fur-Fighters-Frenzy/Core)

---