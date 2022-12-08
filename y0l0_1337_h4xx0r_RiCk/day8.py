with open('input_8_1') as file:
    data = file.read().splitlines()
    data = [[int(char) for char in row] for row in data]


def visible_tree(row, col):
    if row == 0 or row == len(data) - 1 or col == 0 or col == len(data[0]) - 1:
        return True
    else:
        trees_left = data[row][0:col]
        trees_right = data[row][col+1:]
        trees_above = [tree_heights_row[col] for tree_row_num, tree_heights_row in enumerate(data) if tree_row_num < row]
        trees_below = [tree_heights_row[col] for tree_row_num, tree_heights_row in enumerate(data) if tree_row_num > row]

        if all(tree < data[row][col] for tree in trees_left) or \
            all(tree < data[row][col] for tree in trees_right) or \
            all(tree < data[row][col] for tree in trees_above) or \
            all(tree < data[row][col] for tree in trees_below):
            return True
        else:
            return False


def get_scenic_score(row, col):
    cur_height = data[row][col]

    trees_left = data[row][0:col]
    trees_left.reverse()
    trees_right = data[row][col+1:]
    trees_above = [tree_heights_row[col] for tree_row_num, tree_heights_row in enumerate(data) if tree_row_num < row]
    trees_above.reverse()
    trees_below = [tree_heights_row[col] for tree_row_num, tree_heights_row in enumerate(data) if tree_row_num > row]
    lists_together = [trees_left, trees_right, trees_above, trees_below]

    scenic_score = 1

    for tree_list in lists_together:
        if tree_list is None or not tree_list:
            return 0
        else:
            count = 0
            for tree in tree_list:
                if tree < cur_height:
                    count += 1
                else:
                    count += 1
                    break
        scenic_score *= count
    return scenic_score


def q8_1():
    visible_count = 0
    for row in range(len(data)):
        for col in range(len(data[row])):
            if visible_tree(row, col):
                visible_count += 1
    print(visible_count)


def q8_2():
    scenic_score_max = 0
    for row in range(len(data)):
        for col in range(len(data[row])):
            scen_score = get_scenic_score(row, col)
            if scen_score > scenic_score_max:
                scenic_score_max = scen_score
    print(scenic_score_max)


# q8_1()
q8_2()