using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqDemo;

// Enkel modell för kursen
public class Student
{
    public int Id { get; set; }
    public string Namn { get; set; } = "";
    public int Alder { get; set; }
    public string Klass { get; set; } = "";
    public int Poang { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        // Testdata
        List<Student> studenter = new()
        {
            new Student { Id = 1, Namn = "Abbe", Alder = 19, Klass = "B", Poang = 78 },
            new Student { Id = 2, Namn = "Berra", Alder = 23, Klass = "A", Poang = 76 },
            new Student { Id = 3, Namn = "Cecile", Alder = 42, Klass = "C", Poang = 99 },
            new Student { Id = 4, Namn = "Janne", Alder = 55, Klass = "B", Poang = 55 },
            new Student { Id = 5, Namn = "Erik", Alder = 55, Klass = "A", Poang = 34 },
            new Student { Id = 6, Namn = "Öjnos", Alder = 15, Klass = "C", Poang = 88 }
        };


        /**
         * Where
• Hämta alla studenter som är äldre än 20.
• Hämta alla studenter som har mer än 80 poäng.

**/
        var under20 = studenter.Where(s => s.Alder < 20).ToList();
        var over50 = studenter.Where(s => s.Alder > 50).ToList();

        Console.WriteLine("=== Young students only");
        foreach (var student in under20)
        {
            Console.WriteLine($"{student.Namn} ({student.Alder})");
        }

        Console.WriteLine("=== Old students only");
        foreach (var student in over50)
        {
            Console.WriteLine($"{student.Namn} ({student.Alder})");
        }

        /**
Select
• Visa bara namnen på alla studenter.
•**/

        var namesOnly = studenter.Select(s => s.Namn).ToList();

        Console.WriteLine("=== Names only");
        foreach (var student in namesOnly)
        {
            Console.WriteLine(student); // No Alder.
        }

        /**
        Count
• Räkna hur många studenter som finns.
• Räkna hur många som går i klass A
        **/

        var count = studenter.Count;
        Console.WriteLine($"There are {count} students.");


        var classA = studenter.Where(s => s.Klass.Equals("A")).ToList();

        var countA = classA.Count;
        Console.WriteLine($"There are {countA} students in Klass A.");

        /**
Take
• Ta ut de tre första studenterna.
• Ta ut de två bästa poängen efter sortering.
        **/

        Console.WriteLine("=== First three students.");
        var firstThree = studenter.Take(3).ToList();
        foreach (var student in firstThree)
        {
            Console.WriteLine($"{student.Namn} ({student.Alder})");
        }


        Console.WriteLine("=== Best two students.");
        var bestTwo = studenter.OrderByDescending(s => s.Poang).Take(2).ToList();
        foreach (var student in bestTwo)
        {
            Console.WriteLine($"{student.Namn} ({student.Alder}). P = {student.Poang}");
        }


        /**
        Distinct
• Visa alla unika klassnamn.
• Visa alla unika åldrar.
        **/

        Console.WriteLine("=== Distinct.");

        Console.WriteLine("== Klasser.");
        var classes = studenter.Select(s => s.Klass).Distinct().ToList();
        foreach (var klass in classes)
        {
            Console.WriteLine(klass);
        }

        Console.WriteLine("== Åldrar.");
        var ages = studenter.Select(s => s.Alder).Distinct().ToList();
        foreach (var age in ages)
        {
            Console.WriteLine(age);
        }

        /**


Any / All
• Finns det någon student med 100 poäng?
• Har alla studenter minst 50 poäng?
        **/

        var any100 = studenter.Any(s => s.Poang == 100);


        Console.WriteLine($"=== Any student with 100 points: {any100}");

        var all50 = studenter.All(s => s.Poang >= 50);
        Console.WriteLine($"=== All students with at least 50 points: {all50}");

        /**
FirstOrDefault / Last / Single
• Hämta första studenten i klass B.
• Hämta sista studenten i listan.
• Hämta studenten med ett visst Id.
        **/

        var firstB = studenter.FirstOrDefault(s => s.Klass.Equals("B"));
        Console.WriteLine($"=== First student in Klass B: {firstB?.Namn} ({firstB?.Alder})");

        var lastStudent = studenter.LastOrDefault();
        Console.WriteLine($"=== Last student in the list: {lastStudent?.Namn} ({lastStudent?.Alder})");

        var studentById = studenter.SingleOrDefault(s => s.Id == 1);
        Console.WriteLine($"=== Student with Id 1: {studentById?.Namn} ({studentById?.Alder})");

        /**
GroupBy
• Gruppera studenter per klass.
• Skriv ut varje klass och namnen i gruppen.
        **/

        var groupedByClass = studenter.GroupBy(s => s.Klass);
        foreach (var group in groupedByClass)
        {
            Console.WriteLine($"=== Klass {group.Key}");
            foreach (var student in group)
            {
                Console.WriteLine($"{student.Namn} ({student.Alder})");
            }
        }




        /**
Sum / Average / Min / Max
• Summera alla poäng.
• Räkna ut medelpoäng.
• Hitta lägsta och högsta poäng.

      **/
        Console.WriteLine("=== Stats.");

        var sum = studenter.Sum(s => s.Poang);
        Console.WriteLine("Sum = " + sum);

        var average = studenter.Average(s => s.Poang);
        Console.WriteLine("Average = " + Math.Round(average, 2));

        var min = studenter.Min(s => s.Poang);
        Console.WriteLine("Min = " + min);

        var max = studenter.Min(s => s.Poang);
        Console.WriteLine("Max = " + max);
    }
}

