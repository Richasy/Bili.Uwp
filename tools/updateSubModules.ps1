<#
.Synopsis
    Update the git submodule repos with their latest version.

.Description
    submodule repos:
        documentation
        prototypes
    To target the repos latest version, we need to update the targeted version of the submodules.
    We will retrieve the latest version of the remote master branch, get it, and point to it through a detached commit.
      - If one submodule is checked out on a specific branch **without** pending modifications, after script execution, you will be pointing to the remote master head (through a detached commit), as expected.
      - If one submodule is checked out on a specific branch **with** pending modifications, script execution will fail with an error message if your pending modifications are conflicting with master head.
    In any case, the script won't change your local branch content/state.

.Link
    https://git-scm.com/book/en/v2/Git-Tools-Submodules

.Parameter help
    Displays this message.

#>

[CmdletBinding()]
Param(
   [alias("h")]
   [switch]$help = $False
)


if($help)
{
    Get-Help $PSScriptRoot\updateSubModules
    return
}

Write-Host -Background green -Foreground black -NoNewline ">> "
Write-Output "Submodule summary before update:"
git submodule summary  -n 2

Write-Host -Background green -Foreground black -NoNewline ">> "
Write-Output "Start update..."
git submodule update --init --remote 
if ($LASTEXITCODE -eq 0)
{
    Write-Host -Background green -Foreground black ">> Update succeeded"
    Write-Host -Background green -Foreground black -NoNewline ">> "
    Write-Host 'Documentation repo is now updated to its remote master branch head content as a detached commit. (https://git-scm.com/docs/git-submodule/ - update section)';
    Write-Host 'Prototypes repo is now updated to its remote master branch head content as a detached commit. (https://git-scm.com/docs/git-submodule/ - update section)';
    Write-Host -Background yellow -Foreground black "WARNING: " -NoNewLine
    Write-Output "We didn't update your local master branch. Proceed as you like to update it whenever you want."
}
else
{
   Write-Host -Background red -Foreground black 'Update FAILED.'
}
Write-Host -Background green -Foreground black -NoNewline ">> "
Write-Output "Submodule summary after update:"
git submodule summary  -n 2

