const fs = require("fs");

(async () => {
    const input = fs.readFileSync("C:\\Users\\casva\\Documents\\adventofcode2022\\cas\\Day9\\input.txt").toString();
    const steps = input.split("\r\n");
    const usedPositions = [{x:0,y:0}];
   
    let knots = []
    for(let i = 0; i < 10;i++){
        knots.push({x:0,y:0});
    }
    
    for(const step of steps) {
        const direction = step.split(" ")[0];
        for(let i = 0; i < +step.split(" ")[1]; i++) {
            for(let knotIndex in knots) {
                knotIndex = +knotIndex;
                if(knotIndex === knots.length - 1){
                    continue;
                }
                const head = knots[knotIndex]
                const tail = knots[knotIndex + 1];
                if(knotIndex === 0) {
                    MoveHead(direction, head);
                }
                const touching = CheckTouching(head,tail);
                if(!touching) {
                    MoveTail(head, tail);
                    if(knotIndex === knots.length - 2) {
                        registerPosition(usedPositions, tail);
                    }
                }
            }   
        }
    }
    console.log(usedPositions.length);
    
})();

function registerPosition(positions, tail) {
    if(positions.filter(item => item.x === tail.x && item.y === tail.y).length === 0) {
        positions.push({...tail});
    }
}

function CheckTouching(head, tail) {
    return Math.abs(head.x - tail.x) <= 1 && Math.abs(head.y - tail.y) <= 1;
}

function MoveTail(head, tail) {
    if(head.x === tail.x) {
        if(head.y > tail.y) {
            tail.y++;
        } else {
            tail.y--;
        }
    } else if (head.y === tail.y){
        if(head.x > tail.x) {
            tail.x++;
        } else {
            tail.x--;
        }
    } else {
        if(head.x > tail.x && head.y > tail.y) {
            tail.x++;
            tail.y++;
        } else if (head.x < tail.x && head.y < tail.y) {
            tail.x--;
            tail.y--;
        } else if (head.x > tail.x && head.y < tail.y) {
            tail.x++;
            tail.y--;
        } else {
            tail.x--;
            tail.y++;
        }
    }
}

function MoveHead(direction, head) {
   switch(direction) {
    case "R": {
        head.x++;
        break;
    }
    case "L": {
        head.x--;
        break;
    }
    case "U": {
        head.y++;
        break;
    }
    case "D": {
        head.y--;
        break;
    }
   }
}

