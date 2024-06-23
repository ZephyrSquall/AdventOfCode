use std::fs;
use std::fmt::Display;
use std::time::Instant;
use crate::solver::SOLVERS;

struct Solution<'a> {
    day: u8,
    title: &'a str,
    solution_1: Box<dyn Display>,
    solution_2: Box<dyn Display>,
    time_1: u128,
    time_2: u128,
}

pub fn run() {
    let solutions = run_solvers();

    for solution in solutions {
        println!("Day {}: {}", solution.day, solution.title);
        println!("{} in {} ns", solution.solution_1, solution.time_1);
        println!("{} in {} ns", solution.solution_2, solution.time_2);
    }
}

fn run_solvers() -> Vec<Solution<'static>> {
    let mut solutions = Vec::new();

    for solver in SOLVERS {
        // "{:02}" left-pads the day number with a 0 if needed so the width of the number is two.
        let file_path = format!("puzzle_inputs/{:02}.txt", solver.day);
        let input = fs::read_to_string(&file_path).expect("Error reading file");

        let start = Instant::now();
        let solution_1 = (solver.solve_part_1)(&input);
        let duration = start.elapsed();
        let time_1 = duration.as_micros();

        let start = Instant::now();
        let solution_2 = (solver.solve_part_2)(&input);
        let duration = start.elapsed();
        let time_2 = duration.as_nanos();

        let solution = Solution {
            day: solver.day,
            title: solver.title,
            solution_1,
            solution_2,
            time_1,
            time_2,
        };

        solutions.push( solution);
    }

    solutions
}