#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <ranges>
#include <algorithm>
#include <functional>
#include "Day09Solver.h"

struct Edge
{
    std::string source_node;
    std::string target_node;
    int weight;
};

Edge get_edge(std::string line)
{
    Edge edge;

    auto nodes_and_weight = line | std::ranges::views::split(std::string_view{" = "});
    for (const auto &&[index, nodes_and_weight] : std::views::enumerate(nodes_and_weight))
    {
        if (index == 0)
        {
            auto nodes = nodes_and_weight | std::ranges::views::split(std::string_view{" to "});
            for (const auto &&[index, nodes] : std::views::enumerate(nodes))
            {
                // Convert view to string.
                std::string node_str;
                std::ranges::copy(nodes | std::ranges::views::transform([](char c)
                                                                        { return (char)c; }),
                                  std::back_inserter(node_str));

                if (index == 0)
                {
                    edge.source_node = node_str;
                }
                else
                {
                    edge.target_node = node_str;
                }
            }
        }
        else
        {
            // Convert view to string.
            std::string weight_str;
            std::ranges::copy(nodes_and_weight | std::ranges::views::transform([](char c)
                                                                               { return (char)c; }),
                              std::back_inserter(weight_str));

            edge.weight = stoi(weight_str);
        }
    }

    return edge;
}

int AdventOfCode2015::Day09Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/09.txt");
    std::string line;

    std::vector<std::string> nodes = {};
    std::vector<Edge> edges = {};

    while (getline(infile, line))
    {
        Edge edge = get_edge(line);

        if (std::count(nodes.begin(), nodes.end(), edge.source_node) == 0)
        {
            nodes.push_back(edge.source_node);
        }
        if (std::count(nodes.begin(), nodes.end(), edge.target_node) == 0)
        {
            nodes.push_back(edge.target_node);
        }

        edges.push_back(edge);
    }

    int minimum_distance = INT_MAX;
    int max_depth = nodes.size();
    std::vector<std::string> explored_nodes = {};

    // Perform a depth-first search to explore every path. The following lambda function will be
    // executed recursively to explore all possible paths between nodes.
    std::function<void(std::vector<std::string>, int)> find_next_node = [edges, &minimum_distance, max_depth, &find_next_node](std::vector<std::string> explored_nodes, int current_distance)
    {
        // If explored_nodes has every node, then a full path has been found. This is guaranteed to
        // be the shortest path found so far since the path would have been aborted if its distance
        // were above min_distance, so update minimum_distance (which is captured in this lambda by
        // reference) to the current_distance.
        if (explored_nodes.size() == max_depth)
        {
            minimum_distance = current_distance;
            return;
        }

        std::string current_node = explored_nodes.back();

        // Check each edge to see if it has potential to go to the next step of a new shortest path.
        for (Edge edge : edges)
        {
            std::string next_node;
            int next_distance;

            if (edge.source_node == current_node)
            {
                next_node = edge.target_node;
            }
            else if (edge.target_node == current_node)
            {
                next_node = edge.source_node;
            }
            else
            {
                // The edge does not links to the current node, so skip to the next edge.
                continue;
            }

            next_distance = current_distance + edge.weight;
            if (next_distance >= minimum_distance)
            {
                // This path exceeds the minimum distance and can no longer be the shortest path, so
                // skip to the next edge.
                continue;
            }

            if (std::count(explored_nodes.begin(), explored_nodes.end(), next_node) != 0)
            {
                // The edge goes to a node that has already been visited, so skip to the next edge.
                continue;
            }

            // This edge has the potential to be the next shortest path, so update the explored
            // nodes and repeat this search from the next node.
            std::vector<std::string> next_explored_nodes = explored_nodes;
            next_explored_nodes.push_back(next_node);
            find_next_node(next_explored_nodes, next_distance);
        }
    };

    // Start calling the recursive find_next_node once for each possible starting node (with a path
    // that consists only of the starting node).
    for (std::string node : nodes)
    {
        // Do not search for paths from the final location, as every path leading into the location
        // has already been explored. Any path found by exploring from the final location will just
        // be the reverse of an already-found path.
        if (node == nodes.back())
        {
            break;
        }

        explored_nodes = {node};
        int current_distance = 0;

        find_next_node(explored_nodes, current_distance);
    }

    return minimum_distance;
}

int AdventOfCode2015::Day09Solver::SolvePart2()
{
    return 0;
}