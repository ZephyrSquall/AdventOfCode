from solver import Solver
from typing import Final
from copy import deepcopy

class Day08Solver(Solver):
    puzzle_title = "Two-Factor Authentication"

    @staticmethod
    def solve_part_1() -> str:
        with open('PuzzleInputs/08.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        ROWS: Final[int] = 6
        COLUMNS: Final[int] = 50

        # Like creating a bool[50][6] in other languages. screen[0][0] is the upper-left corner.
        # Every value is initialized to False.
        screen: list[list[bool]] = [[False for i in range(ROWS)] for i in range(COLUMNS)]
        is_rotation_horizontal: bool = True

        for input_line in input_lines:
            line_split: list[str] = input_line.split(" ", 1)

            if (line_split[0] == "rect"):
                dimension_split: list[str] = line_split[1].split("x")
                rectangle_width: int = int(dimension_split[0])
                rectangle_height: int = int(dimension_split[1])
                for i in range(rectangle_width):
                    for j in range(rectangle_height):
                        screen[i][j] = True

            elif (line_split[0] == "rotate"):
                direction_split: list[str] = line_split[1].split("=")
                if (direction_split[0] == "row y"):
                    is_rotation_horizontal = True
                elif (direction_split[0] == "column x"):
                    is_rotation_horizontal = False
                else:
                    raise RuntimeError("\"rotate\" line did not contain \"row y\" or \"column x\"")
                rotation_parameters_split: list[str] = direction_split[1].split(" by ")
                rotation_index: int = int(rotation_parameters_split[0])
                rotation_distance: int = int(rotation_parameters_split[1])

                if (is_rotation_horizontal):
                    previous_row: list[bool] = []
                    for column in range(COLUMNS):
                        previous_row.append(screen[column][rotation_index])
                    for column in range(COLUMNS):
                        new_column: int = (column + rotation_distance) % COLUMNS
                        screen[new_column][rotation_index] = previous_row[column]
                else:
                    previous_column: list[bool] = deepcopy(screen[rotation_index])
                    for row in range(ROWS):
                        new_row: int = (row + rotation_distance) % ROWS
                        screen[rotation_index][new_row] = previous_column[row]

            else:
                raise RuntimeError("Line did not start with \"rect\" or \"rotate\"")

        count: int = sum([column.count(True) for column in screen])
        return str(count)


    @staticmethod
    def solve_part_2() -> str:
        return "0"