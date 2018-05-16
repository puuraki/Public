<?php

$filename = 'logs/messages.json';

if (!$handle = fopen($filename, 'a+')) {
	 echo "Cannot open file ($filename)";
	 exit;
}

fclose($handle);
chmod($filename, 0746);

$content = file_get_contents($filename);

echo $content;