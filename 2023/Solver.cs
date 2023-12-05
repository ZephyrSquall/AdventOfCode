namespace AdventOfCode2023;

abstract class Solver
{
    public abstract string PuzzleInputPath { get; }
    public abstract long SolvePart1();
    public abstract long SolvePart2();
}
