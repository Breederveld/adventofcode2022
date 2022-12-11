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
    for(let i = 0; i <= cycles.length; i++) {

        if(checkCycles.includes((i + 1)) ){
            const value = X * (i+1)
            result += value;
        }
        X += cycles[i];

    }
    console.log(result);
})();