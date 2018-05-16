<?php

$name= isset($_POST['name']) ? $_POST['name'] : ''; 
$message= isset($_POST['message']) ? $_POST['message'] : ''; 

if($name != "" && $message != ""){
	$time=date("H:i");
	$name = strip_tags(html_entity_decode($name));
	$message = strip_tags(html_entity_decode($message));
	write($time,$name,$message);
}

function write($time,$name,$message){
    $filename = 'logs/messages.json';
	$jsonData = file_get_contents($filename);
	$jsonArray = json_decode($jsonData);
	echo $jsonArray;
	$newData = array (
		'time' => $time,
		'name' => $name,
		'message' => $message,
	);
    $jsonArray[] = $newData;
	
	$jsonData = json_encode($jsonArray);
	
    if (is_writable($filename)) {

       if (!$handle = fopen($filename, 'w')) {
            echo "Cannot open file ($filename)";
            exit;
       }

       if (fwrite($handle, $jsonData) === FALSE) {
           echo "Cannot write to file ($filename)";
           exit;
       }

       fclose($handle);

       } 
	   else {
           echo "The file $filename is not writable";
    }   
}