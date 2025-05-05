const { exec } = require("child_process");
const path = require("path");
const fs = require("fs");

// Hàm đệ quy tìm các file SCSS
function findScssFiles(dir, files = []) {
    fs.readdirSync(dir).forEach((file) => {
        const fullPath = path.join(dir, file);
        if (fs.statSync(fullPath).isDirectory()) {
            findScssFiles(fullPath, files);
        } else if (file.endsWith(".scss")) {
            files.push(fullPath);
        }
    });
    return files;
}

// Tìm và build SCSS
const projectRoot = path.resolve(".");
const scssFiles = findScssFiles(projectRoot);

scssFiles.forEach((scssFile) => {
    const cssFile = scssFile.replace(/\.scss$/, ".css");
    exec(`sass "${scssFile}" "${cssFile}" --no-source-map --style=compressed`, (err, stdout, stderr) => {
        if (err) {
            console.error(`Error building ${scssFile}: ${stderr}`);
        } else {
            console.log(`Built: ${scssFile} -> ${cssFile}`);
        }
    });
});
