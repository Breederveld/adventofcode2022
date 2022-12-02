with open("input_2_1") as file:
    data = file.read().splitlines()

# , draw
# Col 1 opponent (Rock, paper, scissors | ABC)
# Col2 my choice XYZ
# Score: my choice rock1, paper2, scissors3 + 0 lost, 3 draw, 6 win


def rockPaperScissors(opp_shape, my_shape):
    if opp_shape == my_shape:
        return 'draw'
    else:
        if opp_shape == 'rock':
            if my_shape == 'scissors':
                return 'loss'
            elif my_shape == 'paper':
                return 'won'
        elif opp_shape == 'paper':
            if my_shape == 'rock':
                return 'loss'
            elif my_shape == 'scissors':
                return 'won'
        elif opp_shape == 'scissors':
            if my_shape == 'paper':
                return 'loss'
            elif my_shape == 'rock':
                return 'won'


def shapeForOutcome(opp_shape, desired_outcome):
    if desired_outcome == 'draw':
        return opp_shape
    else:
        if desired_outcome == 'win':
            if opp_shape == 'rock':
                return 'paper'
            elif opp_shape == 'paper':
                return 'scissors'
            elif opp_shape == 'scissors':
                return 'rock'
        elif desired_outcome == 'loss':
            if opp_shape == 'rock':
                return 'scissors'
            elif opp_shape == 'paper':
                return 'rock'
            elif opp_shape == 'scissors':
                return 'paper'


def q2_1():
    opp_map = {'A': 'rock', 'B': 'paper', 'C': 'scissors'}
    my_map = {'X': 'rock', 'Y': 'paper', 'Z': 'scissors'}

    points_map_shape = {'rock': 1, 'paper': 2, 'scissors': 3}
    points_map_outcome = {'loss': 0, 'draw': 3, 'won': 6}

    my_score = 0

    for round in data:
        (opp_shape_enc, my_shape_enc) = round.split(" ")
        opp_shape = opp_map[opp_shape_enc]
        my_shape = my_map[my_shape_enc]

        outcome = rockPaperScissors(opp_shape, my_shape)

        my_score += points_map_shape[my_shape] + points_map_outcome[outcome]

    print(my_score)

def q2_2():
# Second col defines desired outcome for me:
# X=loss, Y=draw, Z=win

    opp_map = {'A': 'rock', 'B': 'paper', 'C': 'scissors'}
    desired_outcome_map = {'X': 'loss', 'Y': 'draw', 'Z': 'win'}

    points_map_shape = {'rock': 1, 'paper': 2, 'scissors': 3}
    points_map_outcome = {'loss': 0, 'draw': 3, 'win': 6}

    my_score = 0

    for round in data:
        (opp_shape_enc, desired_outcome_enc) = round.split(" ")
        opp_shape = opp_map[opp_shape_enc]
        desired_outcome = desired_outcome_map[desired_outcome_enc]

        my_shape = shapeForOutcome(opp_shape, desired_outcome)

        my_score += points_map_shape[my_shape] + points_map_outcome[desired_outcome]

    print(my_score)

# q2_1()
q2_2()