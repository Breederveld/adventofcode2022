# Section IDs >> Elf

with open('input_4_1') as file:
    data = file.read().split()
    data = [row.split(',') for row in data]

def check_all_in_range_both_ways(minmax1, minmax2):
    list1 = list(range(int(minmax1[0]), int(minmax1[1]) + 1))
    list2 = list(range(int(minmax2[0]), int(minmax2[1]) + 1))

    regular = all(id in list2 for id in list1)
    reverse = all(id in list1 for id in list2)

    return regular or reverse


def check_any_in_range_both_ways(minmax1, minmax2):
    list1 = list(range(int(minmax1[0]), int(minmax1[1]) + 1))
    list2 = list(range(int(minmax2[0]), int(minmax2[1]) + 1))

    regular = any(id in list2 for id in list1)
    reverse = any(id in list1 for id in list2)

    return regular or reverse


def q4_1():
    countpairs = 0
    for elfduo_range in data:
        elf1 = elfduo_range[0].split('-')
        elf2 = elfduo_range[1].split('-')
        if check_all_in_range_both_ways(elf1, elf2):
            countpairs += 1
    print(countpairs)

def q4_2():
    countpairs = 0
    for elfduo_range in data:
        elf1 = elfduo_range[0].split('-')
        elf2 = elfduo_range[1].split('-')
        if check_any_in_range_both_ways(elf1, elf2):
            countpairs += 1
    print(countpairs)

q4_1()
q4_2()