from solver import Solver
from hashlib import md5

class Day05Solver(Solver):
    puzzle_title = "How About a Nice Game of Chess?"

    @staticmethod
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
            hash_output: str = md5(hash_input.encode("utf-8")).hexdigest()

            if (hash_output[:5] == "00000"):
                password += hash_output[5]

            index += 1

        return password


    @staticmethod
    def solve_part_2() -> str:
        with open('PuzzleInputs/05.txt') as puzzle_input:
            input_line: str = puzzle_input.read()

        password_characters: list[str] = ['', '', '', '', '', '', '', '']
        index: int = 0
        is_password_digit_set: list[bool] = [False, False, False, False, False, False, False, False]

        while (False in is_password_digit_set):
            hash_input: str = input_line + str(index)
            hash_output: str = md5(hash_input.encode("utf-8")).hexdigest()

            if (hash_output[:5] == "00000"):
                position_character: str = hash_output[5]
                if (position_character in ['0', '1', '2', '3', '4', '5', '6', '7']):
                    position: int = int(position_character)
                    if (not is_password_digit_set[position]):
                        is_password_digit_set[position] = True
                        password_characters[position] = hash_output[6]

            index += 1

        password = "".join(password_characters)
        return password