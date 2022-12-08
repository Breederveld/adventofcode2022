const fs = require("fs");

(async () => {
    const input = fs.readFileSync("C:\\Users\\cas.vangool\\adventofcode\\adventofcode2022\\cas\\Day8\\input.txt").toString();
    const rows = input.split("\r\n");
    const grid = {
        columns: [],
        rows: rows.map(row => [...row])
    };

    for(let a = 0; a < grid.rows.length; a++) {
        for(let b = 0; b < grid.rows[a].length; b++) {
            if(a === 0) {
                grid.columns.push([grid.rows[a][b]]);
            } else {
                grid.columns[b].push(grid.rows[a][b]);
            }
        }
    }
    const outercircle = (grid.columns[0].length * 2 + grid.rows[0].length * 2) - 4;
    let innercircletrees = 0;
    for(let x = 1; x < grid.rows.length - 1; x++) {
        for(let y = 1; y < grid.columns.length - 1; y++) {
            if(isVisible(grid, x , y)) {
                innercircletrees++;
            }
        }
    }
    console.log(outercircle + innercircletrees);
   

})();

function isVisible(grid, x, y) {
    const column = grid.columns[x];
    const row = grid.rows[y];
    const height = +grid.columns[x][y];
    const frontcolumn = [...column].splice(0,y).map(item => +item);
    const backcolumn = [...column].splice(y + 1).map(item => +item);
    const frontrow = [...row].splice(0,x).map(item => +item);
    const backrow = [...row].splice(x + 1).map(item => +item);
    if(
        frontcolumn.filter(item => item >= height).length > 0 &&
        backcolumn.filter(item => item >= height).length > 0 &&
        frontrow.filter(item => item >= height).length > 0 &&
        backrow.filter(item => item >= height).length > 0
        ){
            return false;
        }
    
    return true;
}