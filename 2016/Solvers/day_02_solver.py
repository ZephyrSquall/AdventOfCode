from solver import Solver

class Day02Solver(Solver):
    puzzle_title = "Bathroom Security"

    @staticmethod
    def solve_part_1() -> str:
        with open('PuzzleInputs/02.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        class Keypad:
            # (0,0) is the 1 at the top-left, (2,2) is the 9 at the bottom-right.
            def __init__(self) -> None:
                self._horizontal_position = 1
                self._vertical_position = 1

            def go_up(self) -> None:
                self._vertical_position = max(self._vertical_position - 1, 0)
            def go_down(self) -> None:
                self._vertical_position = min(self._vertical_position + 1, 2)
            def go_left(self) -> None:
                self._horizontal_position = max(self._horizontal_position - 1, 0)
            def go_right(self) -> None:
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

        return str(result)


    @staticmethod
    def solve_part_2() -> str:
        with open('PuzzleInputs/02.txt') as puzzle_input:
            input_lines: list[str] = puzzle_input.read().splitlines()

        class Keypad:
            # (2,0) is the 1 at the top-center, (4,4) is the blank at the bottom-right.
            def __init__(self) -> None:
                self._x = 0
                self._y = 2

            def go_up(self) -> None:
                if (not (self._x == 0 and self._y == 2
                    or self._x == 1 and self._y == 1
                    or self._x == 2 and self._y == 0
                    or self._x == 3 and self._y == 1
                    or self._x == 4 and self._y == 2)):
                    self._y -= 1
            def go_down(self) -> None:
                if (not (self._x == 0 and self._y == 2
                    or self._x == 1 and self._y == 3
                    or self._x == 2 and self._y == 4
                    or self._x == 3 and self._y == 3
                    or self._x == 4 and self._y == 2)):
                    self._y += 1
            def go_left(self) -> None:
                if (not (self._x == 2 and self._y == 0
                    or self._x == 1 and self._y == 1
                    or self._x == 0 and self._y == 2
                    or self._x == 1 and self._y == 3
                    or self._x == 2 and self._y == 4)):
                    self._x -= 1
            def go_right(self) -> None:
                if (not (self._x == 2 and self._y == 0
                    or self._x == 3 and self._y == 1
                    or self._x == 4 and self._y == 2
                    or self._x == 3 and self._y == 3
                    or self._x == 2 and self._y == 4)):
                    self._x += 1

            def current_key(self) -> str:
                if (self._x == 2 and self._y == 0):
                    return "1"
                elif (self._x == 1 and self._y == 1):
                    return "2"
                elif (self._x == 2 and self._y == 1):
                    return "3"
                elif (self._x == 3 and self._y == 1):
                    return "4"
                elif (self._x == 0 and self._y == 2):
                    return "5"
                elif (self._x == 1 and self._y == 2):
                    return "6"
                elif (self._x == 2 and self._y == 2):
                    return "7"
                elif (self._x == 3 and self._y == 2):
                    return "8"
                elif (self._x == 4 and self._y == 2):
                    return "9"
                elif (self._x == 1 and self._y == 3):
                    return "A"
                elif (self._x == 2 and self._y == 3):
                    return "B"
                elif (self._x == 3 and self._y == 3):
                    return "C"
                elif (self._x == 2 and self._y == 4):
                    return "D"
                else:
                    raise RuntimeError("Keypad position on blank space")


        keypad = Keypad()
        numbers: list[str] = []

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

        result: str = ""
        for number in numbers:
            result += number

        return result