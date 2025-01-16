#  Statements

[Home](index.md)

## If {#if}

`if` can be used with an expression for the usual condition check.

```
if (a==b) {
	// do something
}
```

## Else {#else}

`else` also exists.

```
if (a==b) {
	// do something
} else {
	// do something else
}
```

Other variations:

```
if (a==b) {
} else {
	// do something else
}
```

```
if (a==b) {
	// do something
} else if (a > b) {
	// do something else
}
```

## While {#while}

`while` loop until the condition is false.

```
while (a==b) {
	// do something
}
```

Also:

```
{					// do we really need a `do` here?
	// do something
} while (a==b)
```

## For {#for}

For statement format:

```
for (i32 a = 0; a < 5; a++) {
	// do something
}
```

## Each {#each}

Each gives us an [iterator](iterators.md) over a collection.

```
i32[] b = [1, 2, 3, 4]
i32 a = each(b)
i32 tot = a
a.Next()
tot += a
```

We can use it to loop over a collection like so:

```
i32[] b = [1, 2, 3, 4]
for (i32 a = each(b); a.Exists(); a.Next()) {
	// do something
}
```

Another, *interesting* example is:

```
i32[] b = [1, 2, 3, 4]
i32 a = each(b)
if (a.Exists()) {
	// do something
	a.Next()
} while (a.Exists())

```

Here the `if` statement allows the code to enter the block, where it executes the `a.Next()` and then the `while` statement will loop back to the start of the block if true. Note it will not execute the `if` statement again as that is outside this code block.

