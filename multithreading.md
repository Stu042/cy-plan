# Multithreading

[Home](index.md)

This is the real interesting bit...

## Main Function

True/internal definition of the `Main` function, code entry point.

```
i32 Main(str[] args) #0 {
}
```

When defining the `Main` function you don't need to add the `#0` but if you add `#1` or another value the compiler will error. The value represents the thread the function will run on and the `Main` function will always run on the first thread.

## Working With Threads

All thread statements start with `#`. To help working with threads the below statements are available:

- `#this` - this thread
- `#count` - count of available threads
- `#set 5` - must be only one instance in the program. Will ask the start-up code to create this amount of threads to be available for use.
- `#set default` - This will create the recommended amount of threads for the running architecture. Again must be only one instance of this in the program.

If X amount of threads are available and we ask code to run on thread X + 2, then thread 1 will run the code, i.e. (requested thread) % (thread count - 1).

