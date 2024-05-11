from solver import Solver
from functools import cmp_to_key

class Day04Solver(Solver):
    puzzle_title = "Security Through Obscurity"

    def solve_part_1() -> str:
        def letter_count_compare(a: tuple[chr, int], b: tuple[chr, int]) -> int:
            if (a[1] < b[1]):
                return -1
            elif (a[1] > b[1]):
                return 1
            else:
                if (ord(a[0]) < ord(b[0])):
                    return 1
                elif (ord(a[0]) > ord(b[0])):
                    return -1
                else:
                    return 0

        with open('PuzzleInputs/04.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        sector_id_sum: int = 0

        for input_line in input_lines:
            input_split_1: list[str] = input_line.split('[')
            # [:-1] gets all characters from a string except the last character, removing the
            # trailing ']'.
            given_checksum: str = input_split_1[1][:-1]
            input_split_2: list[str] = input_split_1[0].rsplit('-', 1)
            # Remove dashes from the name as they are not relevant to calculating the checksum.
            name: str = input_split_2[0].replace('-', '')
            sector_id: int = int(input_split_2[1])

            letter_counts: list[tuple[chr, int]] = []

            for i in range(ord('a'),ord('z')+1):
                letter = chr(i)
                letter_count = name.count(letter)
                letter_counts.append((letter, letter_count))

            letter_counts.sort(key=cmp_to_key(letter_count_compare), reverse = True)

            calculated_checksum: str = ""
            for letter_count in letter_counts[:5]:
                calculated_checksum += letter_count[0]
            
            if (calculated_checksum == given_checksum):
                sector_id_sum += sector_id

        return str(sector_id_sum)


    def solve_part_2() -> str:
        return "0"