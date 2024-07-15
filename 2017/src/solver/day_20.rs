use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 20,
    title: "Particle Swarm",

    solve_1: |input| {
        // In the long term, position and velocity are irrelevant, as given enough time, eventually
        // any particle with a higher acceleration magnitude will end up with a higher velocity
        // magnitude, which in turn will eventually lead to a farther position from <0, 0, 0> (or
        // any other finite point). Hence in the long term, the particle that will stay closest to
        // <0, 0, 0> is the particle with the smallest acceleration magnitude.
        // (Magnitude in this case is calculated using Manhattan distance instead of the actual
        // direct line distance through 3D space, i.e. calculated by the sum of absolute values
        // instead of the square root of the sum of squares.)

        // Helper method to take a line from the puzzle input and calculate the acceleration
        // magnitude.
        fn get_acceleration_magnitude(line: &str) -> u32 {
            line.rsplit('<')
                .next()
                .expect("Line should have an acceleration vector")
                .trim_end_matches('>')
                .split(',')
                .fold(0, |acc, a| {
                    acc + a
                        .parse::<i32>()
                        .expect("Acceleration vector components should be numbers")
                        .unsigned_abs()
                })
        }

        let mut min_acceleration_magnitude = u32::MAX;
        let mut min_particle_id = usize::MAX;

        for (particle_id, line) in input.lines().enumerate() {
            let acceleration_magnitude = get_acceleration_magnitude(line);
            if acceleration_magnitude < min_acceleration_magnitude {
                min_acceleration_magnitude = acceleration_magnitude;
                min_particle_id = particle_id;
            }
        }

        Solution::USize(min_particle_id)
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
p=< 3,0,0>, v=< 2,0,0>, a=<-1,0,0>
p=< 4,0,0>, v=< 0,0,0>, a=<-2,0,0>"
            ),
            Solution::U8(0)
        );
    }
}
