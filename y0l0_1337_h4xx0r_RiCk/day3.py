# Rucksacks with 2 compartments
# All items with given type in 1/2 comps
# Same number of items in each comp
# Prio = a-z 1-26, A-Z 27-52

with open('input_3_1') as file:
    data = file.read().splitlines()


def q3_1():
    totalprio = 0
    for rucksack in data:
        comp1 = rucksack[0:len(rucksack)//2]
        comp2 = rucksack[len(rucksack)//2:]
        risks = set()
        for char in comp1:
            if char in comp2:
                risks.add(char)
        for char in risks:
            if char.islower():
                totalprio += ord(char) - 96
            elif char.isupper():
                totalprio += ord(char) - 38
    print(totalprio)


def q3_2():
# Common item between 3 elves in group
    data_groups = [data[row:row+3] for row in range(len(data)) if row % 3 == 0]
    totalprio = 0
    for group in data_groups:
        for char in group[0]:
            if char in group[1] and char in group[2]:
                if char.islower():
                    totalprio += ord(char) - 96
                elif char.isupper():
                    totalprio += ord(char) - 38
                break
    print(totalprio)
# q3_1()
q3_2()