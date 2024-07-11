use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 17,
    title: "Spinlock",

    solve_1: |input| {
        let steps = input
            .parse::<usize>()
            .expect("Input should be single number");
        let mut circular_buffer: Vec<u16> = Vec::with_capacity(2018);
        circular_buffer.push(0);
        let mut index = 0;

        for value in 1..=2017 {
            // Vec::insert() inserts a new value at the given index, which means the value already
            // there is pushed after it. We actually want to insert the new value after the value
            // that is already there, so add 1 to the new index to get the index to point to the
            // position after the next value. This also means the index will be pointing to the
            // new value after insertion, so no further updates to the index are needed before
            // starting the next loop.
            index = (index + steps + 1) % circular_buffer.len();
            circular_buffer.insert(index, value);
        }

        // After the final insertion, index is pointing to the 2017 value, so move it forwards once
        // to get the index of the value after 2017.
        index = (index + 1) % circular_buffer.len();

        Solution::U16(circular_buffer[index])
    },

    solve_2: |input| Solution::U8(0),
};

#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn example1_1() {
        assert_eq!((SOLVER.solve_1)("3"), Solution::U16(638))
    }
}
