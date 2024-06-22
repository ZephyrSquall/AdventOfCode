pub mod day_01;
pub mod day_02;
pub mod day_03;
pub mod day_04;

pub trait Solve{
    fn solve_part_1(&self, input: &str) -> i32;
    fn solve_part_2(&self, input: &str) -> i32;
}

