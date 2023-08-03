
# Lambda Programming Language Tutorial

## Table of Contents

- [Chapter 1: Introduction](Ch01_Introduction.md)
- **Chapter 2: Basic Features**  
    - [Symbols](#symbols)
    - [Definitions](#definitions)
    - [Conditionals](#conditionals)
    - [Lambdas](#lambdas)
- [Chapter 3: Modules](Ch03_Modules.md)
- [Chapter 4: Data Structures](Ch04_DataStructures.md)

## Basic Features

Launching *Lambda* directly will open it in its shell mode, through which we can enter any definition or expression.

    Lambda Shell v1.0.0-202307161703
    >
    
There are three kinds of statements recognized by the shell - expression statements, definitions, and module imports.

Every expression evaluates to an object having one of four possible data types - a symbol, a lambda function, a module, or the standing input stream.

We'll hold off on covering modules and module imports until [Chapter 3: Modules](Ch03_Modules.md). The standard input stream will be covered in [Chapter 4: Data Structures](Ch04_DataStructures.md).

### Symbols

Symbols in *Lambda* are akin to those in *Lisp*. They are indivisible tokens than can be printed or compared for equality. We can print out the symbol 'Hello, world!' like so:

    Lambda Shell v1.0.0-202307161703
    >'Hello, world!';
    Hello, world!
    >

A symbol is written by enclosing any sequence of characters inside a pair of apostrophes, except for line breaks and other apostrophes. Escape sequences are available, however, for including whatever characters are desired in the symbol.

- `\t` - tab
- `\r` - carriage return 
- `\n` - line feed
- `\q` - apostrophe / single quote 
- `\Q` - double quote 
- `\b` - backslash
- `\u<digits>;` - unicode character with decimal code point. For example, `\u955;` gives the character `λ`.

Thus, we can write:

    Lambda Shell v1.0.0-202307161703
    >'\u9786; Hello,\nworld! \u9786;';
    ☺ Hello,
    world! ☺
    >

The empty symbol `''` in particular is used heavily in *Lambda* as a terminator, an empty list, or as a way to represent null values. It is akin to `nil` in *Lisp*. 

### Definitions

We may define an identifier to refer to the result of any expression using the notation:

    identifier expression ;
    
For example, to define the identifier `message` to refer to the symbol `'Hello, world!'` we simply write:

    Lambda Shell v1.0.0-202307161703
    >message 'Hello, world!';
    > 

From this point onward, `message` will refer to that symbol. 

    Lambda Shell v1.0.0-202307161703
    >message 'Hello, world!';
    >message;
    Hello, world!
    >

Identifiers are made up of any sequence of characters except groupings `()[]{}`, delimiters `;:,.`, single `'` or double `"` quotes, backticks `` ` ``, and whitespace. 

This means that `x`, `foo`, `123`, and even `<=` are all valid identifiers which can be defined to refer to any object. 

Any identifier which has not been previously defined will evaluate to a symbol of itself. 

    Lambda Shell v1.0.0-202307161703
    >foo;
    foo             ` Initially, foo evaluates to the symbol 'foo'.
    >foo 'hello';   ` Then we define foo to refer to the symbol 'hello'.
    >foo;
    hello           ` Now foo evaluates to 'hello'.
    >

Comments are denoted by the backtick `` ` `` operator. 

### Conditionals

*Lambda*'s conditional expressions use a notation similar to C's ternary operator, except the only condition we can check for is equality `=`. 

    expression = expression ? expression : expression 
    
To illustrate, we begin by defining a few identifiers:

    Lambda Shell v1.0.0-202307161703
    >p true;
    >q false;
    >r true;

We can now use the ternary operator to check for equality between p, q, and r.

    >p = q ? red : blue ;
    blue
    >p = r ? red : blue ;
    red
    >    

It should be noted that the spaces surrounding `=` and `?` are necessary since those characters are valid within identifiers. 

### Lambdas

Lambda expressions within *Lambda* work much like those in Python and C#. They are anonymous functions which can be defined in-line as part of a larger statement.

The notation to define a lambda function is

    , identifier expression
    
To illustrate, we can use a lambda and a conditional to define the boolean `not` function like so:

    >not ,x x = 'true' ? 'false' : 'true' ;

In this example, the identifier `not` is defined to be a function of parameter `x` which returns `'false'` if `x` is equal to `'true'`, and returns `'true'` otherwise.

To call our new function, we use a notation simlar to Lisp-style S-expressions. 

    >(not 'true');      ` We can pass in the symbols 'true' 
    false               ` and 'false' directly. 
    >(not 'false');     
    true
    >p 'true';
    >q 'false';
    >(not p);           ` We can pass in identifiers 
    false               ` representing those symbols. 
    >(not q);           
    true
    >(not (not p));     ` We can pass in the result of
    >true               ` nested expressions. 
    >(not (not q));     
    >false
    >
    
Although functions in *Lambda* always have exactly one parameter, this isn't a limitation because functions can seamlessly return other functions! We can illustrate this by defining the boolean `and` and `or` functions like so:

    >and ,x ,y
            x = 'false' ? 'false' :
            y = 'false' ? 'false' :
            'true' ;
    
    >or ,x ,y
        x = 'true' ? 'true' :
        y = 'true' ? 'true' :
        'false' ;

In this example, the identifier `and` is defined to be a function of parameter `x`, which returns another function of parameter `y`. This other function uses nested conditional expressions to return `'false'` if either `x` or `y` are equal to `'false'`, and returns `'true'` otherwise. The identifier `or` is defined similarly. 

We can now call our new `and` function like so:

    >((and 'true') 'true');
    true
    >((and 'true') 'false');
    false
    >((and 'false') 'true');
    false
    >((and 'false') 'false');
    false
    >

In *ANSI Common Lisp* the syntax of a function call is `(identifier expression expression expression ...)`. In *Lambda*, however, a function doesn't have to be referred to by name in order to be called. Instead, an expression whose result is a function can be called directly.

Thus, the syntax of a function call in *Lambda* is `(expression expression)` as we see in the example above.

Admittedly, requiring all function calls to always have exactly one argument does seem unwieldy. Therefore, *Lambda* provides a "syntactic surgar" to give the appearance of passing in multiple arguments to a function. The above calls to `and` can be rewritten as:

    >(and 'true' 'true');
    true
    >(and 'true' 'false');
    false
    >(and 'false' 'true');
    false
    >(and 'false' 'false');
    false
    >

To be clear, whenever we write a function call in the form `(foo a b c d)` it will automatically be converted by *Lambda*'s parser into the form `((((foo a) b) c) d)` before it's evaluated.

If you're familiar with the *Haskell* language, you may recognize this technique as [currying](https://wiki.haskell.org/Currying).

[< Prev](Ch01_Introduction.md)   [Next >](Ch03_Modules.md)
