# Add this ItemGroup when initiating a new project:

```
<ItemGroup>
    <Protobuf Include="../*.proto" OutputDir="%(RelativePath)models" />
</ItemGroup>
```

# gRPC error codes:

- https://github.com/grpc/grpc/blob/master/doc/statuscodes.md

# Reflection:

- https://github.com/grpc/grpc/blob/master/doc/csharp/server_reflection.md
