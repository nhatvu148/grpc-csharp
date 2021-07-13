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
- https://github.com/ktr0731/evans
- Run: ./evans.exe -r -p 50051 (turn off ssl first)
- Or run: .\evans.exe --proto .\greeting.proto

# Reference:

- https://github.com/nhatvu148/grpc-crud-csharp
