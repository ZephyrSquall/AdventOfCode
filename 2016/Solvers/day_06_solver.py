from solver import Solver

class Day06Solver(Solver):
    puzzle_title = "Signals and Noise"

    @staticmethod
    def get_counts(input_lines: list[str]) -> list[dict[str, int]]:
        counts: list[dict[str, int]] = []
        for _ in range(len(input_lines[0])):
            counts.append(dict())

        for i in range(len(input_lines)):
            for j in range(len(input_lines[i])):
                letter: str = input_lines[i][j]
                # For the dictionary representing the jth letter, get the key for the current letter
                # and add one to it. If that key doesn't exist, make it default to 0 (and then add 1
                # as usual).
                counts[j][letter] = counts[j].get(letter, 0) + 1

        return counts


    @staticmethod
    def solve_part_1() -> str:
        with open('PuzzleInputs/06.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        counts: list[dict[str, int]] = Day06Solver.get_counts(input_lines)

        result: str = ""
        for j in range(len(counts)):
            most_common_letter: str = max(counts[j], key=counts[j].__getitem__)
            result += most_common_letter

        return result


    @staticmethod
    def solve_part_2() -> str:
        with open('PuzzleInputs/06.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        counts: list[dict[str, int]] = Day06Solver.get_counts(input_lines)

        result: str = ""
        for j in range(len(counts)):
            most_common_letter: str = min(counts[j], key=counts[j].__getitem__)
            result += most_common_letter

        return result