# Classes

[Home](index.md)

Cy uses serveral types of classes, listed below:
- Model
- Config
- Transient
- Singleton
- Class


## Model {#model}

A model is a collection of properties *only*.

```
model MyModel {
    i8 SmallInteger
    i32 LargerInteger
}
```

To create an instance of a model:

```
MyModel myModel = MyModel {
    SmallInteger = 1,
    LargerInteger = 2
}
```


## Config {#config}

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


## Transient {#transient}
A standard class, but instances of this can be injected directly into your classes. Contains methods and properties.

```
transient MyTransient {
    i8 SmallInteger
    i32 LargerInteger

    MyTransient() {
        SmallInteger = 1
        LargerInteger = 100
    }

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

## Singleton {#singleton}

Again a standard class but a single instance of this can be injected directly into your classes. So if multiple classes ask for an injected instance they will each get a reference to the same instance. Contains methods and properties.


## Class {#class}

A standard class, contains methods and properties. A factory must be used to create an instance of this class. Class is the assumed class type, and hence has no requirement for the name class.

An unusal idiosyncrasy of class is the constructor has two input groups, the first is for user input and the second is for injected input. Only the first is required and can be empty.

```
transient ATransient {
    i32 LargerInteger
}

MyClass {
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
