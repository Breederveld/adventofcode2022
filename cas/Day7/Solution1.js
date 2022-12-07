const fs = require("fs");

(async () => {
    const input = fs.readFileSync("C:\\Users\\cas.vangool\\adventofcode\\adventofcode2022\\cas\\Day7\\input.txt").toString();
    const lines = input.split("\r\n");
    let directories = [];
    let activedirectory = undefined;
    for(const line of lines){
        const command = line.split(" ");
        if(command[0] === "$") {
            if(command[1] === "cd") {
                if(command[2] !== "..") {
                    const directorie = {
                        name: command[2],
                        parent: activedirectory,
                        children: [],
                        files: [],
                    };
                    if(activedirectory) {
                        activedirectory.children.push(directorie);
                    }
                    directories.push(directorie);
                    activedirectory = directorie;
                } else {
                    activedirectory = activedirectory.parent;
                }
            }
        } else {
            if(command[0] === "dir"){
                continue;
            } else {
                activedirectory["files"].push({name: command[1], size: command[0]});
            }
        }
    }
    let totalsize = 0;
    for(let directory of directories) {
        const size = checkDirectorySize(directory);
        if(size < 100000) {
            console.log(directory.name);
            totalsize += size;
        }
    }
    console.log(totalsize);
    

})();

function checkDirectorySize(directory) {
    let size = 0;
    for(let file of directory.files) {
        size += +file.size;
    }

    for(let child of directory.children) {
        size += checkDirectorySize(child);
    }
    return size;
}