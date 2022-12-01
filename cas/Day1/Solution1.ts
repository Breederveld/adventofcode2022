import fs from 'fs';
(async() => {
    const data = fs.readFileSync('input1.txt');
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
    console.log(results);
    console.log(Math.max(...results));
})();