from solver import Solver
from hashlib import md5

class Day05Solver(Solver):
    puzzle_title = "How About a Nice Game of Chess?"

    def solve_part_1() -> str:
        with open('PuzzleInputs/05.txt') as puzzle_input:
            input_line: str = puzzle_input.read()

        password: str = ""
        index: int = 0

        while (len(password) < 8):
            hash_input: str = input_line + str(index)
            # I have previously implemented an md5 hash for 2015 day 4 in C++ (see
            # https://github.com/ZephyrSquall/AdventOfCode/blob/main/2015/Solvers/Day04Solver.cpp).
            # Converting this code to python would be incredibly tedious, so instead I'll simply use
            # a premade function for this.
            hash_output = md5(hash_input.encode("utf-8")).hexdigest()

            if (hash_output[:5] == "00000"):
                password += hash_output[5]

            index += 1

        return password


    def solve_part_2() -> str:
        return "0"