use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 10,
    title: "Knot Hash",

    solve_1: |input| {
        let lengths = input
            .split(',')
            .map(|s| s.parse::<usize>().expect("Error parsing number"))
            .collect::<Vec<usize>>();

        Solution::USize(knot_hash(256, lengths))
    },

    solve_2: |input| Solution::U8(0),
};

fn knot_hash(list_size: usize, lengths: Vec<usize>) -> usize {
    let mut list = (0..list_size).collect::<Vec<usize>>();
    let mut position = 0;
    let mut skip_size = 0;

    for length in lengths {
        // A length of 0 needs to be explicitly skipped as it will set up invalid initial
        // conditions. Fortunately, a length of 0 is a no-op anyway so no additional handling is
        // needed.
        if length != 0 {
            let mut start_position = position;
            let mut end_position = (start_position + length - 1) % list.len();

            while start_position != end_position {
                list.swap(start_position, end_position);

                start_position = (start_position + 1) % list.len();
                // If length is even, the positions will be equal at this intermediate step instead
                // of when both start_position and end_position are updated, so add an extra check
                // here to end the loop if they're equal.
                if start_position == end_position {
                    break;
                }
                // In mod n arithmetic, adding n - 1 is equivalent to subtracting 1. This trick is
                // used here to prevent the possibility of subtracting 1 from a usize with value 0
                // which would cause an overflow.
                end_position = (end_position + list.len() - 1) % list.len();
            }
        }

        println!("{}: {:?}", length, list);

        position = (position + length + skip_size) % list.len();
        skip_size += 1;
    }

    list[0] * list[1]
}

#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn example1_1() {
        let input = "3, 4, 1, 5";
        let lengths = input
            .split(", ")
            .map(|s| s.parse::<usize>().expect("Error parsing number"))
            .collect::<Vec<usize>>();
        assert_eq!(Solution::USize(knot_hash(5, lengths)), Solution::U8(12))
    }
}
