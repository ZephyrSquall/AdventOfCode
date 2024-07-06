use std::collections::HashMap;

use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 12,
    title: "Digital Plumber",

    solve_1: |input| {
        let mut pipes: HashMap<u16, Vec<u16>> = HashMap::new();

        for line in input.lines() {
            let mut iter = line.split_whitespace();
            let current_pipe = iter
                .next()
                .expect("Line shouldn't be empty")
                .parse()
                .expect("Error parsing number");

            // Consume the "<->".
            iter.next();

            let connected_pipes = iter
                .map(|pipe| {
                    pipe.trim_end_matches(',')
                        .parse()
                        .expect("Error parsing number")
                })
                .collect();

            pipes.insert(current_pipe, connected_pipes);
        }

        let mut pipe_stack = vec![0];
        let mut found_pipes = vec![0];

        while let Some(pipe) = pipe_stack.pop() {
            let connected_pipes = pipes.get(&pipe).expect("Pipe missing from hashmap");

            for connected_pipe in connected_pipes {
                if !found_pipes.contains(connected_pipe) {
                    found_pipes.push(*connected_pipe);
                    pipe_stack.push(*connected_pipe);
                }
            }
        }

        Solution::USize(found_pipes.len())
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
0 <-> 2
1 <-> 1
2 <-> 0, 3, 4
3 <-> 2, 4
4 <-> 2, 3, 6
5 <-> 6
6 <-> 4, 5"
            ),
            Solution::U8(6)
        )
    }
}
