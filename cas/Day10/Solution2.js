const fs = require("fs");

(async () => {
    const input = fs.readFileSync("C:\\Users\\casva\\Documents\\adventofcode2022\\cas\\Day10\\input.txt").toString();
    const instructions = input.split("\r\n");
    const checkCycles = [20, 60 ,140 ,100,180,220];
    const cycles = []
    for(let i = 0; i < instructions.length; i++) {
        const parts = instructions[i].split(" ");
            cycles.push(0);
        if(parts.length === 2){
            cycles.push(+parts[1]);
        }
    }

    let result = 0;
    let X = 1;
    let CRT = [[],[],[],[],[],[]];
    for(let i = 0; i < 240; i++) {
        const row = Math.floor(i / 40);
        const position = i - (row * 40);
        if(position === X - 1 || position === X || position === X + 1){
            CRT[row].push("#");
        } else {
            CRT[row].push(".");
        }
        
        X += cycles[i];

    }
    console.log(CRT);
    for(const row of CRT) {
        console.log(row.join(''));
    }
})();