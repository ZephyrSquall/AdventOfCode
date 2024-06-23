use itertools::Itertools;

pub const SOLVER_02: super::Solver = super::Solver {
    day: 2,
    title: "Corruption Checksum",
    
    solve_part_1: |input| {
        let mut checksum = 0;

        for line in input.lines() {
            let mut largest_int = i32::MIN;
            let mut smallest_int = i32::MAX;

            for number in line.split('\t') {
                let number = number.parse::<i32>().expect("Error parsing number");
                if number > largest_int {
                    largest_int = number;
                }
                if number < smallest_int {
                    smallest_int = number;
                }
            }

            checksum += largest_int - smallest_int;
        }

        Box::from(checksum)
    },

    solve_part_2: |input| {
        let mut checksum = 0;

        for line in input.lines() {
            let numbers_iter = line
                .split('\t')
                .map(|string| string.parse::<i32>().expect("Error parsing number"));

            // .permutations gives every pair of numbers. Both orderings of each pair of numbers are
            // present, hence it is not necessary to check number[1] % number[0] == 0 for any pair.
            for number in numbers_iter.permutations(2) {
                if number[0] % number[1] == 0 {
                    checksum += number[0] / number[1];
                    break;
                }
            }
        }

        Box::from(checksum)
    },
};
