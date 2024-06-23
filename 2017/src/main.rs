use std::fs;

mod solver;

fn main() {
    let solvers: [solver::Solver; 1] = [solver::day_02::SOLVER_02];

    println!("Day 01:");
    println!("{}", solver::day_01::solve_part_1());
    println!("{}", solver::day_01::solve_part_2());
    println!("Day 02:");
    let file_path = "puzzle_inputs/02.txt";
    let input = fs::read_to_string(file_path).expect("Error reading file");
    println!("{}", (solvers[0].solve_part_1)(&input));
    println!("{}", (solvers[0].solve_part_2)(&input));
    println!("Day 03:");
    println!("{}", solver::day_03::solve_part_1());
    println!("{}", solver::day_03::solve_part_2());
    println!("Day 04:");
    println!("{}", solver::day_04::solve_part_1());
    println!("{}", solver::day_04::solve_part_2());
}
