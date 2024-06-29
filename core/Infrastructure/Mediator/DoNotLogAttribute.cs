using System;

namespace Core.Infrastructure.Mediator;

[AttributeUsage(AttributeTargets.Class)]
public class DoNotLogAttribute : Attribute { }
