const fs = require('fs');

(async () => {
    const input = fs.readFileSync("C:\\Users\\casva\\Documents\\adventofcode2022\\cas\\Day4\\input.txt");
    const pairs = input.toString().split("\r\n");
    let overlaps = 0;
    for(const pair of pairs) {
        const assignments = pair.split(',').map(item => {
            const i = item.split('-');
            return arrayOfRange(+i[0], +i[1]);
        });
        if(assignments[0].filter(i => assignments[1].includes(i)).length > 0) {
            overlaps++;
        }
    }
    console.log(overlaps);
})();

function arrayOfRange(lowest, highest) {
    const array = [];
    for(let i = lowest; i <= highest; i++){
        array.push(i);
    }
    return array;
}