var refresh = setInterval(ajaxRequest,1000);

function ajaxRequest()
{
	$.ajax({
		type: 'post',
		url: 'shoutbox_read.php',
		success: function(data) {
			var json_obj = $.parseJSON(data);
			var output = "";
			
			for(var i in json_obj)
			{
				output+="<li><span class='message_time'>" + json_obj[i].time + "</span> <span class='shoutbox_username'>" + json_obj[i].name + "</span>: <span class='msg_body'>" + json_obj[i].message + "</span></li>";
			}
			$('#message_log').html(output);
		},
		error: function(data) 
		{
			console.log("something went wrong!");
		}
	});
	var elem = $('#message_container');
	elem.scrollTop = elem.scrollHeight;
}

$(function() {
	$('form').on('submit', function (e){
		e.preventDefault();
		
		$.ajax({
			type: 'post',
			url: 'shoutbox_write.php',
			data: $('form').serialize(),
			success: function(){
				var commentElement = $('#message');
				commentElement.val('');
			}
		});
	});
});