use itertools::Itertools;
use std::fs;

pub fn solve_part_1() -> i32 {
    solve(false)
}

pub fn solve_part_2() -> i32 {
    solve(true)
}

fn solve(test_anagrams: bool) -> i32 {
    let file_path = "puzzle_inputs/04.txt";
    let input = fs::read_to_string(file_path).expect("Error reading file");
    let mut valid_passphrase_count = 0;

    for line in input.lines() {
        // Count every passphrase.
        valid_passphrase_count += 1;

        // Check every combination of words for a match.
        for (word1, word2) in line.split(' ').tuple_combinations::<(&str, &str)>() {
            if word_match(word1, word2, test_anagrams) {
                // Undo counting this passphrase, as it has matching words and is therefore invalid.
                valid_passphrase_count -= 1;
                break;
            }
        }
    }

    valid_passphrase_count
}

fn word_match(word1: &str, word2: &str, test_anagrams: bool) -> bool {
    if test_anagrams {
        // Sorts the letters of the words alphabetically (unstable sort used for performance).
        // Anagrams of a word will all sort to the same word, thus strings which are equal after
        // this procedure are anagrams of each other.
        word1
            .chars()
            .sorted_unstable()
            .eq(word2.chars().sorted_unstable())
    } else {
        word1 == word2
    }
}
