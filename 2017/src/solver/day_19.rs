use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 19,
    title: "A Series of Tubes",

    solve_1: |input| {
        let mut grid = Vec::new();
        for line in input.lines() {
            let mut grid_line = Vec::new();
            for char in line.chars() {
                grid_line.push(char);
            }
            grid.push(grid_line);
        }

        enum Direction {
            Up,
            Right,
            Down,
            Left,
        }

        // Get the packet's starting position and direction, which is downwards from where the only
        // non-space character is located in the first row.

        let mut x = grid[0]
            .iter()
            .position(|c| *c != ' ')
            .expect("Should have a non-space character in first row");
        let mut y = 0;
        let mut direction = Direction::Down;
        let mut visited_letters = Vec::new();

        // Pushes the letter in the grid at the given position into visited_letters, unless it's a
        // regular path character ('|', '-', or '+').
        fn update_visited_letters(
            grid: &[Vec<char>],
            visited_letters: &mut Vec<char>,
            x: usize,
            y: usize,
        ) {
            let letter = grid[y][x];
            if letter != '|' && letter != '-' && letter != '+' {
                visited_letters.push(letter);
            }
        }

        loop {
            match direction {
                Direction::Up => {
                    y -= 1;
                    update_visited_letters(&grid, &mut visited_letters, x, y);

                    if grid[y - 1][x] == ' ' {
                        if grid[y][x + 1] != ' ' {
                            direction = Direction::Right;
                        } else if grid[y][x - 1] != ' ' {
                            direction = Direction::Left;
                        } else {
                            break;
                        }
                    }
                }
                Direction::Right => {
                    x += 1;
                    update_visited_letters(&grid, &mut visited_letters, x, y);

                    if grid[y][x + 1] == ' ' {
                        if grid[y + 1][x] != ' ' {
                            direction = Direction::Down;
                        } else if grid[y - 1][x] != ' ' {
                            direction = Direction::Up;
                        } else {
                            break;
                        }
                    }
                }
                Direction::Down => {
                    y += 1;
                    update_visited_letters(&grid, &mut visited_letters, x, y);

                    if grid[y + 1][x] == ' ' {
                        if grid[y][x + 1] != ' ' {
                            direction = Direction::Right;
                        } else if grid[y][x - 1] != ' ' {
                            direction = Direction::Left;
                        } else {
                            break;
                        }
                    }
                }
                Direction::Left => {
                    x -= 1;
                    update_visited_letters(&grid, &mut visited_letters, x, y);

                    if grid[y][x - 1] == ' ' {
                        if grid[y + 1][x] != ' ' {
                            direction = Direction::Down;
                        } else if grid[y - 1][x] != ' ' {
                            direction = Direction::Up;
                        } else {
                            break;
                        }
                    }
                }
            }
        }

        let visited_letters = visited_letters.into_iter().collect();

        Solution::String(visited_letters)
    },

    solve_2: |input| Solution::U8(0),
};

#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn example1_1() {
        assert_eq!(
            (SOLVER.solve_1)(
                // In Rust, an end-of-line backslash ignores the newline and all whitespace at the
                // beginning of the following line. In this case, that whitespace is intended to be
                // part of the string, so a backslash can't be used to align this string literal.
                "     |          
     |  +--+    
     A  |  C    
 F---|----E|--+ 
     |  |  |  D 
     +B-+  +--+ 
                "
            ),
            Solution::Str("ABCDEF")
        )
    }
}
