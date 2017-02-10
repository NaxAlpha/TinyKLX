param(
	[String]$config
)
$f = Get-Content -Path .\settings.ini
$outFile= 'Config.vb'
echo "Public Class Config" > $outFile
foreach($x in $f){
    $a = $x.Split('=');
    
    $k = $a[0].TrimStart().TrimEnd();

    $v = $a[1].TrimStart().TrimEnd();
    $vs = $v.Split('|');

    $vd = $vs[0].TrimStart().TrimEnd();
    $vr = $vs[1].TrimStart().TrimEnd();

    $vx = $vd;

    if($config -eq 'Release') {
        $vx = $vr;
        echo 'Release'
    }

    echo "Public ReadOnly Property $k As Object = $vd" >> $outFile
}

echo "End Class" >> $outFile