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

```
if (a==b) {         // no `do` which means we can do interesting things...
	// do something
} while (a==b)
```

## For {#for}

For statement format:

```
for (mut i32 a = 0; a < 5; a++) {
	// do something
}
```

## Each {#each}

Each gives us an [iterator](iterators.md) to iterate over a collection.
