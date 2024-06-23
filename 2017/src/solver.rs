use std::fmt::Display;

pub mod day_01;
pub mod day_02;
pub mod day_03;
pub mod day_04;

pub struct Solver {
    pub solve_part_1: fn(input: &str) -> Box<dyn Display>,
    pub solve_part_2: fn(input: &str) -> Box<dyn Display>,
}
