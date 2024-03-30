#include <iostream>
#include <fstream>
#include <string>
#include <cmath>
#include <stdint.h>
#include <bitset>
#include <format>
#include "Day04Solver.h"

// Note that this MD5 implementation does not properly handle multiple blocks. It will only be
// correct if the input string is small enough that it requires only a single 512-bit block.
static std::string md5(std::string str)
{
    std::string input = str;
    std::string input_ascii = "";

    // Convert each character into its 8-bit ASCII code then convert it into a binary string.
    for (int i = 0; i < input.size(); i++)
    {
        input_ascii += std::bitset<8>(input.c_str()[i]).to_string();
    }

    // Store the size of the input as a 64-bit binary string (padded with 0s) for the final input
    // padding step.
    int input_size = input_ascii.size();
    std::string input_size_binary_string = std::bitset<64>(input_size).to_string();

    int s[64] = {
        7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
        5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20,
        4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
        6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21};

    uint32_t k[64];
    for (int i = 0; i < 64; i++)
    {
        k[i] = floorl(pow(2, 32) * fabs(sin(i + 1)));
    }

    uint32_t initial_a = 0x67452301;
    uint32_t initial_b = 0xefcdab89;
    uint32_t initial_c = 0x98badcfe;
    uint32_t initial_d = 0x10325476;

    // Pad input to 512 bits.
    input_ascii += "1";
    while (input_ascii.size() % 512 != 448)
    {
        input_ascii += "0";
    }
    input_ascii += input_size_binary_string;

    // Split input into 32-bit blocks. Convert these blocks into 32-bit integers.
    uint32_t m[16];
    for (int i = 0; i < 16; i++)
    {
        std::string input_substring = input_ascii.substr(i * 32, 32);
        m[i] = std::stoll(input_substring, nullptr, 2);

        // The endianness of the ascii string is wrong, so each 32-bit integer needs its bytes
        // reversed. However, the length string has the correct endianness, so don't swap its bytes.
        if (i < 14)
        {
            m[i] = std::byteswap(m[i]);
        }
    }

    uint32_t m_temp = m[15];
    m[15] = m[14];
    m[14] = m_temp;

    uint32_t a = initial_a;
    uint32_t b = initial_b;
    uint32_t c = initial_c;
    uint32_t d = initial_d;

    // Note that addition needs to be done modulo 2^32. However, as the numbers are stored in
    // unsigned 32-bit integers, they will naturally overflow and thus produce mod 2^32 arithmetic
    // automatically.
    for (int i = 0; i < 64; i++)
    {
        uint32_t f;
        int g;

        if (i < 16)
        {
            f = (b & c) | ((~b) & d);
            g = i;
        }
        else if (i < 32)
        {
            f = (b & d) | (c & (~d));
            g = ((5 * i) + 1) % 16;
        }
        else if (i < 48)
        {
            f = b ^ c ^ d;
            g = ((3 * i) + 5) % 16;
        }
        else
        {
            f = c ^ (b | (~d));
            g = (7 * i) % 16;
        }

        f += a + k[i] + m[g];
        a = d;
        d = c;
        c = b;
        b += std::rotl(f, s[i]);
    }

    initial_a += a;
    initial_b += b;
    initial_c += c;
    initial_d += d;

    // The output has incorrect endianness, so swap the bytes of each number.
    // (I have no idea why the endianness of the final output is wrong, this is something I need to
    // look into further to be completely satisfied with this MD5 implementation.)
    initial_a = std::byteswap(initial_a);
    initial_b = std::byteswap(initial_b);
    initial_c = std::byteswap(initial_c);
    initial_d = std::byteswap(initial_d);

    std::string output = std::format("{:0>8x}{:0>8x}{:0>8x}{:0>8x}", initial_a, initial_b, initial_c, initial_d);

    return output;
}

int AdventOfCode2015::Day04Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/04.txt");
    std::string line;
    std::getline(infile, line);

    int i = 0;
    std::string hash = md5(line + std::to_string(i));

    while (hash.substr(0, 5) != "00000")
    {
        i++;
        std::string input = line + std::to_string(i);
        hash = md5(input);
    }

    return i;
}

int AdventOfCode2015::Day04Solver::SolvePart2()
{
    std::ifstream infile("PuzzleInputs/04.txt");
    std::string line;
    std::getline(infile, line);

    int i = 0;
    std::string hash = md5(line + std::to_string(i));

    while (hash.substr(0, 6) != "000000")
    {
        i++;
        std::string input = line + std::to_string(i);
        hash = md5(input);
    }

    return i;
}
