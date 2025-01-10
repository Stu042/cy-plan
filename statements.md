#  Statements

## If

`if` can be used with an expression for the usual condition check.

```
if(a==b) {
	// do something
}
```

## Else

`else` also exists.

```
if(a==b) {
	// do something
} else {
	// do something else
}
```

Other variations:

```
if(a==b) {
} else {
	// do something else
}
```

```
if(a==b) {
	// do something
} else if (a > b) {
	// do something else
}
```

## While

`while` loop until the condition is false.

```
while(a==b) {
	// do something
}
```

Also:

```
{					// do we really need a `do` here?
	// do something
} while(a==b)
```

## For

For statement format:

```
for(i32 a = 0; a < 5; a++){
	// do something
}
```

## Each

Each gives us an interator over a collection.

```
i32[] b = [1, 2, 3, 4]
i32 a = each b
```

