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
    let bestSceneScore = 0;
    for(let x = 0; x < grid.rows.length; x++) {
        for(let y = 0; y < grid.columns.length; y++) {
            const score = scenicScore(grid, x ,y);
            if(bestSceneScore < score) {
                bestSceneScore = score;
            }
        }
    }
    console.log(bestSceneScore);
   

})();

function scenicScore(grid, x, y) {
    const column = grid.columns[x];
    const row = grid.rows[y];
    const height = +grid.columns[x][y];
    const frontcolumn = [...column].splice(0,y).map(item => +item).reverse();
    const backcolumn = [...column].splice(y + 1).map(item => +item);
    const frontrow = [...row].splice(0,x).map(item => +item).reverse();
    const backrow = [...row].splice(x + 1).map(item => +item);
    let topscore = 0,leftscore = 0,rightscore = 0,bottomscore = 0;
    for(let index in frontcolumn){
        if(frontcolumn[index] < height) {
            topscore++;
        } else {
            topscore++;
            break;
        }
    }
    for(let index in backcolumn){
        if(backcolumn[index] < height) {
            bottomscore++;
        } else {
            bottomscore++;
            break;
        }
    }

    for(let index in frontrow){
        if(frontrow[index] < height) {
            leftscore++;
        } else {
            leftscore++;
            break;
        }
    }

    for(let index in backrow){
        if(backrow[index] < height) {
            rightscore++;
        } else {
            rightscore++;
            break;
        }
    }



    return leftscore * rightscore * bottomscore * topscore;
}