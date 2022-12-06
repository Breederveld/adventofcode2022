with open('input_6_1') as file:
    data = file.read()


def q6_1():
    for char in range(4, len(data)):
        substr = data[char - 4: char]
        counts = [substr.count(c) for c in substr]
        if all(c == 1 for c in counts):
            print(char)
            break


def q6_2():
    for char in range(14, len(data)):
        substr = data[char - 14: char]
        counts = [substr.count(c) for c in substr]
        if all(c == 1 for c in counts):
            print(char)
            break


q6_1()
q6_2()