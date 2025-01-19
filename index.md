# Home

## Aim

Cy is a C like language - yes, yet another one. Its purpose is to provide easy to use, general purpose language that has built in multi-threading.
"Built in multi-threading" I hear you say, most languages do that, including C. Well they don't, they have libraries - often standard libraries - that provide the functionality to help with multi-threading. Cy on the other hand has it built in at the lowest level. The aim is to make multi-threaded coding so easy and intuitive that we use it automaticly, in a safe, fast and efficient manner.

Although Cy is a object oriented language, there is no requirement to use objects - often it will be easier though and some types/collections do provide methods to help use them.

## Types, Syntax and Semantics

- [Types](types.md)
- [Classes](classes.md)
- [Statements](statements.md)
- [Functions](functions.md)
- [Iterators](iterators.md)
- [Multithreading](multithreading.md)

## Discussion

Objects are a choice, not a requirement. You might enjoy them - at least occasionally - but you can write pure procedural code using Cy.

The Main function is the entry point and must be toplevel, i.e. not in an object.

Unit testing is built into the language, as well as the build environment.

## Notes

- Syntatic sugar is deliberatly kept to a minimum, we might use it later to simplify common commands.
- Members and Methods require an instance to work with.
- Constants can work with an instance or a type.
- Methods/Functions have a type, an identifier and must have at least `()` to denote they are a functional component.

