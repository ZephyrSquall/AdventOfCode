use atoi::ascii_to_digit;
use std::fs;

pub fn solve_part_1() -> i32 {
    let file_path = "puzzle_inputs/01.txt";
    let input = fs::read(file_path).expect("Error reading file");
    let mut checksum: i32 = 0;

    // Use windows to access every pair of consecutive characters.
    for character_window in input.windows(2) {
        if character_window[0] == character_window[1] {
            let digit = ascii_to_digit::<i32>(character_window[0]).expect("Should decode to digit");
            checksum += digit;
        }
    }

    // Account for wrap-around by comparing the first and last elements.
    if input[0] == input[input.len() - 1] {
        let digit = ascii_to_digit::<i32>(input[0]).expect("Should decode to digit");
        checksum += digit;
    }

    checksum
}

pub fn solve_part_2() -> i32 {
    let file_path = "puzzle_inputs/01.txt";
    let input = fs::read(file_path).expect("Error reading file");
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

    checksum
}