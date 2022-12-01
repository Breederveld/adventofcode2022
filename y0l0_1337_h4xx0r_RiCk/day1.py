with open("input_1_1") as file:
    data = file.read().splitlines()

def q1_1():
    max = 0
    total_tmp = 0

    for line_num, line in enumerate(data):
        if line != "":
            total_tmp += int(line)

        if line_num == len(data) or line == "":
            if total_tmp > max:
                max = total_tmp
            total_tmp = 0
    print(max)


def q1_2():
    totals = []
    total_tmp = 0

    for line_num, line in enumerate(data):
        if line != "":
            total_tmp += int(line)

        if line_num == len(data) or line == "":
            totals.append(total_tmp)
            total_tmp = 0
    totals.sort(reverse=True)
    print(sum(totals[0:3]))


q1_2()