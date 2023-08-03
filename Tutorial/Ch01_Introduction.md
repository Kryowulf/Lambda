
# Lambda Programming Language Tutorial

## Table of Contents

- **Chapter 1: Introduction**
- [Chapter 2: Basic Features](Ch02_BasicFeatures.md)
- [Chapter 3: Modules](Ch03_Modules.md)
- [Chapter 4: Data Structures](Ch04_DataStructures.md)


## Introduction

*Lambda* is a minimalist, purely functional, lazily evaluated language inspired by Haskell and Lisp and is an almost direct executable implementation of [Lambda calculus](https://en.wikipedia.org/wiki/Lambda_calculus) with carefully chosen syntax and semantics. 

What makes *Lambda* interesting is how well it lends itself to building abstractions. Unlike [other minimalist languages](https://en.wikipedia.org/wiki/Brainfuck), *Lambda* is not really a [Turing tarpit](https://en.wikipedia.org/wiki/Turing_tarpit). Despite having only thee fundamental operations - defining a function, calling a function, and testing for equality - we can build a rich set of reusable data structures, algorithms, flow control constructs, I/O operations, and object-oriented primitives. 

Mathematical operations are intentionally left out - as they were in [McCarthy's 1959 implementation of Lisp](https://languagelog.ldc.upenn.edu/myl/llog/jmc.pdf) - since they too can be recreated within *Lambda* itself.

Code can be executed either in a shell with a read-eval-print loop (REPL), or by executing a file containing a `main` function. In the next chapter, we introduce *Lambda*'s basic features via the shell.

[Next >](Ch02_BasicFeatures.md)

