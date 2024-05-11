from typing import Final
import sys
import time
from solver import Solver
from Solvers.day_01_solver import Day01Solver
from Solvers.day_02_solver import Day02Solver
from Solvers.day_03_solver import Day03Solver
from Solvers.day_04_solver import Day04Solver

# Required to print box-drawing characters.
sys.stdout.reconfigure(encoding='utf-8')

DAY_TITLE: Final[str] = "Day"
PUZZLE_TITLE: Final[str] = "Puzzle"
PART_TITLE: Final[str] = "Part"
SOLUTION_TITLE: Final[str] = "Solution"
TIMING_TITLE: Final[str] = "Time (ms)"
solvers: dict[int, Solver] = {
    1: Day01Solver,
    2: Day02Solver,
    3: Day03Solver,
    4: Day04Solver,
}

class MaxLengths:
    max_day_length: int = len(DAY_TITLE)
    max_puzzle_length: int = len(PUZZLE_TITLE)
    max_part_length: int = len(PART_TITLE)
    max_solution_length: int = len(SOLUTION_TITLE)
    max_timing_length: int = len(TIMING_TITLE)

class DayResult:
    def __init__(self, day: int):
        self.day: int = day
        solver: Solver = solvers[day]
        self.puzzle_title: str = solver.puzzle_title

        start_time: float = time.time()
        self.solution_1: str = solver.solve_part_1()
        end_time: float = time.time()
        # Round time taken to 4 decimal places
        self.time_1: str = "{0:.4f}".format((end_time - start_time) * 1000)

        start_time: float = time.time()
        self.solution_2: str = solver.solve_part_2()
        end_time: float = time.time()
        # Round time taken to 4 decimal places
        self.time_2: str = "{0:.4f}".format((end_time - start_time) * 1000)

        if(len(self.puzzle_title) > MaxLengths.max_puzzle_length):
            MaxLengths.max_puzzle_length = len(self.puzzle_title)
        if(len(self.solution_1) > MaxLengths.max_solution_length):
            MaxLengths.max_solution_length = len(self.solution_1)
        if(len(self.solution_2) > MaxLengths.max_solution_length):
            MaxLengths.max_solution_length = len(self.solution_2)
        if(len(self.time_1) > MaxLengths.max_timing_length):
            MaxLengths.max_timing_length = len(self.time_1)
        if(len(self.time_2) > MaxLengths.max_timing_length):
            MaxLengths.max_timing_length = len(self.time_2)


def print_header():
    print(f"╔═{'':═>{MaxLengths.max_day_length}}═╤═{'':═>{MaxLengths.max_puzzle_length}}═╤═{'':═>{MaxLengths.max_part_length}}═╤═{'':═>{MaxLengths.max_solution_length}}═╤═{'':═>{MaxLengths.max_timing_length}}═╗")
    print(f"║ {DAY_TITLE:<{MaxLengths.max_day_length}} │ {PUZZLE_TITLE:<{MaxLengths.max_puzzle_length}} │ {PART_TITLE:<{MaxLengths.max_part_length}} │ {SOLUTION_TITLE:<{MaxLengths.max_solution_length}} │ {TIMING_TITLE:<{MaxLengths.max_timing_length}} ║")
    print(f"╟─{'':─>{MaxLengths.max_day_length}}─┼─{'':─>{MaxLengths.max_puzzle_length}}─┼─{'':─>{MaxLengths.max_part_length}}─┼─{'':─>{MaxLengths.max_solution_length}}─┼─{'':─>{MaxLengths.max_timing_length}}─╢")

def print_row(day_result: DayResult):
    print(f"║ {day_result.day:>{MaxLengths.max_day_length}} │ {day_result.puzzle_title:<{MaxLengths.max_puzzle_length}} │ {"1":>{MaxLengths.max_part_length}} │ {day_result.solution_1:>{MaxLengths.max_solution_length}} │ {day_result.time_1:>{MaxLengths.max_timing_length}} ║")
    print(f"║ {'':>{MaxLengths.max_day_length}} │ {'':>{MaxLengths.max_puzzle_length}} │ {"2":>{MaxLengths.max_part_length}} │ {day_result.solution_2:>{MaxLengths.max_solution_length}} │ {day_result.time_2:>{MaxLengths.max_timing_length}} ║")

def print_empty_row():
    print(f"║ {'':>{MaxLengths.max_day_length}} │ {'':>{MaxLengths.max_puzzle_length}} │ {'':>{MaxLengths.max_part_length}} │ {'':>{MaxLengths.max_solution_length}} │ {'':>{MaxLengths.max_timing_length}} ║")

def print_final_row():
    print(f"╚═{'':═>{MaxLengths.max_day_length}}═╧═{'':═>{MaxLengths.max_puzzle_length}}═╧═{'':═>{MaxLengths.max_part_length}}═╧═{'':═>{MaxLengths.max_solution_length}}═╧═{'':═>{MaxLengths.max_timing_length}}═╝")


day_results: list[DayResult] = []

if (len(sys.argv) == 1):
    for i in range(1, len(solvers) + 1):
        day_results.append(DayResult(i))
else:
    for i in range(1, len(sys.argv)):
        day_results.append(DayResult(int(sys.argv[i])))

print_header()
firstRow: bool = True
for i in range(len(day_results)):
    if (firstRow):
        firstRow = False
    else:
        print_empty_row()
    print_row(day_results[i])
print_final_row()