from solver import Solver

class Day06Solver(Solver):
    puzzle_title = "Signals and Noise"

    def get_counts(input_lines: list[str]) -> list[dict[chr, int]]:
        counts: list[dict[chr, int]] = []
        for _ in range(len(input_lines[0])):
            counts.append(dict())

        for i in range(len(input_lines)):
            for j in range(len(input_lines[i])):
                letter: chr = input_lines[i][j]
                # For the dictionary representing the jth letter, get the key for the current letter
                # and add one to it. If that key doesn't exist, make it default to 0 (and then add 1
                # as usual).
                counts[j][letter] = counts[j].get(letter, 0) + 1

        return counts


    def solve_part_1() -> str:
        with open('PuzzleInputs/06.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        counts: list[dict[chr, int]] = Day06Solver.get_counts(input_lines)

        result: str = ""
        for j in range(len(counts)):
            most_common_letter: chr = max(counts[j], key=counts[j].get)
            result += most_common_letter

        return result


    def solve_part_2() -> str:
        with open('PuzzleInputs/06.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        counts: list[dict[chr, int]] = Day06Solver.get_counts(input_lines)

        result: str = ""
        for j in range(len(counts)):
            most_common_letter: chr = min(counts[j], key=counts[j].get)
            result += most_common_letter

        return result