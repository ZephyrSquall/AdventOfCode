from abc import abstractmethod

class Solver:
    puzzle_title: str = "MISSING PUZZLE TITLE"

    @staticmethod
    @abstractmethod
    def solve_part_1() -> str:
        raise NotImplementedError("Implement part 1 solver")

    @staticmethod
    @abstractmethod
    def solve_part_2() -> str:
        raise NotImplementedError("Implement part 2 solver")