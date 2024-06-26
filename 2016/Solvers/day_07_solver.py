from solver import Solver

class Day07Solver(Solver):
    puzzle_title = "Internet Protocol Version 7"

    @staticmethod
    def solve_part_1() -> str:
        with open('PuzzleInputs/07.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        tls_count: int = 0

        for input_line in input_lines:
            has_abba: bool = False
            is_inside_square_brackets: bool = False
            # Python doesn't support "for(int i = 0; i < some_value; i++)" style for loops, only
            # "foreach" loops. As this solution directly modifies i in the loop, foreach doesn't
            # work, so the "for" syntax must be made manually with a while loop.
            # Start at i=3 as there must be at least three preceding characters to be able to check
            # for an ABBA pattern.
            i: int = 3
            while(i < len(input_line)):
                if (input_line[i] == '['):
                    is_inside_square_brackets = True
                    i += 3
                elif (input_line[i] == ']'):
                    is_inside_square_brackets = False
                    i += 3
                elif (input_line[i-3] == input_line[i] and
                      input_line[i-2] == input_line[i-1] and
                      input_line[i-1] != input_line[i]):
                    if(is_inside_square_brackets):
                        # A single ABBA inside square brackets disqualifies the IP address from
                        # being IPv7, so set has_abba to False and immediately break from the loop
                        # to guarantee this line isn't counted.
                        has_abba = False
                        break
                    else:
                        has_abba = True
                i += 1

            if (has_abba):
                tls_count += 1

        return str(tls_count)


    @staticmethod
    def solve_part_2() -> str:
        with open('PuzzleInputs/07.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        ssl_count: int = 0

        for input_line in input_lines:
            # aba sequences are unique identified by their first two characters, so there's no need
            # to store the third character.
            aba_sequences: list[tuple[str, str]] = []
            # A bab_sequence is just an aba_sequence found inside square brackets
            bab_sequences: list[tuple[str, str]] = []
            is_inside_square_brackets: bool = False
            # Start at i=2 as there must be at least two preceding characters to be able to check
            # for an ABA or BAB pattern
            i: int = 2
            while(i < len(input_line)):
                if (input_line[i] == '['):
                    is_inside_square_brackets = True
                    i += 2
                elif (input_line[i] == ']'):
                    is_inside_square_brackets = False
                    i += 2
                elif (input_line[i-2] == input_line[i] and
                      input_line[i-1] != input_line[i]):
                    if(is_inside_square_brackets):
                        bab_sequences.append((input_line[i-2], input_line[i-1]))
                    else:
                        aba_sequences.append((input_line[i-2], input_line[i-1]))
                i += 1

            does_any_aba_have_matching_bab: bool = False
            for aba in aba_sequences:
                for bab in bab_sequences:
                    if (aba[0] == bab[1] and aba[1] == bab[0]):
                        does_any_aba_have_matching_bab = True
                        break
                if (does_any_aba_have_matching_bab):
                    break
            if (does_any_aba_have_matching_bab):
                ssl_count += 1

        return str(ssl_count)