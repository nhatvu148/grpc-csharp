# Add this ItemGroup when initiating a new project:

```
<ItemGroup>
    <Protobuf Include="../*.proto" OutputDir="%(RelativePath)models" />
</ItemGroup>
```
