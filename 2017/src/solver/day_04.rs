use itertools::Itertools;
use std::fmt::Display;

pub const SOLVER_04: super::Solver = super::Solver {
    day: 4,
    title: "High-Entropy Passphrases",
    
    solve_part_1: |input| solve(input, false),
    solve_part_2: |input| solve(input, true),
};

fn solve(input: &str, test_anagrams: bool) -> Box<dyn Display> {
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

    Box::from(valid_passphrase_count)
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
