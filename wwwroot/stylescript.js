var timeLeft = 0;
window.setInterval(
	function () {
		timeLeft += 1;
		document.getElementById("timeLeft").innerHTML = "00" + timeLeft;

	}, 1000);