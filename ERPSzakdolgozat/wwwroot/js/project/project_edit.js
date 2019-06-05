$(document).ready(function () {
	$("#logUser").on("change", function () {
		var value = $("#logUser option:selected").text().toLowerCase();

		$("#logList div").filter(function () {
			$(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
		});
	});

	$("#logSearch").on("keyup", function () {
		var value = $(this).val().toLowerCase();

		$("#logList div").filter(function () {
			$(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
		});
	});
});