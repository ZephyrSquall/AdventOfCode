use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 6,
    title: "Memory Reallocation",

    solve_1: |input| {
        // Use the input to initialize the first bank configuration and bank history.
        let mut banks: Box<[u32]> = input
            .split_whitespace()
            .map(|s| s.parse::<u32>().expect("Error parsing number"))
            .collect();
        let num_banks = banks.len();
        // banks must be cloned so the real banks array can continue to be mutated without changing
        // history.
        let mut banks_history = vec![banks.clone()];
        let mut step: u32 = 1;

        loop {
            // Find the max value and the index of this value.
            let mut max_bank = u32::min_value();
            let mut max_bank_index = 0;
            for (index, bank) in banks.iter().enumerate() {
                if *bank > max_bank {
                    max_bank = *bank;
                    max_bank_index = index;
                }
            }

            // Distribute blocks.
            banks[max_bank_index] = 0;
            while max_bank > 0 {
                max_bank -= 1;
                // modulo operator ensures index wraps around to 0 as needed.
                max_bank_index = (max_bank_index + 1) % num_banks;

                banks[max_bank_index] += 1;
            }

            if banks_history.contains(&banks) {
                return Solution::U32(step);
            }
            banks_history.push(banks.clone());
            step += 1
        }
    },
    solve_2: |input| Solution::U8(0),
};

#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn example1_1() {
        assert_eq!((SOLVER.solve_1)("0 2 7 0"), Solution::U8(5))
    }
}
