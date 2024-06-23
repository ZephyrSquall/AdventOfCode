use std::fmt::Display;

pub mod day_01;
pub mod day_02;
pub mod day_03;
pub mod day_04;

pub const SOLVERS: [Solver; 4] = [
    day_01::SOLVER,
    day_02::SOLVER,
    day_03::SOLVER,
    day_04::SOLVER,
];

pub struct Solver<'a> {
    pub day: u8,
    pub title: &'a str,
    pub solve_part_1: fn(input: &str) -> Box<dyn Display>,
    pub solve_part_2: fn(input: &str) -> Box<dyn Display>,
}
