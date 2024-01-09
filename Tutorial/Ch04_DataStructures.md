
# Lambda Programming Language Tutorial

## Table of Contents

- [Chapter 1: Introduction](Ch01_Introduction.md)
- [Chapter 2: Basic Features](Ch02_BasicFeatures.md)
- [Chapter 3: Modules](Ch03_Modules.md)
- **Chapter 4: Data Structures**

## Data Structures

### Summary of Topics

Within the *Lambda* programming language we can:

- Define a function

        >first  ,x ,y x ;
        >second ,x ,y y ;
        >
    
- Call a function

        >(first 'red' 'blue');
        red
        >(second 'red' 'blue');
        blue
        >

- Compare two values for equality

        >p 'true'  ;
        >q 'false' ;
        >r 'true'  ;
        
        >p = q ? 'red' : 'blue' ;
        blue
        >p = r ? 'red' : 'blue' ;
        red
        >
        
- Load and import modules

        >defs [definitions.txt] ;
        >defs.username ; 
        Batman
        >defs.password ;
        12345
        
        >:[library.txt]
        >(and (or p q) (and (not q) r)) ;
        true

That's really about it. That is the whole *Lambda* programming language in a nutshell. No arithmetic operators or numeric data types, no comparisons other than for equality, no flow control other than function calls and conditional expressions, no dedicated I/O functions - in fact, there are no built-in functions of any kind. 

That is the whole *Lambda* language.

It turns out, this is enough to build anything.

*Lambda* is probably the most minimal language - as close to pure lambda calculus as you can get - in which it is still possible to construct whatever abstractions and features you desire. 

We'll begin with the most humble of data structures - the pair. 

### Pair

The *pair* data structure is simple enough to define: 

    >first  ,x ,y x ;           ` Accepts two arguments and return the first.
    
    >second ,x ,y y ;           ` Accept stwo arguments and return the second.
    
    >pair ,x ,y ,f (f x y) ;    ` Accepts three arguments, calls the third one 
                                ` passing in the first two, and return the result. 

    >A (pair 'red' 'blue') ;
    
    >B (pair 'green' 'yellow') ;

In [Chapter 2: Basic Features](Ch02_BasicFeatures.md) we saw that lambda functions are *curried*. The function `pair` takes three arguments, but in our definitions for `A` and `B` we passed in only two. They are, in effect, waiting to be given that third argument.

    >(A first);
    red
    >(A second);
    blue
    >(B first);
    green
    >(B second);
    yellow
    >
    
We've just used `A` and `B` to store and recall structured information.

If you're familiar with Lisp, then you well know that the humble *pair* is a powerful data structure. From it we can construct linked lists, trees, graphs, maps, sets, stacks, queues - just about any abstract data structure you want!

It wasn't necessary to explicitly pass in the functions `first` and `second`. We could have passed in their corresponding lambda expressions directly and gotten the same result. 

    >(A ,x ,y x);
    red
    >(A ,x ,y y);
    blue
    >(B ,x ,y x);
    green
    >(B ,x ,y y);
    yellow
    >

### Linked Lists

With `pair` and a few extra definitions, we can now create a linked list and access its elements.

    >head ,p (p first) ;
    >tail ,p (p second) ;
    >nil '';
    >team_roster (pair 'Alice' (pair 'Bob' (pair 'Charles' (pair 'Debby' nil)))) ;
    
    >(head team_roster) ;
    Alice
    >(head (tail team_roster)) ;
    Bob
    >(head (tail (tail team_roster))) ;
    Charles
    >

### Strings

With symbols and linked lists, we can now emulate the *string* data type seen in other languages. The word "Hello", for example, can be encoded as:

    >message (pair 'H' (pair 'e' (pair 'l' (pair 'l' (pair 'o' nil))))) ;
    >message;
    Hello
    >
    
The *Lambda* interpreter recognizes strings in this form. It will print them out just like symbols and it even provides a short-hand for writing them.

    >message "Hello";
    >message;
    Hello
    >(head message);
    H
    >(head (tail message));
    e
    >

To be clear, the string `"Hello"` is fundamentally different from the symbol `'Hello'`. The string `"Hello"` is a linked list of the symbols `'H'`, `'e'`, `'l'`, `'l'`, and `'o'` whereas the symbol `'Hello'` is a plain atomic value which cannot be split apart into its individual characters. 

All of the same escape codes for symbols can be used with strings as well - `\t`, `\r`, `\n`, `\q`, `\Q`, `\b`, and `\u<digits>;`. 

### I/O

Console standard input in *Lambda* is nothing more than a lazily-evaluated string. Because of this, even though there are no built-in I/O functions like Python's *print()* and *input()*, we can nonetheless create an interactive user experience.

**interactive.txt**

    pair ,x ,y ,f (f x y);

    head ,p (p ,h ,t h);

    tail ,p (p ,h ,t t);

    nil '';

    str ,symbol
        (pair symbol nil);

    concat ,xs ,ys 
        xs = nil ? ys : 
        (pair (head xs) (concat (tail xs) ys)) ;
       
    main ,input 
        (concat "Interactive Lambda Example\n"
        (concat "Please enter your name: "
        (input ,x ,xs 
            (concat "Hello Mr. " 
            (concat (str x) 
                    ". How are you today?\n"))))) ;

