# `Tsu.Trees.RedTree`

This is a library to help implement the red/green tree pattern in your project without too much boilerplate.

## Red/Green Tree Pattern

The red/green tree pattern consists of a more efficient method of constructing immutable trees which are modified often, and you wish to avoid too many extra allocations.

It consists of two trees: the red tree and the green tree.

The green tree is an internal tree which only contains links to its children and the node information, it contains the core information of the node but can easily be re-parented without the need to allocate a new instance of it (as such, it can also be cached).

The red tree is the externally visible tree which holds the link to the parent node as well as the green node for the children information, these nodes are re-created often and as such cannot be easily cached since parents often change when you re-build a node with a different child.


## Features

What this library does is generate all the code for the green nodes as well as many helpers for the red nodes themselves, such as the `.ChildNodes()` method, `.EnumerateDescendants()`, `.WithX()` method for each child, `.IsEquivalent` and more.