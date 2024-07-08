use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 14,
    title: "Disk Defragmentation",

    solve_1: |input| {
        let mut knot_hash_binaries = Vec::with_capacity(128);

        for row in 0..128 {
            let hash_input = format!("{}-{}", input, row);
            let knot_hash = (super::day_10::SOLVER.solve_2)(&hash_input).to_string();
            let knot_hash_binary =
                u128::from_str_radix(&knot_hash, 16).expect("Error parsing hash");

            knot_hash_binaries.push(knot_hash_binary);
        }

        let mut used_squares = 0;

        for knot_hash_binary in knot_hash_binaries {
            used_squares += knot_hash_binary.count_ones();
        }

        Solution::U32(used_squares)
    },

    solve_2: |input| Solution::U8(0),
};

#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn example1_1() {
        assert_eq!((SOLVER.solve_1)("flqrgnkx"), Solution::U16(8108))
    }
}
