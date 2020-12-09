param([string]$spotfire = "", [string]$project = "")

$runtimeFolder = [System.IO.Path]::GetDirectoryName($spotfire);
$configName = "$spotfire.config"
$configNameBackup = "$spotfire.config.tmpl"
$projectFolder = ([String]$project).TrimEnd('\\').TrimEnd('"')

function OpenFileExc([string]$filename) {
    if (Test-Path $filename) {
        $file = gi (Resolve-Path $filename) -Force
        if ($file -is [IO.FileInfo]) {
            try {
                $stream = $file.Open('Open', 'ReadWrite', 'None')
                return $stream, 'open'
            }
            catch {
                return $null, 'open'
            }
        }
    }
    else {
        try {
            $stream = New-Object System.IO.FileStream($filename, 'CreateNew', 'ReadWrite', 'None')
            return $stream, 'create'
        }
        catch {
            return $null, 'create'
        }
    }
    return $null, ''
}

function WaitForFile ([string]$filename) {
    while ($true) {
        $file, $action = OpenFileExc($filename)
        if ($file -eq $null) {
            $tries += 1
            if ($tries -gt 10) { 
                return $null, 'timeout'
            }

            Start-Sleep -Seconds (Get-Random -Minimum 0.5 -Maximum 1)
            continue
        }
        else {
            return $file, $action
        }

    }
    return $null, 'impossible'
}

function Get-Modules-Xml($stream) {
    $doc = New-Object system.Xml.XmlDocument
    $doc.Load($stream)
    return $doc
}

function Has-Module($doc) {
    $root = $doc.DocumentElement
    $modules = $root.SelectSingleNode("Spotfire.Dxp/modules")
    foreach ($f in $modules.SelectNodes("folder")) 
    {
        $folder = [System.Xml.XmlElement]$f
        if ($folder.GetAttribute("path") -eq $projectFolder)
        {
            return $true
        }
    }
    return $false
}

function Add-Module($doc, $path) {
    $root = $doc.DocumentElement
    $modules = $root.SelectSingleNode("Spotfire.Dxp/modules")
    $newFolder = $doc.CreateElement("folder")
    $path = $doc.CreateAttribute("path")
    $path.Value = $projectFolder
    $addedPath = $newFolder.Attributes.Append($path)
    $addedFolder = $modules.AppendChild($newFolder)
    return $doc
}

function Write-Module-Xml($stream, $doc) {
    $stream.Seek(0, [System.IO.SeekOrigin]::Begin)
    $doc.Save($stream)
}

echo "Configuring $configName to use $projectFolder"

$stream, $action = WaitForFile $configName
if ($stream -eq $null) {
    Write-Output "Maximum number of retries. Aborting."
    exit 1
}

if ($action -eq 'create') {
    [string]$configTmpl = Get-Content $configNameBackup -Encoding UTF8
    $tw = New-Object System.IO.StreamWriter($stream)
    $tw.Write($configTmpl)
    $tw.Flush()
    $pos = $stream.Seek(0, 'Begin')  # do not close
}

$doc = Get-Modules-Xml $stream
if (Has-Module $doc) {
    echo "Already in config file: $projectFolder"
    echo "Nothing to do"
    exit 0
}

Add-Module $doc $projectFolder
Write-Module-Xml $stream $doc

$stream.Close()
$stream.Dispose()

exit 0

