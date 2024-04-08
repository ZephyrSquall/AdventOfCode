#include <iostream>
#include <fstream>
#include <string>
#include "Day08Solver.h"

int AdventOfCode2015::Day08Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/08.txt");
    std::string line;

    int total_difference = 0;

    while (getline(infile, line))
    {
        int literal_size = line.size();

        std::string unquoted_line = line.substr(1, literal_size - 2);
        int code_size = 0;
        bool is_escaped_char = false;
        bool is_ascii_code = false;
        int ascii_count = 0;

        for (char c : unquoted_line)
        {
            if (is_ascii_code)
            {
                // if ascii_count = 1, then this is the second and therefore final character in the
                // ascii code.
                if (ascii_count == 1)
                {
                    code_size++;
                    ascii_count = 0;
                    is_ascii_code = false;
                }
                else
                {
                    ascii_count++;
                }
            }
            else if (is_escaped_char)
            {
                if (c == 'x')
                {
                    is_ascii_code = true;
                }
                // Handles case where the backslash escapes a quote or another backslash.
                else
                {
                    code_size++;
                }
                is_escaped_char = false;
            }
            else
            {
                // Check if c == '\' but the backslash needs to be escaped in C++.
                if (c == '\\')
                {
                    is_escaped_char = true;
                }
                else
                {
                    code_size++;
                }
            }
        }

        total_difference += literal_size;
        total_difference -= code_size;
    }

    return total_difference;
}

int AdventOfCode2015::Day08Solver::SolvePart2()
{
    return 0;
}