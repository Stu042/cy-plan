# Functions

[Home](index.md)

## Basic Function

This is a very simple function that does nothing:

```
void MyFirstFunction() {
}
```

The function definition is made up of a type, `void`, an identifier, `MyFirstFunction` and an empty input list denoted as `()`. Finaly the block for the code is defined starting with `{` and ending with `}`.

There can only be one top level function named `Main` in a project, as the `Main` function is the entry point for the application.

The following functions are examples of the `Main` function:

```
// No value returned, no input gathered
void Main() {
}

// A value is returned, 42 in this case, no input gathered
i32 Main() {
    return 42
}

// A value is returned, 0 in this case, input is gathered from the command line
i32 Main(str[] args) {
    return 0
}

// No value returned, input is gathered from the command line
void Main(str[] args) {
}
```

Note that for `str[] args` the count of args can be obtained using, `args.Count()` and you can iterate over the collection using, `str arg = each(args)`. See [Iterators](iterators.md) for more information.

