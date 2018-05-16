/////////////////////
//  Kuva-arvoitus  //
/////////////////////

var xSquares = 16;
var ySquares = 16;

document.getElementById("playArea").addEventListener("click", function(event) {changeVisibility(getId(event));});

function getId(event)
	{	
		var divOffset = 13;
		var locationX = event.clientX - divOffset;
		var locationY = event.clientY - divOffset;
		var squareLocX;
		var squareLocY;
		var id = "";
		
		if (locationX < xSquares*50 && locationY < ySquares*50)
		{
			console.log("mouse location:", locationX, locationY);
			squareLocX = Math.floor(locationX / 50);
			squareLocY = Math.floor(locationY / 50);
			
			id = "square" + squareLocX + "-" + squareLocY;
			console.log("id: " + id);
		}
		else
		{
			console.log("Out of bounds!");
		}
		
		return id;
	}

function createSquare(posX,posY,id)
{	
	var size = 48;
	var square = document.createElement("div");
	
	square.id = "square" + id;
	square.className = "square";
	document.getElementById("playArea").appendChild(square);
	square.style.width = size + 'px';
	square.style.height = size + 'px';
	square.style.left = posX + 'px';
	square.style.top = posY + 'px';
	square.style.visibility = "visible";
	square.style.backgroundImage = "url('pictures/Mario.png')";
	square.style.backgroundPosition = -posX + 'px ' + -posY + 'px';
}

function addEvent()
{
	var parent = document.getElementById("playArea").childNodes;
	
	for(var i = 0; i < parent.length; i++)
	{
		parent[i].addEventListener("mouseover",function () {changeVisibility(this.id);});
	}
}

function createGrid()
{
	var parentDiv = document.getElementById("playArea");
	var idX = 0;
	var idY = 0;
	var id;
	var width = xSquares*50;
	var height = ySquares*50;
	
	for(var i = 0; i < width; i = i+50)
	{
		idY = 0;
		
		for(var j = 0; j < height; j = j+50)
		{
			id = idX + "-" + idY;
			createSquare(i,j,id);
			idY++;
		}
		
		idX++;
	}
	
	parentDiv.style.width = width + "px";
	parentDiv.style.height = height + "px";
	parentDiv.style.border = "1px solid";
	parentDiv.style.backgroundImage = "url('pictures/SuperMarioWorldMain800.png')";
	//addEvent();
}

function changeVisibility(id) 
{	
	var square = document.getElementById(id);
	
	if(square.style.visibility == "visible")
	{
		square.style.visibility = "hidden";
	}
	else
	{
		square.style.visibility = "visible";
	}
	
}