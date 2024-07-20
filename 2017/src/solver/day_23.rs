use super::{Solution, Solver};
use std::str::SplitWhitespace;

pub const SOLVER: Solver = Solver {
    day: 23,
    title: "Coprocessor Conflagration",

    solve_1: |input| {
        let instructions = get_instructions(input);
        let mut program_state = ProgramState {
            program_counter: 0,
            registers: [0; 8],
            mul_invocations: 0,
        };

        // The puzzle description does not state under what conditions the code in the puzzle input
        // terminates. If we assume that the puzzle has a finite answer, then at some point, the mul
        // instruction must stop getting invoked. I can think of two possibilities that can cause
        // this without an explicit termination condition: The program eventually falls into an
        // infinite loop that contains no mul instructions, or the program eventually jumps to an
        // instruction that is outside of the code. Through testing my own puzzle input, I have
        // verified that the latter is true (my code eventually jumps to an instruction outside of
        // the code), so I will assume this is the intended termination condition for all puzzle
        // inputs.
        while let Some(instruction) = instructions.get(program_state.program_counter) {
            instruction.execute(&mut program_state);
        }

        Solution::U32(program_state.mul_invocations)
    },

    #[allow(unused_variables, unreachable_code)]
    solve_2: |input| {
        // I currently cannot find a solution to this puzzle so I am skipping it for now. My current
        // attempt did not finish after running for several hours so it is clearly not the intended
        // solution, but I currently cannot think of any way to optimize my solution and am stumped.
        // So that other solutions aren't blocked, a return statement has been inserted at the
        // start of this solver to prevent the rest of it from executing.
        return Solution::U8(0);

        let instructions = get_instructions(input);
        let mut program_state = ProgramState {
            program_counter: 0,
            registers: [1, 0, 0, 0, 0, 0, 0, 0],
            mul_invocations: 0,
        };

        while let Some(instruction) = instructions.get(program_state.program_counter) {
            instruction.execute(&mut program_state);
        }

        // Register 'h' is the 8th register, so it has index 7.
        Solution::I64(program_state.registers[7])
    },
};

struct ProgramState {
    program_counter: usize,
    registers: [i64; 8],
    mul_invocations: u32,
}
// Note that unlike in day 18, it is specified that only the first 8 letters ('a' to 'h')
// are used. This is taken advantage of so that each letter is an index into an array of 8
// ints, rather than a HashMap. Hence the Register enum uses a usize instead of a char here.
enum RegisterOrValue {
    Register(usize),
    Value(i64),
}
// Representation of individual instructions.
enum Instruction {
    Set(RegisterOrValue, RegisterOrValue),
    Sub(RegisterOrValue, RegisterOrValue),
    Mul(RegisterOrValue, RegisterOrValue),
    Jnz(RegisterOrValue, RegisterOrValue),
}

impl RegisterOrValue {
    // Get the value from the operand, either by directly returning it if the operand is an
    // integer literal, or by fetching the value from the register if the operand is a
    // register name.
    fn get_value(&self, program_state: &ProgramState) -> i64 {
        match self {
            RegisterOrValue::Register(index) => program_state.registers[*index],
            RegisterOrValue::Value(value) => *value,
        }
    }
    // Get a mutable reference to the value in the operand. It's assumed this operand will
    // only be called for operands that are going to be written to (first operand of the
    // set, sub, and mul instructions), which only makes sense when the operand is a
    // register, so this function panics if the operand is a literal.
    fn get_value_mut<'a>(&self, program_state: &'a mut ProgramState) -> &'a mut i64 {
        match self {
            RegisterOrValue::Register(index) => &mut program_state.registers[*index],
            RegisterOrValue::Value(_) => {
                panic!("Should not be getting a mutable reference to a literal operand")
            }
        }
    }
}

impl Instruction {
    // Instruction mappings for part 1.
    fn execute(&self, program_state: &mut ProgramState) {
        match self {
            Instruction::Set(op_1, op_2) => {
                let op_2 = op_2.get_value(program_state);
                let op_1 = op_1.get_value_mut(program_state);
                *op_1 = op_2;
                program_state.program_counter += 1;
            }
            Instruction::Sub(op_1, op_2) => {
                let op_2 = op_2.get_value(program_state);
                let op_1 = op_1.get_value_mut(program_state);
                *op_1 -= op_2;
                program_state.program_counter += 1;
            }
            Instruction::Mul(op_1, op_2) => {
                let op_2 = op_2.get_value(program_state);
                let op_1 = op_1.get_value_mut(program_state);
                *op_1 *= op_2;
                program_state.program_counter += 1;
                program_state.mul_invocations += 1;
            }
            Instruction::Jnz(op_1, op_2) => {
                let op_1 = op_1.get_value(program_state);
                if op_1 != 0 {
                    let op_2 = op_2.get_value(program_state);
                    if op_2.is_negative() {
                        program_state.program_counter -= usize::try_from(op_2.unsigned_abs())
                            .expect("Should be able to convert to usize losslessly");
                    } else {
                        program_state.program_counter += usize::try_from(op_2)
                            .expect("Should be able to convert to usize losslessly");
                    }
                } else {
                    program_state.program_counter += 1;
                }
            }
        }
    }
}

fn get_instructions(input: &str) -> Vec<Instruction> {
    // Helper function to extract an operand.
    fn get_operand(iter: &mut SplitWhitespace) -> RegisterOrValue {
        let op = iter.next().expect("Line should have third value");
        let register_or_value = if let Ok(value) = op.parse() {
            RegisterOrValue::Value(value)
        } else {
            let mut index = op
                .chars()
                .next()
                .expect("Operand should be a number or single character")
                as usize;
            // ASCII value of 'a' is 97, so subtracting 97 gives an index from 0 to 7 (7
            // corresponding to 'h' with ASCII value 104).
            index -= 97;

            RegisterOrValue::Register(index)
        };

        register_or_value
    }

    let mut instructions = Vec::new();

    for line in input.lines() {
        let mut iter = line.split_whitespace();
        let instruction = iter.next().expect("Line should have first value");
        let instruction = match instruction {
            "set" => {
                let op_1 = get_operand(&mut iter);
                let op_2 = get_operand(&mut iter);
                Instruction::Set(op_1, op_2)
            }
            "sub" => {
                let op_1 = get_operand(&mut iter);
                let op_2 = get_operand(&mut iter);
                Instruction::Sub(op_1, op_2)
            }
            "mul" => {
                let op_1 = get_operand(&mut iter);
                let op_2 = get_operand(&mut iter);
                Instruction::Mul(op_1, op_2)
            }
            "jnz" => {
                let op_1 = get_operand(&mut iter);
                let op_2 = get_operand(&mut iter);
                Instruction::Jnz(op_1, op_2)
            }
            _ => panic!("Invalid instruction name"),
        };
        instructions.push(instruction);
    }

    instructions
}

// The puzzle description provides no examples for this puzzle.
