use std::collections::HashMap;

use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 13,
    title: "Packet Scanners",

    solve_1: |input| {
        let mut firewall: HashMap<u8, Layer> = HashMap::new();
        let mut greatest_depth = 0;

        for line in input.lines() {
            let mut iter = line.split(": ");

            let depth = iter
                .next()
                .expect("Line shouldn't be empty")
                .parse()
                .expect("Error parsing number");
            let range = iter
                .next()
                .expect("Line shouldn't be empty")
                .parse()
                .expect("Error parsing number");

            let layer = Layer {
                range,
                position: 0,
                is_moving_down: true,
            };
            firewall.insert(depth, layer);

            if depth > greatest_depth {
                greatest_depth = depth;
            }
        }

        let mut total_severity = 0;

        for depth in 0..=greatest_depth {
            if advance_picosecond(depth, &mut firewall) {
                total_severity += depth as u32 * firewall[&depth].range as u32;
            }
        }

        // Advances all scanners by one step. Returns true if caught at this step.
        fn advance_picosecond(depth: u8, firewall: &mut HashMap<u8, Layer>) -> bool {
            // A packet is only caught if the scanner is at the top before moving, so check if
            // caught only before updating the scanner position.
            let is_caught = firewall
                .get(&depth)
                .is_some_and(|layer| layer.position == 0);

            update_scanners(firewall);

            is_caught
        }

        Solution::U32(total_severity)
    },

    solve_2: |input| Solution::U8(0),
};

struct Layer {
    range: u8,
    position: u8,
    is_moving_down: bool,
}

fn update_scanners(firewall: &mut HashMap<u8, Layer>) {
    for layer in firewall.values_mut() {
        if layer.is_moving_down {
            if layer.position + 1 == layer.range {
                layer.is_moving_down = false;
                layer.position -= 1;
            } else {
                layer.position += 1;
            }
        } else if layer.position == 0 {
            layer.is_moving_down = true;
            layer.position += 1;
        } else {
            layer.position -= 1;
        }
    }
}

#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn example1_1() {
        assert_eq!(
            (SOLVER.solve_1)(
                "\
0: 3
1: 2
4: 4
6: 4"
            ),
            Solution::U8(24)
        )
    }
}
