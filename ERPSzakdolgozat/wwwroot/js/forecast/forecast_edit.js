$(document).ready(function () {
	$('.forecastType').on("change", function () {
		var value = $('option:selected', this).text();

		if (value === "Project") {
			if ($(this).parent().next().parent().find('.leaveName').hasClass('hidden') === false) {
				$(this).parent().next().parent().find('.leaveName').addClass('hidden');
			}

			$(this).parent().next().next().find('.leaveNameInput').val('');
			$(this).parent().next().parent().find('.projectSelect').removeClass('hidden');
		} else {
			if ($(this).parent().next().parent().find('.projectSelect').hasClass('hidden') === false) {
				$(this).parent().next().parent().find('.projectSelect').addClass('hidden');
			}

			$(this).parent().next().next().find('.leaveNameInput').val(value);
			$(this).parent().next().parent().find('.leaveName').removeClass('hidden');
		}
	});

	$('#addNewForecast').click(function () {
		$.ajax({
			url: '../AddForecast',
			type: 'POST',
			data: {
				fcType: $('#fc_ForecastType').val(),
				fcProjectID: $('#fc_ProjectID').val(),
				fcForecastWeekID: $('#fc_ForecastWeekId').val(),
				fcEmployeeID: $('#fc_EmployeeId').val(),
				fcHours: $('#fc_Hours').val(),
				fcComment: $('#fc_Comment').val()
			},
			dataType: "json",
			success: function () {
				window.location.reload();
			},
			error: function (XMLHttpRequest, textStatus, errorThrown) {
				// console.log(XMLHttpRequest, textStatus, errorThrown);
			}
		});
	});

	$('.deleteForecast').click(function () {
		var id = $(this).attr('id');
		$('.deleteForecastModalButton').attr('id', id);
	});

	$('.deleteForecastModalButton').click(function () {
		var id = $(this).attr('id');
		var num = id.substring(2);

		$.ajax({
			url: '../DeleteForecast',
			type: 'POST',
			data: {
				id: num
			},
			dataType: "json",
			success: function () {
				window.location.reload();
			},
			error: function (XMLHttpRequest, textStatus, errorThrown) {
				// console.log(XMLHttpRequest, textStatus, errorThrown);
			}
		});
	});


	// TODO hours css


	// risk checkboxes
	$('.riskCheck').on('change', function () {
		//var id = $(this).attr('id');
		//var num = id.substring(14, id.lastIndexOf('__'));
		//var value = parseInt($('#riskCheckNum' + num).text());
		var riskValue = 0;
		var riskCSS = "text-warning";

		var risksLength = $('#riskList li').length - 1;
		for (var i = 0; i < risksLength; i++) {
			if ($('#rc' + i).is(':checked')) {
				riskValue += parseInt($('#riskCheckNum' + i).text());
			}
		}

		// set the color of the number
		var ratio = parseInt(riskValue) / parseInt(totalRiskValue);
		switch (true) {
			case (ratio >= 0.8):
				riskCSS = "text-danger";
				break;
			case (ratio <= 0.5):
				riskCSS = "text-success";
				break;
			default:
				break;
		}
		$('#riskValue').text(riskValue);
		$('#riskValue').removeClass('text-success');
		$('#riskValue').removeClass('text-warning');
		$('#riskValue').removeClass('text-danger');
		$('#riskValue').addClass(riskCSS);
	});
});