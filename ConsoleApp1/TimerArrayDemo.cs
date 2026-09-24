using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TimerArrayDemo
{
    internal enum ArrayAction
    {
        Reset,
        NewNumbers,
        Rearrange,
        SortAscending,
        SortDescending
    }

    internal class TimerArray
    {
        private const int ArraySize = 5;
        private const double MinInterval = 3.0;
        private const double MaxInterval = 5.0;

        private readonly Random random;
        private readonly int[] initialNumbers;
        private double timeUntilAction;

        public int Id { get; }
        public int[] Numbers { get; private set; }

        public event Action<TimerArray, ArrayAction>? ActionPerformed;

        public TimerArray(int id, Random random)
        {
            Id = id;
            this.random = random;
            initialNumbers = CreateRandomNumbers();
            Numbers = initialNumbers.ToArray();
            timeUntilAction = NextInterval();
        }

        public void Update(double deltaSeconds)
        {
            timeUntilAction -= deltaSeconds;

            if (timeUntilAction > 0)
            {
                return;
            }

            timeUntilAction = NextInterval();

            ArrayAction[] actions = Enum.GetValues<ArrayAction>();
            ArrayAction action = actions[random.Next(actions.Length)];

            Perform(action);
            ActionPerformed?.Invoke(this, action);
        }

        private void Perform(ArrayAction action)
        {
            switch (action)
            {
                case ArrayAction.Reset:
                    Numbers = initialNumbers.ToArray();
                    break;
                case ArrayAction.NewNumbers:
                    Numbers = CreateRandomNumbers();
                    break;
                case ArrayAction.Rearrange:
                    Numbers = Numbers.OrderBy(_ => random.Next()).ToArray();
                    break;
                case ArrayAction.SortAscending:
                    Numbers = Numbers.OrderBy(n => n).ToArray();
                    break;
                case ArrayAction.SortDescending:
                    Numbers = Numbers.OrderByDescending(n => n).ToArray();
                    break;
            }
        }

        private int[] CreateRandomNumbers()
        {
            return Enumerable.Range(0, ArraySize)
                .Select(_ => random.Next(1, 100))
                .ToArray();
        }

        private double NextInterval()
        {
            return MinInterval + random.NextDouble() * (MaxInterval - MinInterval);
        }
    }

    internal class TimerArrayManager
    {
        private const int MaxTimerArrays = 10;
        private const double SpawnInterval = 2.0;

        private readonly List<TimerArray> timerArrays = new();
        private readonly Random random = new();
        private double spawnTimer;
        private int nextId = 1;

        public void Update(double deltaSeconds)
        {
            spawnTimer += deltaSeconds;

            if (spawnTimer >= SpawnInterval && timerArrays.Count < MaxTimerArrays)
            {
                spawnTimer = 0;
                Spawn();
            }

            foreach (TimerArray timerArray in timerArrays)
            {
                timerArray.Update(deltaSeconds);
            }
        }

        private void Spawn()
        {
            TimerArray timerArray = new(nextId++, random);
            timerArray.ActionPerformed += OnActionPerformed;
            timerArrays.Add(timerArray);

            Console.WriteLine(
                $"[Timer {timerArray.Id}] Created ({timerArrays.Count}/{MaxTimerArrays}): {Format(timerArray.Numbers)}"); // Interpolated string
        }

        private void OnActionPerformed(TimerArray timerArray, ArrayAction action)
        {
            Console.WriteLine(
                $"[Timer {timerArray.Id.ToString().PadLeft(2)}] {action.ToString().PadRight(16)}: {Format(timerArray.Numbers)}");
        }

        private string Format(int[] numbers)
        {
            return string.Join(", ", numbers.Select(n => n.ToString().PadLeft(2)));
        }
    }

    internal class Program
    {
        private readonly TimerArrayManager manager = new();
        private bool running = true;

        private static void Main()
        {
            new Program().Run();
        }

        private void Run()
        {
            Console.WriteLine("Press Esc to quit.");

            Stopwatch stopwatch = Stopwatch.StartNew();
            TimeSpan lastTime = stopwatch.Elapsed;

            while (running)
            {
                TimeSpan currentTime = stopwatch.Elapsed;
                double deltaSeconds = (currentTime - lastTime).TotalSeconds;
                lastTime = currentTime;

                manager.Update(deltaSeconds);
                HandleInput();
            }
        }

        private void HandleInput()
        {
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                running = false;
            }
        }
    }
}
