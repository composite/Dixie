# Dixie
Make wings with dependency injection on .NET Core!

Dixie means:

> **D**ependency<br>
> **I**njection<br>
> e**X**tensible<br>
> **I**nterface<br>
> **E**lement

Don't be obstinate, myself!

## Key features

- `Dixie.Microsoft`: The convention extension for `Microsoft.Extensions.DependencyInjection`
    - Attribute-driven service handler like Spring's Annotation-driven.
    - Lazy service via `ILazy<T>`
    - Decorator pattern

## Install

nuget coming soon.

## Requirements

### `Dixie.Microsoft`

`.NET Standard 1.6`

- `Microsoft.Extensions.DependencyInjection`
- `Microsoft.Extensions.DependencyInjection.Abstractions`
- `Microsoft.Extensions.DependencyModel`
- `Microsoft.Extensions.Logging`
- `NETStandard.Library` ???

### `Dixie.Test`

`.NET Core App 1.0`

- `xunit`
- `dotnet-test-xunit`

## How to use

### Attribute-driven

1. Register a service with `ServiceAttribute`.
   
   ```cs
   using global::Dixie.Microsoft.Attribute;

   [Service]
   public class MyService
   {
     public string Foo()
     {
        return "Bar";
     }
   }
   ```

2. Activate Attribute driver in `Startup.cs`

    ```cs
    using global::Dixie.Microsoft;

    //...

    public void ConfigureServices(IServiceCollection services)
    {
      // search all attribute-driven services
      // in your project's entire type, then add'em all.
      services.AddServiceScan(this.GetType());
    }

    //...
    ```

3. Use. like plain DI context.
    
    ```cs
    public SampleDILoadClass(MyService myservice)
    {
      this.mystring = myservice.Foo();
    }

    // or

    public string SampleDILoadMethod()
    {
      // get service via IServiceProvider like normal usage.
      MyService myservice = provider.GetService<MyService>();
      return myservice.Foo();
    }
    ```

### Register service as Lazy

```cs
using global::Dixie.Microsoft.Attribute;

[Lazy, Service]
public class MyLazyService
{
  public string Foo()
  {
    return "Bar";
  }
}
```

```cs
public string SampleDILoadMethod()
{
  // get service via IServiceProvider like normal usage.
  // but, ILazy<T> will be wrapped when getting service.
  ILazy<MyLazyService> mylazy = provider.GetService<ILazy<MyLazyService>>();
  MyLazyService myservice = mylazy.Value;
  return myservice.Foo();
}
```

### Decorator pattern

```cs
using global::Dixie.Microsoft.Attribute;

public interface IMyService
{
  string Foo();
}

[Service]
public class MyPlainService : IMyService
{
  public string Foo()
  {
    return "Bar";
  }
}

[Decorator]
public class MyDecoratorService : IMyService
{
  private ILogger log;
  private readonly IMyService deco;

  public MyDecoratorService(ILoggerFactory logger, IMyService service)
  {
    this.log = logger.CreateLogger(GetType());
    this.deco = service;
  }

  public string Foo()
  {
    log.LogInfomation("before calling decorator MyService.");
    string result = deco.Foo();
    log.LogInfomation("after calling decorator MyService.");
    return "Decorated " + result;
  }
}
```

```cs
public string SampleDILoadMethod()
{
  // get service via IServiceProvider like normal usage.
  MyLazyService myservice = provider.GetService<MyLazyService>();
  return myservice.Foo();
}
```

### Manual usage

```cs
using global::Dixie.Microsoft;

//...

public void ConfigureServices(IServiceCollection services)
{
    // normal usage.
    service.AddSingleton<ISingletonService, SingletonService>();

    // lazy usage.
    service.AddLazySingleton<ILazySingletonService, LazySingletonService>();

    // decorator usage.
    service.AddServiceDecorator<IMyExistingService, MyDecoratorService>();
}

//...
```

## Known issues

- Attribte-driven with open-generic types are not supported. and I don't have a plan with it yet.
  ```
  [Service]
  public class MyService<T> // it will be ignored.
  {
    //...
  }
  ```

- Adding manual usage with open-generic types are not supported **yet**.
- `IDisposable` types usage coming soon.

## TODO

- `IDisposable`
- Manual usage with open-generic type.

## Get involved

`Dixie` is wanna involved your hand, contribute, report bugs, suggestions, request features are welcome!

Feel free and post its issues you want.

## License

Apache License 2.0