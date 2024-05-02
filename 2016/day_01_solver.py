from solver import Solver

class Day01Solver(Solver):
    def solve_part_1() -> int:
        with open('PuzzleInputs/01.txt') as puzzle_input:
            input_line = puzzle_input.read()
        lines = input_line.split(', ')

        # 0 for North, 1 for East, 2 for South, 3 for West.
        facing_direction = 0
        horizontal_offset = 0
        vertical_offset = 0

        for line in lines:
            if(line[:1] == 'L'):
                facing_direction = (facing_direction - 1) % 4
            elif(line[:1] == 'R'):
                facing_direction = (facing_direction + 1) % 4

            distance = int(line[1:])

            if(facing_direction == 0):
                vertical_offset += distance
            elif(facing_direction == 1):
                horizontal_offset += distance
            elif(facing_direction == 2):
                vertical_offset -= distance
            elif(facing_direction == 3):
                horizontal_offset -= distance

        total_distance = abs(horizontal_offset) + abs(vertical_offset)
        return total_distance
    

    def solve_part_2() -> int:
        return 0