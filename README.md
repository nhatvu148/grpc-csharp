# Add this ItemGroup when initiating a new project:

```
<ItemGroup>
    <Protobuf Include="../*.proto" OutputDir="%(RelativePath)models" />
</ItemGroup>
```

# gRPC error codes:

- https://github.com/grpc/grpc/blob/master/doc/statuscodes.md
