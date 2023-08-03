
# Lambda Programming Language Tutorial

## Table of Contents

- [Chapter 1: Introduction](Ch01_Introduction.md)
- [Chapter 2: Basic Features](Ch02_BasicFeatures.md)
- **Chapter 3: Modules**  
    - [Hello, world!](#hello-world)
    - [Building A Library](#building-a-library)
    - [Module Path Expressions](#module-path-expressions)
    - [Dot Notation](#dot-notation)
    - [Imports](#imports)
    - [Importing On The Shell](#importing-on-the-shell)
- [Chapter 4: Data Structures](Ch04_DataStructures.md)

## Modules

### Hello, World!

In the last chapter, we saw how to use the shell to execute statements. In this chapter, we will organize our code into files, or *modules*. *Lambda*'s module system works similarly to that in Python, where each code file represents one module which can in turn import other modules.

*Lambda* code files do not have the expression statements we've seen in the shell. Rather, they consist solely of definitions and imports. When a code file is run, the result of its *main* function is printed.

We can write our "Hello, world!" application in a code file like so:


**hello.txt**

    main ,input 'Hello, world!' ;

To run this file, we simply run *Lambda.exe* on the command line, passing in the path to this file as an argument.

    >lambda.exe hello.txt
    Hello, world!

The `input` parameter of the main function represents our console's standard input as a lazily evaluated string. We will cover this in more detail in [Chapter 4: Data Structures](Ch04_DataStructures.md). 

### Building A Library

The `and`, `or`, and `not` functions defined in the prior chapter are useful constructs to have in any language. It would be helpful if we could define them just once and then simply refer to them as needed in our programs.

To do this, create a file named *library.txt* containing the definitions:

**library.txt**

    
    and ,x ,y 
        x = 'false' ? 'false' : 
        y = 'false' ? 'false' :
        'true' ;
        
    or ,x ,y 
        x = 'true' ? 'true' : 
        y = 'true' ? 'true' : 
        'false' ;
        
    not ,x 
        x = 'true' ? 'false' 
                   : 'true' ;


We can now use these functions in an example program like so:

**example-1.txt**

    Lib [library.txt] ;

    and (Lib 'and') ;
    or  (Lib 'or')  ;
    not (Lib 'not') ;
   
    p 'true'  ;
    q 'false' ;
    r 'true'  ;
    
    main ,input (and (or p q) (and (not q) r)) ;

We should now be able to run this program and see its output.

    >lambda.exe example-1.txt
    true

### Module Path Expressions

The relative path to a code file, enclosed in square brackets, is known as a *module path expression* and evaluates to a *module object* for that file. 

A module object is a function which, when given a symbol matching one of the identifiers defined within that code file, returns the associated value of that identifier. 

To better illustrate, consider the following code file:

**definitions.txt**

    username 'Batman' ;
    
    password '12345' ;
    

The expression `[definitions.txt]` will evaluate to the module object for this code file, which can then be called as if it were a function. The expression `([definitions.txt] 'username')` will evaluate to the symbol `'Batman'` and `([definitions.txt] 'password')` will evaluate to the symbol `'12345'`. 

In *example-1.txt* we defined the identifier `Lib` to refer to `[library.txt]` merely for brevity, to avoid having to keep writing out the whole module path expression. 

However, even if we had written:

    and ([library.txt] 'and') ;
    or  ([library.txt] 'or')  ;
    not ([library.txt] 'not') ;

the code file *library.txt* would still only have been loaded once and its corresponding module object cached. 

### Dot Notation

It is somewhat unwieldy to use function call notation to refer to the defined identifiers within a module. Dot notation is an alternative syntactic shorthand.

We can rewrite *example-1.txt* as:

**example-2.txt**

    Lib [library.txt] ;

    and Lib.and ;
    or  Lib.or  ;
    not Lib.not ;

    p 'true'  ;
    q 'false' ;
    r 'true'  ;

    main ,input (and (or p q) (and (not q) r)) ;

We can even use `Lib.and`, `Lib.or`, and `Lib.not` within the main function itself:

**example-3.txt**

    Lib [library.txt] ;
    
    p 'true'  ;
    q 'false' ;
    r 'true'  ;

    main ,input (Lib.and (Lib.or p q) (Lib.and (Lib.not q) r)) ;

Dot notation isn't specific to modules. Any expression of the form `foo.field` is automatically converted by the *Lambda* parser into the corresponding function call `(foo 'field')` before it is evaluated. Multiple fields chained together will be interpreted as multiple function calls. For example, `foo.field1.field2.field3` will be parsed as `(((foo 'field1') 'field2') 'field3')`.

### Imports

Lambda does have the equivalent to Python's `from library import *`. We simply prefix the module path expression with a colon. The file *example-3.txt* illustrated above can thus be rewritten as:

**example-4.txt**

    :[library.txt]
    
    p 'true'  ;
    q 'false' ;
    r 'true'  ;

    main ,input (and (or p q) (and (not q) r)) ;

Unlike Python's `from library import *`, however, this notation will *not* add `and`, `or`, and `not`to `example-4.txt`'s namespace. It will not be possible to access `and`, `or`, and `not` through the module returned by the path expression `[example-4.txt]`.

This rule helps to prevent polluting module namespaces and makes it possible for a public library to be able to import its own private helper library. 

### Importing On The Shell

The *Lambda* shell does support loading and importing modules, as illustrated below:

    Lambda Shell v1.0.0-202307161703
    >:[library.txt]
    >p true;
    >q false;
    >r true;
    >(and (or p q) (and (not q) r));
    true
    >

If you wish to run a code file's `main` function from the shell, such as that in *example-4.txt*, we can use:

    >([example-4.txt].main _STDIN_);
    true
    >    

The shell automatically defines the global variable `_STDIN_` to represent our console's standard input. This global variable isn't available in code files, however, since it isn't needed as the interpreter will automatically pass the standard input stream as an argument to `main`. 

The standard input stream will be covered more thoroughly in [Chapter 4: Data Structures](Ch04_DataStructures.md).

[< Prev](Ch02_BasicFeatures.md)   [Next >](Ch04_DataStructures.md)
