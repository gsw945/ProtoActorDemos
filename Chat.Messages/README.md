## 操作
- 下载文件 `https://github.com/AsynkronIT/protoactor-dotnet/blob/dev/src/Proto.Actor/Protos.proto` 保存到 `Proto.Actor\Protos.proto`
- 编写IDL文件 `Chat.proto`, 引用了 `Proto.Actor\Protos.proto`
- 添加NuGet包
  - `Grpc.Tools` 编译protobuf文件为C#代码文件
  - `Proto.Actor` 生成的C#代码, 引用了`Proto`命名空间下的`PID`类
    - `Google.Protobuf` 生成的C#代码, 引用了`Google.Protobuf`命名空间
- 编辑项目文件
  - `<Protobuf Include="Chat.proto" ProtoRoot="." OutputDir="$(ProjectDir)Generated" CompileOutputs="true" GrpcServices="none" />`
    - 所有的Protobuf文件, 生成C#代码, 但不参与编译
  - `<Protobuf Update="Chat.proto" CompileOutputs="true" />`
    - 白名单, `Chat.proto`文件参与编译
  - `<None Include="Generated\**" />`
    - `Generated`目录显示在解决方案资源管理器中
  - `<Compile Remove="Generated\**" />`
    - `Generated`目录不参与编译

## 参考
- [Protocol Buffers/gRPC Codegen Integration Into .NET Build](https://github.com/grpc/grpc/blob/master/src/csharp/BUILD-INTEGRATION.md)
- [常用的 MSBuild 项目属性](https://docs.microsoft.com/zh-cn/visualstudio/msbuild/common-msbuild-project-properties?view=vs-2019)