# CYK 算法 C# 实现

```C#
namespace CYKSharp
{
    using System;
    class Program
    {
        static void Main(string[] args)
        {
            var g = @"S -> NP VP
                    NP -> DET N
                    NP -> NP PP
                    PP -> P NP
                    VP -> V NP
                    VP -> VP PP
                    DET -> the
                    NP -> I
                    N -> man
                    N -> telescope
                    P -> with
                    V -> saw
                    N -> cat
                    N -> dog
                    N -> pig
                    N -> hill
                    N -> park
                    N -> roof
                    P -> from
                    P -> on
                    P -> in";
            var s = "I saw the man with the telescope";
            var accepted = new CYKAlgorithm(g).Accept(s);
            if (accepted)
            {
                Console.WriteLine("The sentence \"{0}\" is accepted by the language!", s);
            }
            Console.Read();
        }
    }
}
```

此方法CYK算法的实现，是来之 http://yyl20020115.blog.163.com/blog/static/234684220201441435513647/
