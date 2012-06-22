Param($version = "0.0.0")

# Set environment variables for Visual Studio Command Prompt
pushd 'C:\Program Files\Microsoft Visual Studio 10.0\VC'
cmd /c “vcvarsall.bat&set” |
foreach {
  if ($_ -match “=”) {
    $v = $_.split(“=”); set-item -force -path "ENV:\$($v[0])" -value "$($v[1])"
  }
}
popd

# Find NuGet.exe
if (Test-Path '.nuget\nuget.exe') {
  $nuget = Get-ChildItem '.nuget' -Filter 'nuget.exe'
}
else {
  $nuget = Get-ChildItem '..\Util' -Filter 'nuget.exe'
}

# Get our current branch
$branch = hg branch

# Write out version info for dll
"using System.Reflection;`n[assembly: AssemblyVersion(`"$version`")]" > 'AssemblyVersion.cs'

# Build solution
$solution = Get-ChildItem '.' -Filter '*.sln'
devenv $solution /rebuild Release | Write-Output

# Create binaries directory if necessary
if (!(Test-Path '..\Binaries')) {
  New-Item '..\Binaries' -type directory | Out-Null
}

# Modify NuGet version to a dev version if we're not on the Main branch.
if ($branch -ne 'Main') {
  $version = $version + '-dev-' + (Get-Date).ToString("ddHHmmss")
}

# Build NuGet package
$nuspec = Get-ChildItem '.' -Filter '*.nuspec'
&$nuget.FullName pack -Symbols $nuspec -Version $version -OutputDirectory ..\Binaries

"Built " + $nuspec.BaseName + " version $version"