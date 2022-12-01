const fs = require('fs');
(async() => {
    const data = fs.readFileSync('C:\\Users\\casva\\Documents\\adventofcode2022\\cas\\Day1\\input1.txt');
    console.log(data.toString());
    const rows = data.toString().split("\r\n");
    console.log(rows);
    const results = [];
    let activeelf = 0;
    for(let row of rows) {
        if(row === ""){
            results.push(activeelf);
            activeelf = 0;
        }
        activeelf += +row;
    }
    const sorted = results.sort();
    const total = sorted[sorted.length -1] + sorted[sorted.length -2] + sorted[sorted.length -3];
    console.log(total);
})();