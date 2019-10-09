<?php

	$bd=mysqli_connect('db5000192818.hosting-data.io','dbu397590','Uba2_Wy2V8@DB16','dbs187630');
	$query = 'SELECT * FROM Party';
	$result = mysqli_query($bd, $query);

	$jsonmessage =  $jsonmessage.'{"Deads":[';
	while ($row = mysqli_fetch_assoc($result)) {
		if(substr($jsonmessage, -1) == '}') $jsonmessage = $jsonmessage.',';
		$jsonmessage =  $jsonmessage.'{"dead":"'.$row["dead"].'","position":"'.$row["position"].'","text":"'.(str_replace(array("\r\n", "\r", "\n"), "#", $row["text"])).'"}';

	}
	$jsonmessage =  $jsonmessage.']}';
	printf($jsonmessage);
?>