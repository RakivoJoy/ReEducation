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
            new Student { Id = 1, Namn = "Anna", Alder = 19, Klass = "A", Poang = 85 },
            new Student { Id = 2, Namn = "Bertil", Alder = 23, Klass = "B", Poang = 72 },
            new Student { Id = 3, Namn = "Cecilia", Alder = 42, Klass = "A", Poang = 91 },
            new Student { Id = 4, Namn = "David", Alder = 56, Klass = "C", Poang = 64 },
            new Student { Id = 5, Namn = "Eva", Alder = 19, Klass = "B", Poang = 85 },
            new Student { Id = 6, Namn = "Felix", Alder = 60, Klass = "A", Poang = 77 }
        };


    }
}

