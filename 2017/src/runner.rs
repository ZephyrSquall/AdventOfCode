use crate::solver::SOLVERS;
use std::fs;
use std::time::Instant;

const DAY_TITLE: &str = "Day";
const PUZZLE_TITLE: &str = "Puzzle";
const PART_TITLE: &str = "Part";
const SOLUTION_TITLE: &str = "Solution";
const TIMING_TITLE: &str = "Time (ms)";

struct MaxLength {
    day: usize,
    puzzle: usize,
    part: usize,
    solution: usize,
    timing: usize,
}

struct SolutionFormat {
    day: String,
    title: String,
    solution_1: String,
    solution_2: String,
    time_1: String,
    time_2: String,
}

pub fn run(config: &[u8]) {
    let (solution_formats, max_length) = run_solvers(config);

    print_results_table(solution_formats, &max_length);
}

fn run_solvers(config: &[u8]) -> (Vec<SolutionFormat>, MaxLength) {
    let mut solution_formats = Vec::with_capacity(SOLVERS.len());
    let mut max_length = MaxLength {
        day: DAY_TITLE.len(),
        puzzle: PUZZLE_TITLE.len(),
        part: PART_TITLE.len(),
        solution: SOLUTION_TITLE.len(),
        timing: TIMING_TITLE.len(),
    };

    for solver in SOLVERS {
        // Only run the solver if the config specified to run this solver or the config didn't
        // specify any particular solvers.
        if config.is_empty() || config.contains(&solver.day) {
            // Fetch the input strings for each puzzle from the text files under puzzle_inputs.
            // "{:02}" left-pads the day number with a 0 if needed so the width of the number is two
            // (text files for the first 9 days are prefixed with a 0 e.g. "01.txt" so it's sorted
            // properly by file systems).
            let file_path = format!("puzzle_inputs/{:02}.txt", solver.day);
            let input = fs::read_to_string(&file_path).expect("Error reading file");

            // Run the solvers while measuring their execution time.
            let start = Instant::now();
            let solution_1 = (solver.solve_1)(&input);
            let duration = start.elapsed();
            let time_1 = duration.as_micros();

            let start = Instant::now();
            let solution_2 = (solver.solve_2)(&input);
            let duration = start.elapsed();
            let time_2 = duration.as_micros();

            // Pad time strings with zeroes until they are at least four characters long, then
            // insert a decimal point three characters from the end of the string. This way the
            // number of microseconds is converted to a display of milliseconds with a fractional
            // part.
            let mut time_1 = format!("{time_1:04}");
            time_1.insert(time_1.len() - 3, '.');

            let mut time_2 = format!("{time_2:04}");
            time_2.insert(time_2.len() - 3, '.');

            // Store the string representation of all information to be printed in the results
            // table.
            let solution_format = SolutionFormat {
                day: solver.day.to_string(),
                title: solver.title.to_string(),
                solution_1: solution_1.to_string(),
                solution_2: solution_2.to_string(),
                time_1,
                time_2,
            };

            // Check if the length of any data to be displayed exceeds the current maximum length.
            // If so, update the maximum length.
            if solution_format.day.len() > max_length.day {
                max_length.day = solution_format.day.len();
            }
            if solution_format.title.len() > max_length.puzzle {
                max_length.puzzle = solution_format.title.len();
            }
            if solution_format.solution_1.len() > max_length.solution {
                max_length.solution = solution_format.solution_1.len();
            }
            if solution_format.solution_2.len() > max_length.solution {
                max_length.solution = solution_format.solution_2.len();
            }
            if solution_format.time_1.len() > max_length.timing {
                max_length.timing = solution_format.time_1.len();
            }
            if solution_format.time_2.len() > max_length.timing {
                max_length.timing = solution_format.time_2.len();
            }

            solution_formats.push(solution_format);
        }
    }

    (solution_formats, max_length)
}

// This function displays the results table, so it is the only place in this repository where
// printing is intentionally used to create user-facing output.
#[allow(clippy::print_stdout)]
fn print_results_table(solution_formats: Vec<SolutionFormat>, max_length: &MaxLength) {
    // Generate table header
    println!(
        "╔═{empty:═<day_width$}═╤═{empty:═<puzzle_width$}═╤═{empty:═<part_width$}═╤═{empty:═<solution_width$}═╤═{empty:═<timing_width$}═╗",
        empty = "",
        day_width = max_length.day,
        puzzle_width = max_length.puzzle,
        part_width = max_length.part,
        solution_width = max_length.solution,
        timing_width = max_length.timing
    );
    println!(
        "║ {DAY_TITLE:day_width$} │ {PUZZLE_TITLE:puzzle_width$} │ {PART_TITLE:part_width$} │ {SOLUTION_TITLE:solution_width$} │ {TIMING_TITLE:timing_width$} ║",
        day_width = max_length.day,
        puzzle_width = max_length.puzzle,
        part_width = max_length.part,
        solution_width = max_length.solution,
        timing_width = max_length.timing
    );
    println!(
        "╟─{empty:─<day_width$}─┼─{empty:─<puzzle_width$}─┼─{empty:─<part_width$}─┼─{empty:─<solution_width$}─┼─{empty:─<timing_width$}─╢",
        empty = "",
        day_width = max_length.day,
        puzzle_width = max_length.puzzle,
        part_width = max_length.part,
        solution_width = max_length.solution,
        timing_width = max_length.timing
    );

    // Generate rows for each solution format
    let mut is_first_row = true;
    for solution_format in solution_formats {
        // Skip the empty row if it's the first row.
        if is_first_row {
            is_first_row = false;
        } else {
            println!(
                "║ {empty:day_width$} │ {empty:puzzle_width$} │ {empty:part_width$} │ {empty:solution_width$} │ {empty:timing_width$} ║",
                empty = "",
                day_width = max_length.day,
                puzzle_width = max_length.puzzle,
                part_width = max_length.part,
                solution_width = max_length.solution,
                timing_width = max_length.timing
            );
        }
        println!(
            "║ {day:>day_width$} │ {puzzle:puzzle_width$} │ {part:>part_width$} │ {solution:>solution_width$} │ {timing:>timing_width$} ║",
            day = solution_format.day,
            puzzle = solution_format.title,
            part = "1",
            solution = solution_format.solution_1,
            timing = solution_format.time_1,
            day_width = max_length.day,
            puzzle_width = max_length.puzzle,
            part_width = max_length.part,
            solution_width = max_length.solution,
            timing_width = max_length.timing
        );
        println!(
            "║ {empty:day_width$} │ {empty:puzzle_width$} │ {part:>part_width$} │ {solution:>solution_width$} │ {timing:>timing_width$} ║",
            empty = "",
            part = "2",
            solution = solution_format.solution_2,
            timing = solution_format.time_2,
            day_width = max_length.day,
            puzzle_width = max_length.puzzle,
            part_width = max_length.part,
            solution_width = max_length.solution,
            timing_width = max_length.timing
        );
    }

    // Generate table footer
    println!(
        "╚═{empty:═<day_width$}═╧═{empty:═<puzzle_width$}═╧═{empty:═<part_width$}═╧═{empty:═<solution_width$}═╧═{empty:═<timing_width$}═╝",
        empty = "",
        day_width = max_length.day,
        puzzle_width = max_length.puzzle,
        part_width = max_length.part,
        solution_width = max_length.solution,
        timing_width = max_length.timing
    );
}
