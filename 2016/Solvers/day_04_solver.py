from solver import Solver
from functools import cmp_to_key

class Day04Solver(Solver):
    puzzle_title = "Security Through Obscurity"

    @staticmethod
    def _letter_count_compare(a: tuple[str, int], b: tuple[str, int]) -> int:
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

    class EncryptedLine:
        def __init__(self, input_line: str):
            input_split_1: list[str] = input_line.split('[')
            input_split_2: list[str] = input_split_1[0].rsplit('-', 1)

            self.name: str = input_split_2[0]
            self.sector_id: int = int(input_split_2[1])
            # [:-1] gets all characters from a string except the last character, removing the
            # trailing ']'.
            self.checksum: str = input_split_1[1][:-1]

        def has_valid_checksum(self) -> bool:
            letter_counts: list[tuple[str, int]] = []

            for i in range(ord('a'),ord('z')+1):
                letter: str = chr(i)
                count: int = self.name.count(letter)
                letter_counts.append((letter, count))

            letter_counts.sort(key=cmp_to_key(Day04Solver._letter_count_compare), reverse = True)

            checksum: str = ""
            for letter_count in letter_counts[:5]:
                checksum += letter_count[0]

            return checksum == self.checksum

        def decrypt(self) -> str:
            # Every 26 shifts returns the string to where it started, so only the remainder matters.
            shift: int = self.sector_id % 26
            decrypted_line: str = ""

            for letter in self.name:
                if (letter == '-'):
                    decrypted_line += '-'
                else:
                    letter_index: int = ord(letter)
                    letter_index += shift
                    if (letter_index > ord('z')):
                        letter_index -= 26

                    shifted_letter: str = chr(letter_index)
                    decrypted_line += shifted_letter

            return decrypted_line


    @staticmethod
    def solve_part_1() -> str:
        with open('PuzzleInputs/04.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        sector_id_sum: int = 0

        for input_line in input_lines:
            encrypted_line = Day04Solver.EncryptedLine(input_line)
            if (encrypted_line.has_valid_checksum()):
                sector_id_sum += encrypted_line.sector_id

        return str(sector_id_sum)


    @staticmethod
    def solve_part_2() -> str:
        with open('PuzzleInputs/04.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        for input_line in input_lines:
            encrypted_line = Day04Solver.EncryptedLine(input_line)
            if (encrypted_line.has_valid_checksum()):
                decrypted_line: str = encrypted_line.decrypt()
                if "north" in decrypted_line and "pole" in decrypted_line:
                    return str(encrypted_line.sector_id)

        raise RuntimeError("North Pole storage room not found")