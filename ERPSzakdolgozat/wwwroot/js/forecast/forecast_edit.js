$(document).ready(function () {
	$('.forecastType').on("change", function () {
		var value = $(this).val();

		if (value === "Project") {
			if ($(this).parent().parent().next().find('.leaveName').hasClass('hidden') === false) {
				$(this).parent().parent().next().find('.leaveName').addClass('hidden');
			}
			$(this).parent().parent().next().find('.leaveName').val('');
			$(this).parent().parent().next().find('.projectSelect').removeClass('hidden');
		} else {
			if ($(this).parent().parent().next().find('.projectSelect').hasClass('hidden') === false) {
				$(this).parent().parent().next().find('.projectSelect').addClass('hidden');
			}
			$(this).parent().parent().next().find('.leaveName').val(value);
			$(this).parent().parent().next().find('.leaveName').removeClass('hidden');
		}
	});


	// TODO ajax create-delete


	$('#addNewForecast').click(function () {
		$.ajax({
			url: '../AddResource',
			type: 'POST',
			data: {
				id: $('#Project_Id').val(),
				resName: $('#NewProjectResource_ResourceName').val(),
				resEmp: $('#NewProjectResource_ResourceEmployee').val(),
				resSub: $('#NewProjectResource_ResourceSubcontractor').val(),
				task: $('#NewProjectResource_ResourceTask').val(),
				hDone: $('#NewProjectResource_HoursDone').val(),
				hRem: $('#NewProjectResource_HoursRemaining').val(),
				oDone: $('#NewProjectResource_OvertimeDone').val(),
				oRem: $('#NewProjectResource_OvertimeRemaining').val(),
				cost: $('#NewProjectResource_Cost').val()
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

	$('.deleteResource').click(function () {
		var id = $(this).attr('id');
		$('.deleteResouceModalButton').attr('id', id);
	});

	$('.deleteResouceModalButton').click(function () {
		var id = $(this).attr('id');
		var num = id.substring(2);

		$.ajax({
			url: '../DeleteResource',
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