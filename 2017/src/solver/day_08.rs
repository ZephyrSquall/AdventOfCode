use std::collections::HashMap;

use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 8,
    title: "I Heard You Like Registers",

    solve_1: |input| {
        enum Operation {
            Increase,
            Decrease,
        }
        enum Condition {
            EqualTo,
            NotEqualTo,
            GreaterThan,
            GreaterThanOrEqualTo,
            LessThan,
            LessThanOrEqualTo,
        }

        fn test_condition(a: i32, b: i32, condition: Condition) -> bool {
            match condition {
                Condition::EqualTo => a == b,
                Condition::NotEqualTo => a != b,
                Condition::GreaterThan => a > b,
                Condition::GreaterThanOrEqualTo => a >= b,
                Condition::LessThan => a < b,
                Condition::LessThanOrEqualTo => a <= b,
            }
        }

        let mut registers = HashMap::new();

        for line in input.lines() {
            let mut iter = line.split_whitespace();
            let operation_register = iter.next().expect("Missing value");
            let operation = match iter.next() {
                Some("inc") => Operation::Increase,
                Some("dec") => Operation::Decrease,
                _ => panic!(),
            };
            let operation_amount: i32 = iter
                .next()
                .expect("Missing value")
                .parse()
                .expect("Error parsing number");
            // Ignore the "if"
            iter.next();
            let condition_register = iter.next().expect("Missing value");
            let condition = match iter.next() {
                Some("==") => Condition::EqualTo,
                Some("!=") => Condition::NotEqualTo,
                Some(">") => Condition::GreaterThan,
                Some(">=") => Condition::GreaterThanOrEqualTo,
                Some("<") => Condition::LessThan,
                Some("<=") => Condition::LessThanOrEqualTo,
                _ => panic!(),
            };
            let condition_amount: i32 = iter
                .next()
                .expect("Missing value")
                .parse()
                .expect("Error parsing number");

            let condition_register_ref = registers.entry(condition_register).or_insert(0);
            if test_condition(*condition_register_ref, condition_amount, condition) {
                let operation_register_ref = registers.entry(operation_register).or_insert(0);
                match operation {
                    Operation::Increase => *operation_register_ref += operation_amount,
                    Operation::Decrease => *operation_register_ref -= operation_amount,
                }
            }
        }

        let largest_register_ref = registers
            .values()
            .max()
            .expect("Error finding maximum value");
        Solution::I32(*largest_register_ref)
    },

    solve_2: |input| Solution::U8(0),
};

#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn example1_1() {
        assert_eq!(
            (SOLVER.solve_1)(
                "\
b inc 5 if a > 1
a inc 1 if b < 5
c dec -10 if a >= 1
c inc -20 if c == 10"
            ),
            Solution::U8(1)
        )
    }
}
