from solver import Solver

class Day03Solver(Solver):
    puzzle_title = "Squares With Three Sides"

    @staticmethod
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


    @staticmethod
    def solve_part_2() -> str:
        with open('PuzzleInputs/03.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        possible_triangle_count: int = 0

        for i in range(len(input_lines) // 3):
            triangle_trios: list[list[int]] = [[], [], []]
            for j in range(3):
                input_line = input_lines[i*3 + j]
                input_values: list[int] = list(map(int, input_line.split()))
                triangle_trios[0].append(input_values[0])
                triangle_trios[1].append(input_values[1])
                triangle_trios[2].append(input_values[2])
            for triangle_trio in triangle_trios:
                triangle_trio.sort()
                if (triangle_trio[0] + triangle_trio[1] > triangle_trio[2]):
                    possible_triangle_count += 1

        return str(possible_triangle_count)