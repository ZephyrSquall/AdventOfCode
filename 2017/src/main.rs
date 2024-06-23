use std::fs;

mod solver;

fn main() {
    let solvers: [solver::Solver; 4] = [
        solver::day_01::SOLVER_01,
        solver::day_02::SOLVER_02,
        solver::day_03::SOLVER_03,
        solver::day_04::SOLVER_04,
    ];

    let file_path = "puzzle_inputs/01.txt";
    let input = fs::read_to_string(file_path).expect("Error reading file");
    println!("Day 01:");
    println!("{}", (solvers[0].solve_part_1)(&input));
    println!("{}", (solvers[0].solve_part_2)(&input));
    println!("Day 02:");
    let file_path = "puzzle_inputs/02.txt";
    let input = fs::read_to_string(file_path).expect("Error reading file");
    println!("{}", (solvers[1].solve_part_1)(&input));
    println!("{}", (solvers[1].solve_part_2)(&input));
    println!("Day 03:");
    let file_path = "puzzle_inputs/03.txt";
    let input = fs::read_to_string(file_path).expect("Error reading file");
    println!("{}", (solvers[2].solve_part_1)(&input));
    println!("{}", (solvers[2].solve_part_2)(&input));
    println!("Day 04:");
    let file_path = "puzzle_inputs/04.txt";
    let input = fs::read_to_string(file_path).expect("Error reading file");
    println!("{}", (solvers[3].solve_part_1)(&input));
    println!("{}", (solvers[3].solve_part_2)(&input));
}
