from solver import Solver

class Day02Solver(Solver):
    puzzle_title = "Bathroom Security"
    
    def solve_part_1() -> int:
        with open('PuzzleInputs/02.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        class Keypad:
            # (0,0) is the 1 at the top-left, (2,2) is the 9 at the bottom-right.
            def __init__(self):
                self._horizontal_position = 1
                self._vertical_position = 1

            def go_up(self):
                self._vertical_position = max(self._vertical_position - 1, 0)
            def go_down(self):
                self._vertical_position = min(self._vertical_position + 1, 2)
            def go_left(self):
                self._horizontal_position = max(self._horizontal_position - 1, 0)
            def go_right(self):
                self._horizontal_position = min(self._horizontal_position + 1, 2)

            def current_key(self) -> int:
                return 1 + self._horizontal_position + 3 * self._vertical_position


        keypad = Keypad()
        numbers: list[int] = []

        for input_line in input_lines:
            for letter in input_line:
                if (letter == 'U'):
                    keypad.go_up()
                elif (letter == 'D'):
                    keypad.go_down()
                elif (letter == 'L'):
                    keypad.go_left()
                elif (letter == 'R'):
                    keypad.go_right()
            numbers.append(keypad.current_key())

        result: int = 0
        for i in range(len(numbers)):
            result *= 10
            result += numbers[i]

        return result


    def solve_part_2() -> int:
        return 0