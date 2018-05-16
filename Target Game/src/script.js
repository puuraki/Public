///////////////////
//  Tehtävä 3-4  //
///////////////////

var interval = 1000;
var myVar = setInterval(myTimer, interval);
var position_x = 619;
var position_y = 500;
var size = 75;
var targetDestroyed = true;
var isOn = true;
var score = 0;
var lastScore = 0;
var scored = false;

function startTimer()
{
	var intervalTxt = document.getElementById("interval");
	if(!isOn)
	{
		myVar = setInterval(myTimer, interval);
		isOn = !isOn;
	}
	
	if(interval > 25)
	{
		interval = interval - 25;
		intervalTxt.innerHTML = "Interval: " + interval/1000 + " s";
	}
}

function myTimer()
{
	if(targetDestroyed)
	{
		makeTarget();
	}
	else
	{
		destroyTarget();
		if (scored && lastScore != score)
		{
			clearInterval(myVar);
			isOn = false;
			lastScore = score;
			scored = false;
			startTimer();
		}
	}
}

function addScore()
{
	if(!scored)
	{
		score++;
		scored = true;
	}
	document.getElementById("score").innerHTML = "Score: " + score;
}

function makeTarget()
{
	randomize();
	var target = document.createElement("div");
	target.id = "target";
	document.getElementById("playArea").appendChild(target);
	target.style.width = size + 'px';
	target.style.height = size + 'px';
	target.style.left = position_x + 'px';
	target.style.top = position_y + 'px';
	targetDestroyed = false;
	
	document.getElementById("target").addEventListener("mouseover", addScore);
}

function destroyTarget()
{
	var target = document.getElementById("target");
	target.parentNode.removeChild(target);
	targetDestroyed = true;
}

function randomize()
{
	size = Math.floor((Math.random() * 75) + 1)+25;
	position_x = Math.floor((Math.random() * (700 - size - 6)) + 1);
	position_y = Math.floor((Math.random() * (700 - size - 6)) + 1);
}