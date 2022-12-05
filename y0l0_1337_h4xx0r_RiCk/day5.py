with open('input_5_1') as file:
    data = file.read().splitlines()

the_split_index = data.index('')

stack_init = data[0:the_split_index - 1]
moves = data[the_split_index + 1:]
moves = [move.replace('move ', '').replace(' from ', ',').replace(' to ', ',').split(',') for move in moves]


def stack_parser():
    stacks = []

    for pos, char in enumerate(stack_init[-1]):
        if char != ' ' and char != '[' and char != ']':
            stack = [char]
            for level in range(len(stack_init) - 2, -1, -1):
                if pos < len(stack_init[level]) and stack_init[level][pos] != ' ':
                    stack.append(stack_init[level][pos])
            stacks.append(stack)
    return stacks


def q5_1():
    stacks = stack_parser()

    for move in moves:
        for rep in range(int(move[0])):
            stacks[int(move[2]) - 1].append(stacks[int(move[1]) - 1].pop())
    print(''.join([row[-1] for row in stacks]))


def q5_2():
    stacks = stack_parser()

    for move in moves:
        stacks[int(move[2]) - 1].extend(stacks[int(move[1]) - 1][-1 * int(move[0]):])
        del stacks[int(move[1]) - 1][-1 * int(move[0]):]
    print(''.join([row[-1] for row in stacks]))

q5_1()
q5_2()


