with open('input_7_1') as file:
    data = file.read().splitlines()


def dir_parser():
    tree = {'/': []}
    curpath = None
    curpathstr = None

    for line in data:
        lineparts = line.split(' ')
        if lineparts[0] == '$':
            if lineparts[1] == 'cd':
                if lineparts[2] == '/':
                    curpath = ['/']
                    curpathstr = '_'.join(curpath)
                elif lineparts[2] == '..':
                    curpath.pop()
                else:
                    curpath.append(lineparts[2])
                    curpathstr = '_'.join(curpath)
                    if curpathstr not in tree:
                        tree[curpathstr] = []
            elif lineparts[1] == 'ls':
                pass
        elif lineparts[0] == 'dir':
            pass
        else:
            tree[curpathstr].append(line)

    return tree


def get_dir_size(target_node, tree):
    relevant_nodes = [node for node in tree if node.startswith(target_node)]
    total = 0
    for node in relevant_nodes:
        total += sum([int(file.split(' ')[0]) for file in tree[node]])
    return total

def q7_1():
    fs_tree = dir_parser()
    total_sum = 0
    for node in fs_tree:
        size = get_dir_size(node, fs_tree)
        if size <= 100000:
            total_sum += size
    print(total_sum)


def q7_2():
    fs_tree = dir_parser()
    space_required = 30000000
    disk_total = 70000000
    cur_used = get_dir_size('/', fs_tree)

    additional_needed = space_required - (disk_total - cur_used)
    cur_minimum = disk_total

    for node in fs_tree:
        size = get_dir_size(node, fs_tree)
        if size > additional_needed and size < cur_minimum:
            cur_minimum = size
    print(cur_minimum)




q7_1()
q7_2()



