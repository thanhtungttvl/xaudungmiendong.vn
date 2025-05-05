# build-scss.ps1
$files = Get-ChildItem -Path . -Recurse -Include *.scss
foreach ($file in $files) {
    $outFile = $file.FullName -replace '\.scss$', '.css'
    sass --no-source-map --style=expanded $file.FullName $outFile
}
