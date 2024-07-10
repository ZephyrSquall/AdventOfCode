use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 15,
    title: "Dueling Generators",

    solve_1: |input| {
        let mut lines = input.lines();
        // Get the next line of the puzzle input, split it into individual words, take the last
        // word, and convert it to an integer.
        let mut generator_a = lines
            .next()
            .expect("Input should have first line")
            .split_whitespace()
            .next_back()
            .expect("First line should have text")
            .parse()
            .expect("First line should end with valid number");
        let mut generator_b = lines
            .next()
            .expect("Input should have second line")
            .split_whitespace()
            .next_back()
            .expect("Second line should have text")
            .parse()
            .expect("Second line should end with valid number");

        fn generate_next(value: u64, factor: u64) -> u64 {
            (value * factor) % 2147483647
        }

        let mut judge_count = 0;

        for _ in 0..40000000 {
            generator_a = generate_next(generator_a, 16807);
            generator_b = generate_next(generator_b, 48271);

            // 0xffff is an integer whose last 16 bits are 1 and all remaining bits are 0.
            // Performing a bitwise and with this integer zeroes out all bits except the last 16,
            // which can then be compared with another number that performed this operation to only
            // check if these numbers' last 16 bits match.
            if generator_a & 0xffff == generator_b & 0xffff {
                judge_count += 1;
            }
        }

        Solution::U32(judge_count)
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
                "\
Generator A starts with 65
Generator B starts with 8921"
            ),
            Solution::U16(588)
        )
    }
}
