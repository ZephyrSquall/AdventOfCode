use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 5,
    title: "A Maze of Twisty Trampolines, All Alike",

    solve_part_1: |input| solve(input, false),
    solve_part_2: |input| solve(input, true),
};

fn solve(input: &str, strange_jumps: bool) -> Solution {
    let mut jumps = Vec::new();
    for jump in input.lines() {
        let jump = jump.parse::<isize>().expect("Error parsing number");
        jumps.push(jump);
    }

    let mut step = 0;
    let mut position: isize = 0;

    loop {
        match jumps.get_mut(position as usize) {
            Some(jump) => {
                step += 1;
                position += *jump;

                if strange_jumps && *jump >= 3 {
                    *jump -= 1;
                } else {
                    *jump += 1;
                }
            }
            None => return Solution::U32(step),
        }
    }
}

#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn part1_example1() {
        let result = (SOLVER.solve_part_1)(
            "\
0
3
0
1
-3",
        );
        assert_eq!(Solution::U32(5), result)
    }

    #[test]
    fn part2_example1() {
        let result = (SOLVER.solve_part_2)(
            "\
0
3
0
1
-3",
        );
        assert_eq!(Solution::U32(10), result)
    }
}
