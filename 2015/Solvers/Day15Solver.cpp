#include <iostream>
#include <fstream>
#include <string>
#include <ranges>
#include <algorithm>
#include <vector>
#include <functional>
#include "Day15Solver.h"

struct Ingredient
{
    std::string name;
    int capacity;
    int durability;
    int flavor;
    int texture;
    int calories;
};

Ingredient get_ingredient(std::string line)
{
    Ingredient ingredient;

    auto name_and_rest = line | std::ranges::views::split(std::string_view{": capacity "});
    for (const auto &&[index, name_or_rest] : std::views::enumerate(name_and_rest))
    {
        // name
        if (index == 0)
        {
            // Convert view to string.
            std::string name;
            std::ranges::copy(name_or_rest | std::ranges::views::transform([](char c)
                                                                           { return (char)c; }),
                              std::back_inserter(name));

            ingredient.name = name;
        }
        // rest
        else
        {
            auto capacity_and_rest = name_or_rest | std::ranges::views::split(std::string_view{", durability "});
            for (const auto &&[index, capacity_or_rest] : std::views::enumerate(capacity_and_rest))
            {
                // capacity
                if (index == 0)
                {
                    // Convert view to string.
                    std::string capacity;
                    std::ranges::copy(capacity_or_rest | std::ranges::views::transform([](char c)
                                                                                       { return (char)c; }),
                                      std::back_inserter(capacity));

                    ingredient.capacity = stoi(capacity);
                }
                // rest
                else
                {
                    auto durability_and_rest = capacity_or_rest | std::ranges::views::split(std::string_view{", flavor "});
                    for (const auto &&[index, durability_or_rest] : std::views::enumerate(durability_and_rest))
                    {
                        // durability
                        if (index == 0)
                        {
                            // Convert view to string.
                            std::string durability;
                            std::ranges::copy(durability_or_rest | std::ranges::views::transform([](char c)
                                                                                                 { return (char)c; }),
                                              std::back_inserter(durability));

                            ingredient.durability = stoi(durability);
                        }
                        // rest
                        else
                        {
                            auto flavor_and_rest = durability_or_rest | std::ranges::views::split(std::string_view{", texture "});
                            for (const auto &&[index, flavor_or_rest] : std::views::enumerate(flavor_and_rest))
                            {
                                // flavor
                                if (index == 0)
                                {
                                    // Convert view to string.
                                    std::string flavor;
                                    std::ranges::copy(flavor_or_rest | std::ranges::views::transform([](char c)
                                                                                                     { return (char)c; }),
                                                      std::back_inserter(flavor));

                                    ingredient.flavor = stoi(flavor);
                                }
                                // rest
                                else
                                {
                                    auto texture_and_calories = flavor_or_rest | std::ranges::views::split(std::string_view{", calories "});
                                    for (const auto &&[index, texture_or_calories] : std::views::enumerate(texture_and_calories))
                                    {
                                        // Convert view to string.
                                        std::string texture_or_calories_str;
                                        std::ranges::copy(texture_or_calories | std::ranges::views::transform([](char c)
                                                                                                              { return (char)c; }),
                                                          std::back_inserter(texture_or_calories_str));
                                        // texture
                                        if (index == 0)
                                        {
                                            ingredient.texture = stoi(texture_or_calories_str);
                                        }
                                        // calories
                                        else
                                        {
                                            ingredient.calories = stoi(texture_or_calories_str);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    return ingredient;
}

std::string AdventOfCode2015::Day15Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/15.txt");
    std::string line;

    std::vector<Ingredient> ingredients = {};

    while (getline(infile, line))
    {
        ingredients.push_back(get_ingredient(line));
    }

    const int teaspoon_capacity = 100;

    // Get a vector representing the teaspoon amounts of the ingredient with the corresponding
    // index. Initialize it to all zeros except for the last ingredient, which is instead
    // initialized to the teaspoon capacity.
    std::vector<int> teaspoons(ingredients.size(), 0);
    teaspoons.back() = teaspoon_capacity;

    int max_cookie_score = 0;

    // Lambda to be called on every combination of teaspoons to calculate the cookie's score and
    // update the maximum cookie score if needed.
    std::function<void(std::vector<int>)> check_cookie_score = [ingredients, &max_cookie_score](std::vector<int> teaspoons)
    {
        int cookie_capacity = 0;
        int cookie_durability = 0;
        int cookie_flavor = 0;
        int cookie_texture = 0;

        for (int i = 0; i < teaspoons.size(); i++)
        {
            cookie_capacity += teaspoons[i] * ingredients[i].capacity;
            cookie_durability += teaspoons[i] * ingredients[i].durability;
            cookie_flavor += teaspoons[i] * ingredients[i].flavor;
            cookie_texture += teaspoons[i] * ingredients[i].texture;
        }

        int cookie_score;

        if (cookie_capacity <= 0 || cookie_durability <= 0 || cookie_flavor <= 0 || cookie_texture <= 0)
        {
            cookie_score = 0;
        }
        else
        {
            cookie_score = cookie_capacity * cookie_durability * cookie_flavor * cookie_texture;
        }

        if (cookie_score > max_cookie_score)
        {
            max_cookie_score = cookie_score;
        }
    };

    // Get every possible combination of teaspoon amounts up to 100 teaspoons. It is initially
    // called with every teaspoon amount = 0 except for the last one, whose amount = the teaspoon
    // capacity, and with the teaspoon_index for the last element in teaspoons
    // (teaspoons.size() - 1). At each step, the teaspoon capacity for the final teaspoon is
    // decremented and the next teaspoon down is incremented to keep the total teaspoon amount the
    // same, then this function is called again to perform this process again for the sub-array
    // ending one element sooner.
    std::function<void(int, std::vector<int>)> generate_teaspoons = [check_cookie_score, &generate_teaspoons](int teaspoon_index, std::vector<int> teaspoons)
    {
        if (teaspoon_index == 0)
        {
            check_cookie_score(teaspoons);
        }
        else
        {
            generate_teaspoons(teaspoon_index - 1, teaspoons);

            int remaining_capacity = teaspoons[teaspoon_index];
            for (int i = 0; i < remaining_capacity; i++)
            {
                teaspoons[teaspoon_index]--;
                teaspoons[teaspoon_index - 1]++;
                generate_teaspoons(teaspoon_index - 1, teaspoons);
            }
        }
    };

    generate_teaspoons(teaspoons.size() - 1, teaspoons);

    return std::to_string(max_cookie_score);
}

std::string AdventOfCode2015::Day15Solver::SolvePart2()
{
    return "0";
}