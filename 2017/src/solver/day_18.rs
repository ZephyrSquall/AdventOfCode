use std::{collections::HashMap, str::SplitWhitespace};

use super::{Solution, Solver};

pub const SOLVER: Solver = Solver {
    day: 18,
    title: "Duet",

    solve_1: |input| {
        // Representation of data necessary to run the program.
        struct ProgramState {
            program_counter: usize,
            last_frequency: Option<i64>,
            registers: HashMap<char, i64>,
            is_terminating: bool,
        }
        // Representation of the second operand of some instructions, which can either be a register
        // name or an integer literal.
        enum RegisterOrValue {
            Register(char),
            Value(i64),
        }
        // Representation of individual instructions.
        enum Instruction {
            Snd(char),
            Set(char, RegisterOrValue),
            Add(char, RegisterOrValue),
            Mul(char, RegisterOrValue),
            Mod(char, RegisterOrValue),
            Rcv(char),
            Jgz(char, RegisterOrValue),
        }

        impl RegisterOrValue {
            // Get the value from the operand, either by directly returning it if the operand is an
            // integer literal, or by fetching the value from the register if the operand is a
            // register name.
            fn get_value(&self, program_state: &ProgramState) -> i64 {
                match self {
                    RegisterOrValue::Register(register) => {
                        *program_state.registers.get(register).unwrap_or(&0)
                    }
                    RegisterOrValue::Value(value) => *value,
                }
            }
        }
        impl Instruction {
            fn execute(&self, program_state: &mut ProgramState) {
                match self {
                    Instruction::Snd(op_1) => {
                        let op_1 = program_state.registers.get(op_1).unwrap_or(&0);
                        program_state.last_frequency = Some(*op_1);
                        program_state.program_counter += 1;
                    }
                    Instruction::Set(op_1, op_2) => {
                        let op_2 = op_2.get_value(program_state);
                        program_state.registers.insert(*op_1, op_2);
                        program_state.program_counter += 1;
                    }
                    Instruction::Add(op_1, op_2) => {
                        let op_2 = op_2.get_value(program_state);
                        let op_1 = program_state.registers.entry(*op_1).or_insert(0);
                        *op_1 += op_2;
                        program_state.program_counter += 1;
                    }
                    Instruction::Mul(op_1, op_2) => {
                        let op_2 = op_2.get_value(program_state);
                        let op_1 = program_state.registers.entry(*op_1).or_insert(0);
                        *op_1 *= op_2;
                        program_state.program_counter += 1;
                    }
                    Instruction::Mod(op_1, op_2) => {
                        let op_2 = op_2.get_value(program_state);
                        let op_1 = program_state.registers.entry(*op_1).or_insert(0);
                        *op_1 %= op_2;
                        program_state.program_counter += 1;
                    }
                    Instruction::Rcv(op_1) => {
                        let op_1 = program_state.registers.get(op_1).unwrap_or(&0);
                        if *op_1 != 0 {
                            program_state.is_terminating = true;
                        } else {
                            program_state.program_counter += 1;
                        }
                    }
                    Instruction::Jgz(op_1, op_2) => {
                        let op_1 = program_state.registers.get(op_1).unwrap_or(&0);
                        if *op_1 > 0 {
                            let op_2 = op_2.get_value(program_state);
                            if op_2.is_negative() {
                                program_state.program_counter -= op_2.unsigned_abs() as usize;
                            } else {
                                program_state.program_counter += op_2 as usize;
                            }
                        } else {
                            program_state.program_counter += 1;
                        }
                    }
                }
            }
        }

        // Helper function to extract operands for the snd and rcv instructions.
        fn get_single_operand(mut iter: SplitWhitespace) -> char {
            iter.next()
                .expect("Line should have second value")
                .chars()
                .next()
                .expect("Operand should be single character")
        }
        // Helper function to extract operands for the set, add, mul, mod, and jgz instructions.
        fn get_two_operands(mut iter: SplitWhitespace) -> (char, RegisterOrValue) {
            let char = iter
                .next()
                .expect("Line should have second value")
                .chars()
                .next()
                .expect("Operand should be single character");

            let op_2 = iter.next().expect("Line should have third value");
            let register_or_value = match op_2.parse() {
                Ok(value) => RegisterOrValue::Value(value),
                Err(_) => {
                    let char = op_2
                        .chars()
                        .next()
                        .expect("Operand should be a number or single character");
                    RegisterOrValue::Register(char)
                }
            };

            (char, register_or_value)
        }

        let mut instructions = Vec::new();
        let mut program_state = ProgramState {
            program_counter: 0,
            last_frequency: None,
            registers: HashMap::new(),
            is_terminating: false,
        };
        // Parse the input file into a Vector of Instructions.
        for line in input.lines() {
            let mut iter = line.split_whitespace();
            let instruction = iter.next().expect("Line should have first value");
            let instruction = match instruction {
                "snd" => {
                    let op_1 = get_single_operand(iter);
                    Instruction::Snd(op_1)
                }
                "set" => {
                    let (op_1, op_2) = get_two_operands(iter);
                    Instruction::Set(op_1, op_2)
                }
                "add" => {
                    let (op_1, op_2) = get_two_operands(iter);
                    Instruction::Add(op_1, op_2)
                }
                "mul" => {
                    let (op_1, op_2) = get_two_operands(iter);
                    Instruction::Mul(op_1, op_2)
                }
                "mod" => {
                    let (op_1, op_2) = get_two_operands(iter);
                    Instruction::Mod(op_1, op_2)
                }
                "rcv" => {
                    let op_1 = get_single_operand(iter);
                    Instruction::Rcv(op_1)
                }
                "jgz" => {
                    let (op_1, op_2) = get_two_operands(iter);
                    Instruction::Jgz(op_1, op_2)
                }
                _ => panic!("Invalid instruction name"),
            };
            instructions.push(instruction);
        }

        // Repeatedly execute instructions until an instruction sets the is_terminating flag (which
        // happens then rcv is called with a non-zero value). The program counter tracks the index
        // of the next instruction to execute.
        while !program_state.is_terminating {
            instructions[program_state.program_counter].execute(&mut program_state);
        }

        if let Some(last_frequency) = program_state.last_frequency {
            Solution::I64(last_frequency)
        } else {
            panic!("No frequency was ever sounded")
        }
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
set a 1
add a 2
mul a a
mod a 5
snd a
set a 0
rcv a
jgz a -1
set a 1
jgz a -2"
            ),
            Solution::U8(4)
        )
    }
}
