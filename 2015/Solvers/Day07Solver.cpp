#include <iostream>
#include <fstream>
#include <string>
#include <stdint.h>
#include <variant>
#include <map>
#include <vector>
#include <ranges>
#include <algorithm>
#include "Day07Solver.h"

enum class Gate
{
    And,
    Or,
    LeftShift,
    RightShift,
    Not,
    // Used to represent no gate, simply passing a value from one variable to another.
    Assign,
};

struct Operation
{
    std::variant<uint16_t, std::string> operand1;
    std::variant<uint16_t, std::string> operand2;
    Gate gate;
    std::string result;
    bool complete = false;
};

Operation get_operation(std::string line)
{
    Operation operation;

    // These variables are solely used for string splitting.
    bool two_operands = true;
    std::string split_string;

    // Identify the gate type.
    if (line.contains("AND"))
    {
        operation.gate = Gate::And;
        split_string = " AND ";
    }
    else if (line.contains("OR"))
    {
        operation.gate = Gate::Or;
        split_string = " OR ";
    }
    else if (line.contains("LSHIFT"))
    {
        operation.gate = Gate::LeftShift;
        split_string = " LSHIFT ";
    }
    else if (line.contains("RSHIFT"))
    {
        operation.gate = Gate::RightShift;
        split_string = " RSHIFT ";
    }
    else if (line.contains("NOT"))
    {
        operation.gate = Gate::Not;
        // Since the "NOT " appears at the start of the line, it can easily be removed by taking
        // a substring, which simplifies string splitting in the one_operand case.
        line = line.substr(4);
        two_operands = false;
    }
    else
    {
        operation.gate = Gate::Assign;
        two_operands = false;
    }

    // Perform the string splitting.
    if (two_operands)
    {
        auto variables = line | std::ranges::views::split(std::string_view{split_string});

        for (const auto &&[index, variable_view] : std::views::enumerate(variables))
        {
            // index 0 corresponds to the value to the left of the gate name, which is
            // operand1.
            if (index == 0)
            {
                // Convert view to string.
                std::string variable_str;
                std::ranges::copy(variable_view | std::ranges::views::transform([](char c)
                                                                                { return (char)c; }),
                                  std::back_inserter(variable_str));

                uint16_t variable_num;
                try
                {
                    variable_num = stol(variable_str);
                    operation.operand1 = variable_num;
                }
                catch (const std::invalid_argument)
                {
                    operation.operand1 = variable_str;
                }
            }
            // index 1 corresponds to the value to the right of the gate name, which is another
            // string that needs to be split further.
            else
            {
                auto inner_variables = variable_view | std::ranges::views::split(std::string_view{" -> "});
                for (const auto &&[inner_index, inner_variable_view] : std::views::enumerate(inner_variables))
                {
                    // Convert view to string.
                    std::string inner_variable_str;
                    std::ranges::copy(inner_variable_view | std::ranges::views::transform([](char c)
                                                                                          { return (char)c; }),
                                      std::back_inserter(inner_variable_str));

                    // index 0 corresponds to the value left of the " -> " substring, which is
                    // operand2. index 1 corresponds to the value right of this substring, which
                    // is result.
                    switch (inner_index)
                    {
                    case 0:
                        uint16_t variable_num;
                        try
                        {
                            variable_num = stol(inner_variable_str);
                            operation.operand2 = variable_num;
                        }
                        catch (const std::invalid_argument)
                        {
                            operation.operand2 = inner_variable_str;
                        }
                        break;
                    case 1:
                        operation.result = inner_variable_str;
                        break;
                    }
                }
            }
        }
    }
    else
    {
        auto variables = line | std::ranges::views::split(std::string_view{" -> "});
        for (const auto &&[index, variable_view] : std::views::enumerate(variables))
        {
            // Convert view to string.
            std::string variable_str;
            std::ranges::copy(variable_view | std::ranges::views::transform([](char c)
                                                                            { return (char)c; }),
                              std::back_inserter(variable_str));

            // For the one_operand case (NOT or ASSIGN), index 0 corresponds to operand1 and
            // index 1 corresponds to result. There is no operand2, so leave it unassigned.
            switch (index)
            {
            case 0:
                uint16_t variable_num;
                try
                {
                    variable_num = stol(variable_str);
                    operation.operand1 = variable_num;
                }
                catch (const std::invalid_argument)
                {
                    operation.operand1 = variable_str;
                }
                break;
            case 1:
                operation.result = variable_str;
                break;
            }
        }
    }

    return operation;
}

