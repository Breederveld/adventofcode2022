const fs = require('fs');
(async() => {
    const input = fs.readFileSync("C:\\Users\\casva\\Documents\\adventofcode2022\\cas\\Day3\\input.txt");
    const lines = input.toString().split("\r\n");
    const backpacks = lines.map(item => {
        return {
            a: [...item.substring(0, item.length / 2)],
            b: [...item.substring(item.length/2)]
        }
    });

    const itemsinboth = [];
    for(let backpack of backpacks) {
        const duplicate = backpack.a.filter(item => backpack.b.includes(item));
        itemsinboth.push( duplicate[0]);
    }

    let result = 0;
    for(let letter of itemsinboth) {
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