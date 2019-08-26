# Injecting context using a behavior

This sample demonstrates how to 'inject' some context user dependency injection per unit of work.

## Explanation

The NServiceBus context bag can hold objects that can be passed on to additional objects. It is fairly easy to write an [NServiceBus behavior](https://docs.particular.net/nservicebus/pipeline/manipulate-with-behaviors) to create an object, add it to the context bag and use that in handlers.

This isn't always a clean solution. In this sample we're using an object `UserContext` that holds authorisation rights for a user. Imagine this object needs to be passed around everywhere. This is discussed in the [migration guide](https://docs.particular.net/nservicebus/upgrades/5to6/moving-away-from-ibus#dependency-injection-accessing-message-handler-context-in-the-dependency-hierarchy) when upgrading to NServiceBus 6.

This sample shows how the object `UserContext` can be created and filled with data, retrieved by a repository. It can then be injected into other objects, like with the `MyDependency` class. After processing the message (either succesfully or not) the object is properly released and garbage collected.

## Notes

- The headers of a message can be altered inside a queue or by other means. Storing a User Identifier in plain text isn't the best idea.
- This sample is demoware and not production ready. It doesn't make use of interfaces or anything. Be aware to alter it to your needs.
- This sample is based on [a sample by Ramon Smits](https://github.com/ramonsmits/NServiceBus.InjectStorageContext) to inject an NHibernate storage context.