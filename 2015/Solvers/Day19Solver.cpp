#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <functional>
#include "Day19Solver.h"

struct Replacement
{
    std::string initial;
    std::string final;
};

std::string AdventOfCode2015::Day19Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/19.txt");
    std::string line;

    std::vector<Replacement> replacements = {};
    std::string initial_molecule;

    while (getline(infile, line))
    {
        int initial_length = line.find(" => ");
        if (initial_length != std::string::npos)
        {
            // The initial element is all the characters up to " => ".
            std::string initial_element = line.substr(0, initial_length);
            // The final element is all the characters after the length of the initial element plus
            // the length of " => " (4).
            std::string final_elements = line.substr(initial_length + 4);

            replacements.push_back(Replacement{.initial = initial_element, .final = final_elements});
        }
        else
        {
            // Skip the empty line.
            if (line.size() != 0)
            {
                initial_molecule = line;
            }
        }
    }

    std::vector<std::string> final_molecules = {};

    // Check if the first character on its own can be replaced.
    std::string initial_one_character_element = initial_molecule.substr(0, 1);
    for (Replacement replacement : replacements)
    {
        // If a replacement is found, apply it.
        if (initial_one_character_element == replacement.initial)
        {
            std::string final_molecule = replacement.final + initial_molecule.substr(1);
            // Check if final_molecule already exists in the found final_molecules. If it does,
            // don't add it again to ensure all molecules in final_molecules are distinct.
            if (std::count(final_molecules.begin(), final_molecules.end(), final_molecule) == 0)
            {
                final_molecules.push_back(final_molecule);
            }
        }
    }

    // For every character after the first character, check if either that character on its own or
    // the two characters made from the previous and current character can be replaced.
    for (int i = 1; i < initial_molecule.size(); i++)
    {
        std::string one_character_element = initial_molecule.substr(i, 1);
        std::string two_character_element = initial_molecule.substr(i - 1, 2);

        for (Replacement replacement : replacements)
        {
            if (one_character_element == replacement.initial)
            {
                std::string final_molecule = initial_molecule.substr(0, i) + replacement.final + initial_molecule.substr(i + 1);
                if (std::count(final_molecules.begin(), final_molecules.end(), final_molecule) == 0)
                {
                    final_molecules.push_back(final_molecule);
                }
            }
            else if (two_character_element == replacement.initial)
            {
                std::string final_molecule = initial_molecule.substr(0, i - 1) + replacement.final + initial_molecule.substr(i + 1);
                if (std::count(final_molecules.begin(), final_molecules.end(), final_molecule) == 0)
                {
                    final_molecules.push_back(final_molecule);
                }
            }
        }
    }

    return std::to_string(final_molecules.size());
}

std::string AdventOfCode2015::Day19Solver::SolvePart2()
{
    return "0";
}