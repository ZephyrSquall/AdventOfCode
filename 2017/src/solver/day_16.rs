use std::collections::VecDeque;

use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 16,
    title: "Permutation Promenade",

    solve_1: |input| {
        let mut dancers = VecDeque::from([
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
        ]);

        Solution::String(dance(&mut dancers, input))
    },

    solve_2: |input| Solution::U8(0),
};

fn dance(dancers: &mut VecDeque<char>, input: &str) -> String {
    for dance_move in input.split(',') {
        let mut chars = dance_move.chars();

        // Get the first character and perform the corresponding dance move.
        match chars.next() {
            Some('s') => {
                // The first character has already been consumed, so collecting the chars iterator
                // now simply gets the spin size as a string which can be parsed.
                let spin_size = chars
                    .collect::<String>()
                    .parse()
                    .expect("Spin size should be valid integer");
                dancers.rotate_right(spin_size);
            }
            Some('x') => {
                // Collecting the chars iterator gets a string of the two position integers
                // separated by a forward slash, which can be split by that forward slash and then
                // parsed.
                let exchange_parameters = chars.collect::<String>();
                let mut exchange_parameters = exchange_parameters.split('/');
                let first_position = exchange_parameters
                    .next()
                    .expect("Exchange parameters should have first value")
                    .parse()
                    .expect("First exchange position should be valid integer");
                let second_position = exchange_parameters
                    .next()
                    .expect("Exchange parameters should have second value")
                    .parse()
                    .expect("Second exchange position should be valid integer");
                dancers.swap(first_position, second_position);
            }
            Some('p') => {
                // As the arguments for a partner move are themselves characters, the chars iterator
                // can simply be advanced to get the dancer names.
                let first_dancer = chars
                    .next()
                    .expect("Partner parameters should have first value");
                // Ignore the '/'
                chars.next();
                let second_dancer = chars
                    .next()
                    .expect("Partner parameters should have second value");
                let first_position = dancers
                    .iter()
                    .position(|&d| d == first_dancer)
                    .expect("Dancer should be present");
                let second_position = dancers
                    .iter()
                    .position(|&d| d == second_dancer)
                    .expect("Dancer should be present");
                dancers.swap(first_position, second_position);
            }
            _ => panic!("Dance move has invalid instruction"),
        }
    }

    dancers.iter().collect()
}

#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn example1_1() {
        let mut dancers = VecDeque::from(['a', 'b', 'c', 'd', 'e']);
        assert_eq!(dance(&mut dancers, "s1,x3/4,pe/b"), "baedc")
    }
}
