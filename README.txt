Use this to easily create a constructor with fields sorted by length.
Just paste in your local member variables you want to create the constructor for:

A _a;
Eee _e;
Dd _d;
C _c;

results in: 

readonly A _a;
readonly C _c;
readonly Dd _d;
readonly Eee _e;

(
    A a, 
    C c, 
    Dd d, 
    Eee e)
{
    _a = a;
    _c = c;
    _d = d;
    _e = e;
}