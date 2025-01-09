# Types

[Home](index)

## Basic Types
- `void` a none value, i.e. return nothing.
- `bool` true or false value.

## Numbers
| Name   | Type    | Size (in bytes) | Signed |
|--------|---------|-----------------|--------|
| `i8`   | integer | 1               | Yes    |
| `i16`  | integer | 2               | Yes    |
| `i32`  | integer | 4               | Yes    |
| `i64`  | integer | 8               | Yes    |
| `u8`   | integer | 1               | No     |
| `u16`  | integer | 2               | No     |
| `u32`  | integer | 4               | No     |
| `u64`  | integer | 8               | No     |
| `f16`  | float   | 2               | Yes    |
| `f32`  | float   | 4               | Yes    |
| `f64`  | float   | 8               | Yes    |

### Constants
- `Min` Minimum value this type holds, example usage `i8.Min` will return -127
- `Max` Maximum value this type holds, example usage `i16.Max` will return 32767

### Members
- `Size()` Size in bytes this type uses, example usage `i32.Size()` will return 4

## Strings
- `str` ASCII encoded, null terminated string.
- `utf8` UTF8 encoded, null terminated string.

### Members
- `Size()` Will return the size in bytes the whole string occupies, including null termination.
- `Count()` Will return character count string holds, `str.Count()` will return number of ASCII chars + 1 for the null terminater, the same as `str.Size()`. `utf8.Count()` could return a smaller value than `utf8.Size()` as some characters can require more than one byte to represent it, but will also include the null terminator.
