# Types

[Home](index.md)

## Important

All types are immutable, unless prefixed with:`mut`. i.e.
```
i32 immutableInt
mut i16 mutatableShortInt
```

## Basic Types

- `int` a 32bit integer.
- `float` a 64 bit floating point number.
- `void` a non value, i.e. return nothing.
- `bool` true or false value.

### Members

- `Size()` Size in bytes this type uses, example usage `bool.Size()` will return 1 and `void.Size()` will return 0.


## Numbers {#numbers}

| Name     | Type    | Size (in bytes) | Signed |
|----------|---------|-----------------|--------|
| `i8`     | integer | 1               | Yes    |
| `i16`    | integer | 2               | Yes    |
| `i32`    | integer | 4               | Yes    |
| `int`    | integer | 4               | Yes    |
| `i64`    | integer | 8               | Yes    |
| `u8`     | integer | 1               | No     |
| `u16`    | integer | 2               | No     |
| `u32`    | integer | 4               | No     |
| `u64`    | integer | 8               | No     |
| `f16`    | float   | 2               | Yes    |
| `f32`    | float   | 4               | Yes    |
| `f64`    | float   | 8               | Yes    |
| `float`  | float   | 8               | Yes    |

### Constants {#constants}

- `Min` Minimum value this type holds, example usage `i8.Min` will return -127.
- `Max` Maximum value this type holds, example usage `i16.Max` will return 32767.

Constants are values, so you don't need to add `()` to the name, i.e.

```
u8.Min       // will equal 0
f32 aFloat
aFloat.Max   // will equal approximately 3.4028235×10^38
```

### Members

- `Size()` Size in bytes this type uses, example usage `i32.Size()` will return 4.


## Strings {#strings}

- `str` ASCII encoded, null terminated string.
- `utf8` UTF8 encoded, null terminated string.

### Members

- `Size()` Will return the size in bytes the whole string occupies, including null terminator.
- `Count()` Will return character count string holds, `str.Count()` will return number of ASCII chars + 1 for the null terminator, the same as `str.Size()`. `utf8.Count()` could return a smaller value than `utf8.Size()` as some characters can require more than one byte to represent it, but will also include the null terminator.

Example usage:

```
str s = "Hello human, take me to your leader"
s.Size()    // will equal 36 (bytes)
s.Count()   // will equal 37 (bytes)
```

Note that members require an instance to work with.
