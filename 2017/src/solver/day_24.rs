use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 24,
    title: "Electromagnetic Moat",

    solve_1: |input| {
        #[derive(Clone, PartialEq)]
        struct Component {
            port0: u16,
            port1: u16,
        }

        // A recursive function to calculate the strongest bridge by recursively searching all
        // possible bridges.
        fn find_strongest_bridge(components: &Vec<Component>, port_to_match: u16) -> u16 {
            // Helper function to take a component, all remaining components, and the next port size
            // to match on, and calculate the highest strength.
            fn get_strength(
                component: &Component,
                components: &[Component],
                port_to_match: u16,
            ) -> u16 {
                // The list of components must be cloned so that reducing the list here doesn't
                // affect the overall list or the list of other recursive branches.
                let mut reduced_components = components.to_owned();
                // Remove the component that was just attached to the bridge.
                reduced_components.swap_remove(
                    reduced_components
                        .iter()
                        .position(|c| *c == *component)
                        .expect("Component should have come from components vector"),
                );

                // Get and return the strength by adding the numbers on both ports of the current
                // component, plus the maximum strength from all possible bridges from this point
                // on.
                component.port0
                    + component.port1
                    + find_strongest_bridge(&reduced_components, port_to_match)
            }

            let mut max_strength = 0;

            // For each component, if it matches the current port, use the get_strength function to recursively call the
            for component in components {
                if component.port0 == port_to_match {
                    let strength = get_strength(component, components, component.port1);
                    if strength > max_strength {
                        max_strength = strength;
                    }
                // Using else if prevents ports with the same number on both sides from being
                // searched twice.
                } else if component.port1 == port_to_match {
                    let strength = get_strength(component, components, component.port0);
                    if strength > max_strength {
                        max_strength = strength;
                    }
                }
            }

            max_strength
        }

        // Build the list of components.
        let mut components = Vec::new();
        for line in input.lines() {
            let mut iter = line.split('/');
            let port0 = iter
                .next()
                .expect("Line should have first element")
                .parse()
                .expect("Element should be a number");
            let port1 = iter
                .next()
                .expect("Line should have second element")
                .parse()
                .expect("Element should be a number");
            components.push(Component { port0, port1 });
        }

        Solution::U16(find_strongest_bridge(&components, 0))
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
0/2
2/2
2/3
3/4
3/5
0/1
10/1
9/10"
            ),
            Solution::U8(31)
        );
    }
}