std::map<std::string, uint16_t> execute_operations(std::vector<Operation> operations)
{
    std::map<std::string, uint16_t> values;

    // Loop over each operand. For each operand, check if its operands are defined (either a literal
    // or a variable whose value is defined in the values map), and if so, execute the gate
    // operation and store the result in the values map under the result name, then remove the
    // operation. When the end of the loop is reached, return to the start and repeat. Continue
    // until all operations are removed.
    while (operations.size() > 0)
    {
        std::vector<int> operation_indexes_to_remove;
        int remaining_operations = operations.size();
        for (Operation &operation : operations)
        {
            uint16_t operand1_num;
            bool operand1_defined = false;
            uint16_t operand2_num;
            bool operand2_defined = false;
            uint16_t result_num;

            // Attempt to fetch operand1's value.
            if (std::holds_alternative<uint16_t>(operation.operand1))
            {
                operand1_num = std::get<uint16_t>(operation.operand1);
                operand1_defined = true;
            }
            else
            {
                std::string operand1_str = std::get<std::string>(operation.operand1);
                if (values.contains(operand1_str))
                {
                    operand1_num = values[operand1_str];
                    operand1_defined = true;
                }
            }

            // Attempt to fetch operand2's value. Skip if the gate is NOT or ASSIGN as these do not
            // require a second operand.
            if (operation.gate != Gate::Not && operation.gate != Gate::Assign)
            {
                if (std::holds_alternative<uint16_t>(operation.operand2))
                {
                    operand2_num = std::get<uint16_t>(operation.operand2);
                    operand2_defined = true;
                }
                else
                {
                    std::string operand2_str = std::get<std::string>(operation.operand2);
                    if (values.contains(operand2_str))
                    {
                        operand2_num = values[operand2_str];
                        operand2_defined = true;
                    }
                }
            }

            // Perform the corresponding operation if the necessary operands are defined.
            switch (operation.gate)
            {
            case (Gate::And):
                if (operand1_defined && operand2_defined)
                {
                    result_num = operand1_num & operand2_num;
                    operation.complete = true;
                }
                break;
            case (Gate::Or):
                if (operand1_defined && operand2_defined)
                {
                    result_num = operand1_num | operand2_num;
                    operation.complete = true;
                }
                break;
            case (Gate::LeftShift):
                if (operand1_defined && operand2_defined)
                {
                    result_num = operand1_num << operand2_num;
                    operation.complete = true;
                }
                break;
            case (Gate::RightShift):
                if (operand1_defined && operand2_defined)
                {
                    result_num = operand1_num >> operand2_num;
                    operation.complete = true;
                }
                break;
            case (Gate::Not):
                if (operand1_defined)
                {
                    result_num = ~operand1_num;
                    operation.complete = true;
                }
                break;
            case (Gate::Assign):
                if (operand1_defined)
                {
                    result_num = operand1_num;
                    operation.complete = true;
                }
                break;
            }

            // If the operation completed, store the result in the values map to be used by future
            // operations.
            if (operation.complete == true)
            {
                values.insert({operation.result, result_num});
            }
        }

        // Remove gates which completed their operation.
        std::erase_if(operations, [](Operation operation)
                      { return operation.complete; });
    }

    return values;
}

std::string AdventOfCode2015::Day07Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/07.txt");
    std::string line;

    std::vector<Operation> operations;

    while (getline(infile, line))
    {
        operations.push_back(get_operation(line));
    }

    std::map<std::string, uint16_t> values = execute_operations(operations);

    // Store the answer for part 1 because part 2 requires it.
    AdventOfCode2015::Day07Solver::part_1_answer = values["a"];

    return std::to_string(values["a"]);
}

std::string AdventOfCode2015::Day07Solver::SolvePart2()
{
    std::ifstream infile("PuzzleInputs/07.txt");
    std::string line;

    std::vector<Operation> operations;

    while (getline(infile, line))
    {
        Operation operation = get_operation(line);

        // If the operation assigns a value to "b", replace its operand with the answer to part 1.
        if(operation.result == "b")
        {
            operation.operand1 = (uint16_t)AdventOfCode2015::Day07Solver::part_1_answer;
        }

        operations.push_back(operation);
    }

    std::map<std::string, uint16_t> values = execute_operations(operations);

    return std::to_string(values["a"]);
}