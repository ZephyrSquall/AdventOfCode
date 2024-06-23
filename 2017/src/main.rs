use std::fs;

mod solver;

fn main() {
    let solvers: [solver::Solver; 4] = [
        solver::day_01::SOLVER_01,
        solver::day_02::SOLVER_02,
        solver::day_03::SOLVER_03,
        solver::day_04::SOLVER_04,
    ];

    for solver in &solvers {
        // "{:02}" left-pads the day number with a 0 if needed so the width of the number is two.
        let file_path = format!("puzzle_inputs/{:02}.txt", solver.day);
        let input = fs::read_to_string(&file_path).expect("Error reading file");
        
        println!("Day {}", solver.day);
        println!("{}", (solver.solve_part_1)(&input));
        println!("{}", (solver.solve_part_2)(&input));
    }
}
