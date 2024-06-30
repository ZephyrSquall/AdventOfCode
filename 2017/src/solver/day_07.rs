use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 7,
    title: "Recursive Circus",

    solve_1: |input| {
        let mut programs = Vec::new();
        let mut above_programs = Vec::new();

        for line in input.lines() {
            let mut iter = line.split_whitespace();
            programs.push(iter.next().expect("Error reading first value of line"));
            // call next() twice and discard the value to get past the bracketed number and the "->"
            // elements in the line.
            iter.next();
            iter.next();

            // After iterating three times, iter is at the first element of the list of above
            // programs (or None if this program has no programs above it). Loop over the rest of
            // the iterator to put these in the list of above programs and remove trailing commas.
            for above_program in iter {
                above_programs.push(above_program.trim_end_matches(','));
            }
        }

        // The bottom program is the only program that isn't above any other program, meaning it's
        // the only program that is in programs but not above_programs.
        for program in programs {
            if !above_programs.contains(&program) {
                return Solution::Str(program);
            }
        }

        // If no value is returned from the above loop, either the input is malformed or this solver
        // has a logic error. Either way, no valid solution can be returned, so panic.
        panic!("No bottom program found")
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
pbga (66)
xhth (57)
ebii (61)
havc (66)
ktlj (57)
fwft (72) -> ktlj, cntj, xhth
qoyq (66)
padx (45) -> pbga, havc, qoyq
tknk (41) -> ugml, padx, fwft
jptl (61)
ugml (68) -> gyxo, ebii, jptl
gyxo (61)
cntj (57)"
            ),
            Solution::Str("tknk")
        )
    }
}
