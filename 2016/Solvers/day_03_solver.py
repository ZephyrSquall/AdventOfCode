from solver import Solver

class Day03Solver(Solver):
    puzzle_title = "Squares With Three Sides"

    def solve_part_1() -> str:
        with open('PuzzleInputs/03.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        possible_triangle_count: int = 0

        for input_line in input_lines:
            input_values: list[int] = list(map(int, input_line.split()))
            input_values.sort()

            if (input_values[0] + input_values[1] > input_values[2]):
                possible_triangle_count += 1

        return str(possible_triangle_count)


    def solve_part_2() -> str:
        return "0"