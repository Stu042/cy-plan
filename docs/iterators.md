#  Iterators

[Home](index.md)

Create an iterator using the statement [`each`](statements.md#each).

```
i32[] b = [1, 2, 3, 4]
i32 a = each(b)
```

```
i32[] b = [1, 2, 3, 4]
i32 a = each(b)
i32 tot = a
a = a.Next()
tot += a
```

We can use it to loop over a collection like so:

```
i32[] b = [1, 2, 3, 4]
for (i32 a = each(b); a.Exists(); a = a.Next()) {
	// do something
}
```

Another *interesting*, but useless in this case, example is:

```
i32[] b = [1, 2, 3, 4]
i32 a = each(b)
if (a.Exists()) {
	// do something using a.Value()
	a = a.Next()
} while (a.Exists())
```

## Iterator Members

- Value() the value thsi iterator points to.
- Exists() true if the iterator points to a value.
- Next() return the iterator for the next value.
- Count() count of remaining values to iterate.


Here the `if` statement allows the code to enter the block, where it executes the `a.Next()` and then the `while` statement will loop back to the start of the block if true. Note it will not execute the `if` statement again as that is outside this code block.