Running the above script will yield the output:
    
    Interactive Lambda Example
    Please enter your name: Batman
    Hello Mr. B. How are you today?

The above example is, admittedly, a rather crude way to do I/O in *Lambda*, but it can be simplified a great deal. It is technically possible to build an IO library which would allow *interactive.txt* to be written as something like:

**simplified-io.txt**

    :[IO.txt]
    
    main (do
            "Interactive Lambda Example\n"
            "Please enter your name: "
            (read-line ,myname (do
                "Hello " myname ", how are you today?\n" 
            end))
        end);


As a fun experiment, try running this silly example:

**experiment.txt**

    main ,input input ;


### Lazy Evaluation Semantics

*Lambda*, like Haskell, is fully lazily evaluated. This makes it possible to, among other things, create infinite lists which are computed only on demand.

We can see this in the following shell session:

    Lambda Shell v1.0.0-202307161703
    
    >:[List.txt]    ` Import the definitions for pair, head, tail, and nil.
    
    >cycle-helper ,xs ,ys
            ys = nil ? (cycle-helper xs xs) :
            (pair (head ys) (cycle-helper xs (tail ys)));
            
    >cycle ,xs (cycle-helper xs xs);

This `cycle` function returns an infinitely long list containing the elements of the given list `xs` repeated over and over. Since it's lazily evaluated, we only get stuck in an infinite loop if we attempt to print the entire infinite list. 
    
    >message (cycle "ABC");
    >(head message);
    A
    >(head (tail message));
    B
    >(head (tail (tail message)));
    C
    >(head (tail (tail (tail message))));
    A
    >(head (tail (tail (tail (tail message)))));
    B
    >message;
    ABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABC
    ABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABC
    ABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABC
    ABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABC
    ABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABCABC
    ...

### Object Identity and Variadic Functions

*Lambda*'s conditional expression employs a notion of equaity similar to what you'd find in Java, Python, or C#. It is capable not only of comparing symbols, but of comparing *instances* of functions as well.

Consider the following example:

    Lambda Shell v1.0.0-202307161703
    >first ,x ,y x ;
    >second ,x ,y y ;
    >first = first ? 'equal' : 'not equal' ;
    equal
    >first = second ? 'equal' : 'not equal' ;
    not equal
    >    

We see above that the function `first` is indeed equal to itself. However, every time a lambda expression is evaluated, the result is a function unique from all others.

    Lambda Shell v1.0.0-202307161703
    >make-constant-function ,c ,x c ;
    >f (make-constant-function 'red');
    >g (make-constant-function 'red');
    >(f 123);
    red
    >(g 123);
    red
    >f = g ? 'equal' : 'not equal';
    not equal
    >

In the above example, even though both `f` and `g` are both constant functions which always return the symbol 'red' no matter what argument is given, they are not considered equal to each other since they were created by separate evaluations of the subexpression `,x c`. 

This behavior, combined with currying, makes it possible to define a function `$` for easily building lists of arbitrary length:

    >roster ($ Alice Bob Charles Debby Eve $);
    >(head roster);
    Alice
    >(head (tail roster));
    Bob
    >

The definition of this `$` function is as follows:

    >reverse-helper ,result ,items
        items = nil ? result :
        (reverse-helper (pair (head items) result) (tail items));

    >reverse ,items
        (reverse-helper nil items);    

    >$-helper ,result ,item
            item = $ ? (reverse result) :
            ($-helper (pair item result)) ;
            
    >$ ($-helper nil);

### Final Notes

It has been mentioned that *Lambda* does not provide any sort of built-in mathematics library or numeric data types, but this isn't a problem! Indeed, one of the sample programs provided with *Lambda*'s release is the following prime number generator:

    :[stdlib\IO.txt]
    :[stdlib\Lang.txt]
    :[stdlib\List.txt]
    :[stdlib\Math.txt]

    primes
        ((R ,primes-from ,current-value ,previous-primes
                (if (any? (divides? current-value) previous-primes)
                    (primes-from (++ current-value) previous-primes)
                    (pair current-value 
                         (primes-from (++ current-value) 
                                      (pair current-value previous-primes)))))
            (str-to-int "2")
            nil) ;

    main ,input (join ", " (map int-to-str primes)) ;

which when run prints out:

    >lambda.exe make-primes.txt
    2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 
    67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 
    139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 
    223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 
    293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 
    383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461,
    ...

Internally, Lambda's *Math* library performs arithmetic by implementing integers in binary form as lists of 1s and 0s, onto which various bitwise function can be performed.

We also see many other helpful functions in this example, such as `if`, `any?`, and `R` - the latter functioning similarly to the [Y combinator](https://en.wikipedia.org/wiki/Fixed-point_combinator) thereby allowing us to define "local" recursive helper functions.

Defining functions, calling functions, and comparing for equality - out of these bare minimum of features we've built a working, concise, and neatly written prime number generator. The functional programming paradigm brought us from esoteric minimalism to something almost practical. 

Exploring this phenomenon was the driving force behind creating *Lambda*. It is my hope that one day, students of theoretical computer science might use this language to help further their own study of lambda calculus and of the nature of computability. 

[< Prev](Ch03_Modules.md)
