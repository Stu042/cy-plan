# Classes

[Home](index.md)

Cy uses serveral types of classes, listed below:
- Model
- Config
- Transient
- Singleton
- Class

## Model
A model is a collection of properties *only*.
```
model MyModel {
    i8 SmallInteger
    i32 LargerInteger
}
```

## Config
A config is a model that will have its properties set via a file at application launch. i.e.
```
config MyConfig {
    i8 SmallInteger
    i32 LargerInteger
}
```
Will have its properties set with a similarly named file:
my-config.settings.json
```
{
  "SmallInteger": 255,
  "LargerInteger", 9999
}
```

## Transient
A standard class, but instances of this can be injected directly into your classes. Contains methods and properties.

```
transient MyTransient {
    i8 SmallInteger
    i32 LargerInteger
    i32 Add() {
        return LargerInteger + SmallInteger
    }
}


transient MyOtherTransient {
    MyTransient MyTransient
    MyOtherTransient(MyTransient myTransient) {
        MyTransient = myTransient
    }
}
```

## Singleton
Again a standard class but a single instance of this can be injected directly into your classes. So if multiple classes ask for an injected instance they will each get a reference to the same instance. Contains methods and properties.

## Class
A standard class, contains methods and properties. A factory must be used to create an instance of this class.

```
transient ATransient {
    i32 LargerInteger
}

class MyClass {
    i8 SmallInteger
    i32 LargerInteger
    ATransient ATransient
    MyClass(i8 small, i32 larger)(ATransient aTransient) {
        return MyClass {
            SmallInteger = small,
            LargerInteger = larger,
            ATransient = aTransient
        }
    }
    i32 Add() {
        return LargerInteger + SmallInteger
    }
}


transient MyOtherTransient {
    MyClass MyClass
    MyTransient MyTransient
    MyTransient(MyClass myClass, MyTransient myTransient) {
        MyClass = myClass(1, 2)
        MyTransient = myTransient
    }
}
```
