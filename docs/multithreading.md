# Multithreading

[Home](index.md)

This is the real interesting bit...

## Main Function

True/internal definition of the `Main` function, code entry point.

```
#0
i32 Main(str[] args) {
}
```

When defining the `Main` function you don't need to add the `#0` but if you add `#1` or another value the compiler will error. The value represents the thread the function will run on and the `Main` function will always run on the first thread.

## Working With Threads

All thread statements start with `#`. To help working with threads the below statements are available:

- `#this` - this thread
- `#count` - count of available threads
- `#set 5` - must be only one instance in the program. Will ask the start-up code to create this amount of threads to be available for use.
- `#set default` - This will create the recommended amount of threads for the running architecture. Again must be only one instance of this in the program.

If X amount of threads are available and we ask code to run on thread X + 2, then thread 1 will run the code, i.e. (requested thread) % (thread count). So if we ask for 3 threads for the app, we will have thread #0, #1, #2 ready for use. Ask to use #3 and we will end up with #3 % #3 = #0

You can use code to decide which thread to assign to a function, i.e.

```
// Random() is a user generated function, returns an int, 0 <= value < inputted value
#(Random(#count))
void MyFunc() {
}
```
Every time this function is called the `#{}` code block will be executed to decide which thread to use. Be careful here, this adds a delay before running or assigning this function to its thread - it could cause performance issues.

Simpler example:

```
#(#count - 1)
void MyFunc() {
}
```

Here the last thread will be used, expressions don't require a code block.

Even if a function has been assigned a thread to use, at call time you can ask the function to run on a specified thread, i.e.

```
#(Random(#count))
void MyFunc() {
}

MyFunc() #this
```

In this example the `#{}` code block will not be executed and `MyFunc()` will run synchronously on the same thread we are currently using.

