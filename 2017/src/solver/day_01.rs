use super::{Solution, Solver};
use atoi::ascii_to_digit;

pub const SOLVER: Solver = Solver {
    day: 1,
    title: "Inverse Captcha",

    solve_part_1: |input| {
        let input = input.as_bytes();
        let mut checksum: i32 = 0;

        // Use windows to access every pair of consecutive characters.
        for character_window in input.windows(2) {
            if character_window[0] == character_window[1] {
                let digit =
                    ascii_to_digit::<i32>(character_window[0]).expect("Should decode to digit");
                checksum += digit;
            }
        }

        // Account for wrap-around by comparing the first and last elements.
        if input[0] == input[input.len() - 1] {
            let digit = ascii_to_digit::<i32>(input[0]).expect("Should decode to digit");
            checksum += digit;
        }

        Solution::I32(checksum)
    },
    solve_part_2: |input| {
        let input = input.as_bytes();
        let mut checksum = 0;

        // Note that we only have to check half of the input digits for matches, as every value in the
        // second half will match again to its corresponding digit.
        let half_len = input.len() / 2;

        for i in 0..half_len {
            if input[i] == input[i + half_len] {
                let digit = ascii_to_digit::<i32>(input[i]).expect("Should decode to digit");
                checksum += digit;
            }
        }

        // Double the checksum to account for the skipped second half of the inputs.
        checksum *= 2;

        Solution::I32(checksum)
    },
};
