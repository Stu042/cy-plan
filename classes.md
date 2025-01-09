# Classes

[Home](index)

Cy uses serveral types of classes, listed below:
- Model
- Config
- Class
- Transient
- Singleton

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

## Class
A standard class, contains methods and properties. A factory must be used to create an instance of this class.

## Transient
Again a standard class but instances of this can be injected directly into your classes.

```
transient MyTransient {
    i8 SmallInteger
    i32 LargerInteger
}

transient MyOtherTransient {
    MyTransient MyTransient
    MyOtherTransient(MyTransient myTransient) {
        MyTransient = myTransient
    }
}
```

## Singleton
Again a standard class but a single instance of this can be injected directly into your classes. So if multiple classes ask for an injected instance they will each get a reference to the same instance.

