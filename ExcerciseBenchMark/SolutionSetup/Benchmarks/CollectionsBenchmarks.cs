using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using SolutionSetup.Generators;
using SolutionSetup.Models;

namespace SolutionSetup.Benchmarks
{
    /// <summary>
    /// Compares List, Dictionary and HashSet for Add, Lookup, Remove and Iterate across data sizes of 100, 10,000 and 100,000.
    /// </summary>
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class CollectionsBenchmarks
    {
        /// <summary>Number of elements in each collection.</summary>
        [Params(100, 10_000, 100_000)]
        public int N;

        private List<Person> _source = [];
        private List<Person> _list = [];
        private Dictionary<int, Person> _dict = [];
        private HashSet<Person> _set = [];
        private Person _target = null!;

        /// <summary>Builds the source data and pre-populated fixtures once per parameter value, so population cost is not timed.</summary>
        [GlobalSetup]
        public void Setup()
        {
            _source = PersonGenerator.Generate(N, seed: 42);
            _list = new List<Person>(_source);
            _dict = _source.ToDictionary(p => p.Id);
            _set = new HashSet<Person>(_source);
            // Middle element is used as the lookup/remove target so it is always present and not at an edge.
            _target = _source[N / 2];
        }

        /// <summary>Baseline for relative comparison.</summary>
        [Benchmark(Baseline = true)]
        public List<Person> List_Add()
        {
            var list = new List<Person>();
            foreach (var p in _source) list.Add(p);
            return list;
        }

        /// <summary>Adds all elements to a new Dictionary keyed by Id.</summary>
        [Benchmark]
        public Dictionary<int, Person> Dictionary_Add()
        {
            var dict = new Dictionary<int, Person>();
            foreach (var p in _source) dict.Add(p.Id, p);
            return dict;
        }

        /// <summary>Adds all elements to a new HashSet.</summary>
        [Benchmark]
        public HashSet<Person> HashSet_Add()
        {
            var set = new HashSet<Person>();
            foreach (var p in _source) set.Add(p);
            return set;
        }

        /// <summary>Linear scan lookup (O(n)).</summary>
        [Benchmark]
        public bool List_Lookup() => _list.Contains(_target);

        /// <summary>Hash-based key lookup (O(1) average).</summary>
        [Benchmark]
        public bool Dictionary_Lookup() => _dict.ContainsKey(_target.Id);

        /// <summary>Hash-based membership lookup (O(1) average).</summary>
        [Benchmark]
        public bool HashSet_Lookup() => _set.Contains(_target);

        // Remove benchmarks copy the fixture first: removing from a shared instance would change it for later iterations.
        // The copy cost is included equally in all three Remove benchmarks, so the comparison stays fair.

        /// <summary>Copies the list, then removes the target (O(n) scan + shift).</summary>
        [Benchmark]
        public bool List_Remove() => new List<Person>(_list).Remove(_target);

        /// <summary>Copies the dictionary, then removes the target key.</summary>
        [Benchmark]
        public bool Dictionary_Remove() => new Dictionary<int, Person>(_dict).Remove(_target.Id);

        /// <summary>Copies the set, then removes the target.</summary>
        [Benchmark]
        public bool HashSet_Remove() => new HashSet<Person>(_set).Remove(_target);

        // Iterate benchmarks return a value so the JIT cannot eliminate the loop as dead code.

        /// <summary>Sums salaries while iterating the list.</summary>
        [Benchmark]
        public decimal List_Iterate()
        {
            decimal total = 0;
            foreach (var p in _list) total += p.Salary;
            return total;
        }

        /// <summary>Sums salaries while iterating the dictionary values.</summary>
        [Benchmark]
        public decimal Dictionary_Iterate()
        {
            decimal total = 0;
            foreach (var p in _dict.Values) total += p.Salary;
            return total;
        }

        /// <summary>Sums salaries while iterating the set.</summary>
        [Benchmark]
        public decimal HashSet_Iterate()
        {
            decimal total = 0;
            foreach (var p in _set) total += p.Salary;
            return total;
        }
    }
}
