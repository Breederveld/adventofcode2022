import math

with open('input_10_1') as file:
    data = file.read().splitlines()


def q_10():
    x_curr = 1
    # Value after instruction at index
    x = [1]

    for instruction in data:
        inst_lst = instruction.split(' ')
        if inst_lst[0] == 'noop':
            x.append(x_curr)
        elif inst_lst[0] == 'addx':
            x.append(x_curr)
            x_curr += int(inst_lst[1])
            x.append(x_curr)
    # print(f'Cycles total: {len(x)}')

    signal_total = 0
    strengths = []
    for cycle, x_after in enumerate(x):
        # 20, 60, 100
        if (cycle + 20) % 40 == 0 and cycle <= 220:
            x_during = x[cycle-1]
            signal_strength = cycle * x_during
            strengths.append(signal_strength)
            signal_total += signal_strength
    # print(strengths)
    print(f'Sum signal strengts: {signal_total}')

    # q10_2
    # x sets middle of 3px sprite
    # CRT 40x6
    # 1px per cycle
    # pixel = 1 of sprite px == crtpos
    frame = []
    for crt_pos, sprite in enumerate(x[1:]):
        if crt_pos % 40 in range(sprite-2, sprite+1):
            px = '#'
        else:
            px = '.'
        frame.append(px)
    for pos, char in enumerate(frame):
        if (pos) % 40 == 0:
            print('')
        print(char, end = "")



q_10()
# PCPBKAPJ