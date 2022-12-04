const fs = require('fs');
(async () => {
    const input = fs.readFileSync("C:\\Users\\casva\\Documents\\adventofcode2022\\cas\\Day3\\input.txt");
    const backpacks = input.toString().split("\r\n");
    const groups = [];
    for(let i = 0; i < backpacks.length; i += 3){

        groups.push([[...backpacks[i]], [...backpacks[i + 1]], [...backpacks[i + 2]]]);
    }
    const letters = [];
    for(let group of groups) {
        const duplicates1 = group[0].filter(item => group[1].includes(item));
        const duplicates2 = group[0].filter(item => group[2].includes(item));
        const duplicates3 = duplicates1.filter(item => duplicates2.includes(item));
        letters.push(duplicates3[0]);
    }
    let result = 0;
    for(let letter of letters) {
        console.log(letter);
        if(letter === letter.toUpperCase()) {
            result += letter.charCodeAt(0) - 38;
        } else {
            result += letter.charCodeAt(0) - 96;
        }
        console.log(result);
    }
    console.log(result);
})();