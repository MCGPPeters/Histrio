using System;

namespace Histrio
{
    public interface IContainer : IDisposable
    {
        // Summary:
        //     Gets a value of the specified type from the container, optionally registered
        //     under a key.
        //
        // Parameters:
        //   key:
        //     The key.
        //
        // Type parameters:
        //   T:
        T Get<T>(string key = null) where T : class;

        //
        // Summary:
        //     Determines whether an instance of this type is registered.
        //
        // Type parameters:
        //   T:
        bool IsRegistered<T>() where T : class;

        //
        // Summary:
        //     Determines whether an instance of this type is registered.
        //
        // Parameters:
        //   type:
        //     The type.
        bool IsRegistered(Type type);

        //
        // Summary:
        //     Registers a type to the container. For example, autofac cannot create objects
        //     until you register them This allows containers like autofac to create the
        //     object.
        //
        // Type parameters:
        //   T:
        void RegisterType<T>() where T : class;

        // Summary:
        //     Sets a value in the container, so that from now on, it will be returned when
        //     you call IContainer.Get<T0>(System.String)
        //
        // Parameters:
        //   valueToSet:
        //     The value to set.
        //
        //   key:
        //     The key.
        //
        // Type parameters:
        //   T:
        T Set<T>(T valueToSet, string key = null) where T : class;
    }
}