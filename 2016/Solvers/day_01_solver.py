from solver import Solver

class Day01Solver(Solver):
    puzzle_title = "No Time for a Taxicab"

    def solve_part_1() -> str:
        with open('PuzzleInputs/01.txt') as puzzle_input:
            input_line: str = puzzle_input.read()
        lines: list[str] = input_line.split(', ')

        # 0 for North, 1 for East, 2 for South, 3 for West.
        facing_direction: int = 0
        horizontal_offset: int = 0
        vertical_offset: int = 0

        for line in lines:
            if(line[:1] == 'L'):
                facing_direction = (facing_direction - 1) % 4
            elif(line[:1] == 'R'):
                facing_direction = (facing_direction + 1) % 4

            distance: int = int(line[1:])

            if(facing_direction == 0):
                vertical_offset += distance
            elif(facing_direction == 1):
                horizontal_offset += distance
            elif(facing_direction == 2):
                vertical_offset -= distance
            elif(facing_direction == 3):
                horizontal_offset -= distance

        total_distance: int = abs(horizontal_offset) + abs(vertical_offset)
        return str(total_distance)


    def solve_part_2() -> str:
        with open('PuzzleInputs/01.txt') as puzzle_input:
            input_line: str = puzzle_input.read()
        lines: list[str] = input_line.split(', ')

        # 0 for North, 1 for East, 2 for South, 3 for West.
        facing_direction: int = 0
        horizontal_offset: int = 0
        vertical_offset: int = 0
        visited_coordinates: list[tuple[int, int]] = [(0, 0)]
        has_found_crossover: bool = False

        for line in lines:
            if(line[:1] == 'L'):
                facing_direction = (facing_direction - 1) % 4
            elif(line[:1] == 'R'):
                facing_direction = (facing_direction + 1) % 4

            distance: int = int(line[1:])
            is_last_step_horizontal: bool

            if(facing_direction == 0):
                vertical_offset += distance
                is_last_step_horizontal = False
            elif(facing_direction == 1):
                horizontal_offset += distance
                is_last_step_horizontal = True
            elif(facing_direction == 2):
                vertical_offset -= distance
                is_last_step_horizontal = False
            elif(facing_direction == 3):
                horizontal_offset -= distance
                is_last_step_horizontal = True

            # The first step is always either east or west.
            is_current_step_horizontal: bool = True

            for i in range(1, len(visited_coordinates)):
                # If the current step and last step were in the same direction, the paths can't
                # cross. Skip these steps.
                if ((is_current_step_horizontal and not is_last_step_horizontal) or (not is_current_step_horizontal and is_last_step_horizontal)):
                    # The horizontal crossing threshold comes from the horizontal position of the
                    # vertical line. The two horizontal limits come from the smallest and greatest
                    # horizontal positions on the horizontal line.
                    horizontal_crossing_threshold: int
                    horizontal_first_limit: int
                    horizontal_second_limit: int
                    vertical_crossing_threshold: int
                    vertical_first_limit: int
                    vertical_second_limit: int
                    if (is_current_step_horizontal):
                        horizontal_crossing_threshold = horizontal_offset
                        horizontal_first_limit = visited_coordinates[i][0]
                        horizontal_second_limit = visited_coordinates[i-1][0]
                        vertical_crossing_threshold = visited_coordinates[i][1]
                        vertical_first_limit = visited_coordinates[-1][1]
                        vertical_second_limit = vertical_offset
                    else:
                        horizontal_crossing_threshold = visited_coordinates[i][0]
                        horizontal_first_limit = visited_coordinates[-1][0]
                        horizontal_second_limit = horizontal_offset
                        vertical_crossing_threshold = vertical_offset
                        vertical_first_limit = visited_coordinates[i][1]
                        vertical_second_limit = visited_coordinates[i-1][1]

                    # The lines cross if the horizontal crossing threshold lies between the two
                    # horizontal limits and the vertical crossing threshold lies between the two
                    # vertical limits (if only one is true, that means the lines are simply next to
                    # each other horizontally or vertically). Note that a line could be oriented
                    # top-to-bottom or bottom-to-top so it's important to check the cases where the
                    # second limit is smaller than the first limit too. Check strictly less-than for
                    # the first limit so it doesn't match when checking the last two lines against
                    # each other (they touch at the corner), but check less-than-or-equal-to for the
                    # second limit in case the last line exactly touches but doesn't pass the other
                    # line.
                    if (((horizontal_first_limit < horizontal_crossing_threshold and horizontal_crossing_threshold <= horizontal_second_limit)
                        or (horizontal_second_limit <= horizontal_crossing_threshold and horizontal_crossing_threshold < horizontal_first_limit))
                        and ((vertical_first_limit < vertical_crossing_threshold and vertical_crossing_threshold <= vertical_second_limit)
                        or (vertical_second_limit <= vertical_crossing_threshold and vertical_crossing_threshold < vertical_first_limit))):
                        horizontal_offset = horizontal_crossing_threshold
                        vertical_offset = vertical_crossing_threshold
                        has_found_crossover = True
                        break

                # Each step, the direction switches between horizontal and vertical.
                is_current_step_horizontal = not is_current_step_horizontal

            if (has_found_crossover):
                break

            # If the code reaches this point, the last line doesn't cross any existing lines, so add
            # the last visited point to the list and follow the next direction.
            visited_coordinates.append((horizontal_offset, vertical_offset))

        total_distance: int = abs(horizontal_offset) + abs(vertical_offset)
        return str(total_distance)