///////////////////
//  Tehtävä 3-7  //
///////////////////

var xSquares = 4;
var ySquares = 4;

var firstCard;
var firstBCard;
var secondCard;
var secondBCard;
var firstChosen = false;
var secondChosen = false;

//document.getElementById("playArea").addEventListener("click", function() {changeVisibility(getId());});

function getId()
	{	
		var divOffset = 13;
		var locationX = event.clientX - divOffset;
		var locationY = event.clientY - divOffset;
		var squareLocX;
		var squareLocY;
		var id = "";
		
		if (locationX < xSquares*200 && locationY < ySquares*200)
		{
			console.log("mouse location:", locationX, locationY);
			squareLocX = Math.floor(locationX / 200);
			squareLocY = Math.floor(locationY / 200);
			
			id = "card" + squareLocX + "-" + squareLocY;
			console.log("id: " + id);
		}
		else
		{
			console.log("Out of bounds!");
		}
		
		return id;
	}

function randomizeCards()
{
	var cardList = [];
	var id;
	var isAvailable = false;
	var isAssigned = false;
	var count = 0;
	
	for(var i = 0; i < 16; i++)
	{
		isAssigned = false;
		do
		{
			id = Math.floor((Math.random() * 8) + 1);
			for(var j = 0; j < cardList.length; j++)
			{
				if(cardList[j] === id)
				{
					count++;
				}
			}
			if (count >= 2)
			{
				isAvailable = false;
				count = 0;
			}
			else
			{
				isAvailable = true;
				cardList.push(id);
				isAssigned = true;
				count = 0;
			}
		}
		while(!isAvailable && !isAssigned);
		
		//console.log(cardList[i]);
	}
	
	return cardList;
}

function compareCards(first,second)
{
	if (first === second)
	{
		return true;
	}
	else
	{
		return false;
	}
}


function createCard(posX,posY,id,cards)
{	
	var size = 198;
	var card = document.createElement("div");
	var cardBack = document.createElement("div");
	
	card.id = "Card-" + id;
	card.className = "card";
	document.getElementById("playArea").appendChild(card);
	card.style.width = size + 'px';
	card.style.height = size + 'px';
	card.style.left = posX + 'px';
	card.style.top = posY + 'px';
	card.style.visibility = "visible";
	card.style.backgroundImage = "url('pictures/Mario.png')";
	card.style.backgroundPosition = '0px -440px';
	card.style.zIndex = "1";
	
	cardBack.id = "bCard-" + id;
	cardBack.className = "card";
	document.getElementById("playArea").appendChild(cardBack);
	cardBack.style.width = size + 'px';
	cardBack.style.height = size + 'px';
	cardBack.style.left = posX + 'px';
	cardBack.style.top = posY + 'px';
	//cardBack.style.visibility = "visible";
	cardBack.style.backgroundImage = "url('pictures/" + cards[id] + ".png')";
	cardBack.style.zIndex = "-1";
}

function addEvent()
{
	var parent = document.getElementById("playArea").childNodes;
	
	for(var i = 0; i < parent.length; i++)
	{
		parent[i].addEventListener("click",function () {changeVisibility(this.id);});
	}
}

function createGrid()
{
	var parentDiv = document.getElementById("playArea");
	var id = 0;
	var width = xSquares*220;
	var height = ySquares*220;
	var cards = randomizeCards();
	
	for(var i = 20; i < width; i = i+220)
	{
		
		for(var j = 20; j < height; j = j+220)
		{
			createCard(i,j,id,cards);
			id++;
		}
		
	}
	
	parentDiv.style.width = width+20 + "px";
	parentDiv.style.height = height+20 + "px";
	parentDiv.style.border = "1px solid";
	addEvent();
}

function changeVisibility(id) 
{	
	var card = document.getElementById(id);
	var cardBack = document.getElementById("b" + id).style.backgroundImage;
	var areSame = false;
	
	if(card.style.visibility == "visible")
	{
		if(!firstChosen)
		{
			firstCard = card;
			firstBCard = cardBack;
			firstChosen = true;
			card.style.visibility = "hidden";
		}
		else if(!secondChosen)
		{
			secondCard = card;
			secondBCard = cardBack;
			secondChosen = true;
			card.style.visibility = "hidden";
		}
		
		if(firstChosen && secondChosen)
		{
			areSame = compareCards(firstBCard,secondBCard);
			if(!areSame)
			{
				setTimeout(function(){
						firstCard.style.visibility = "visible";
						secondCard.style.visibility = "visible";
						
						firstChosen = false;
						secondChosen = false;
				}, 500);
			}
			else
			{
				firstChosen = false;
				secondChosen = false;
			}
		}
	}
	else
	{
		card.style.visibility = "visible";
	}
	
}