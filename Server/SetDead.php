<?php
	$_GET["name"] $_GET["dead"] $_GET["position"]
	$bd=new PDO('mysql:host=db5000192818.hosting-data.io;dbname=dbs187630','dbu397590','Uba2_Wy2V8@DB16', array(PDO::MYSQL_ATTR_INIT_COMMAND => 'SET NAMES utf8'));
	$requete = $bd->prepare('SELECT * FROM Party');	
	$requete->execute() ;

?>