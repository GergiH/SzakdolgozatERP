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

	// resource changes
	$('.resType').on("change", function () {
		var value = $(this).val();

		switch (value) {
			case "Employee":
				if ($(this).parent().parent().next().find('.resNameInput').hasClass('hidden') === false) {
					$(this).parent().parent().next().find('.resNameInput').addClass('hidden');
				}
				if ($(this).parent().parent().next().find('.resSubSelect').hasClass('hidden') === false) {
					$(this).parent().parent().next().find('.resSubSelect').addClass('hidden');
				}

				$(this).parent().parent().next().find('.resNameInput').val('');
				$(this).parent().parent().next().find('.resSubSelect').val('');
				$(this).parent().parent().next().find('.resEmpSelect').removeClass('hidden');
				$(this).parent().parent().next().addClass('has-warning');
				break;
			case "Subcontractor":
				if ($(this).parent().parent().next().find('.resNameInput').hasClass('hidden') === false) {
					$(this).parent().parent().next().find('.resNameInput').addClass('hidden');
				}
				if ($(this).parent().parent().next().find('.resEmpSelect').hasClass('hidden') === false) {
					$(this).parent().parent().next().find('.resEmpSelect').addClass('hidden');
				}

				$(this).parent().parent().next().find('.resNameInput').val('');
				$(this).parent().parent().next().find('.resEmpSelect').val('');
				$(this).parent().parent().next().find('.resSubSelect').removeClass('hidden');
				$(this).parent().parent().next().addClass('has-warning');
				break;
			case "Other":
				if ($(this).parent().parent().next().find('.resEmpSelect').hasClass('hidden') === false) {
					$(this).parent().parent().next().find('.resEmpSelect').addClass('hidden');
				}
				if ($(this).parent().parent().next().find('.resSubSelect').hasClass('hidden') === false) {
					$(this).parent().parent().next().find('.resSubSelect').addClass('hidden');
				}

				$(this).parent().parent().next().find('.resEmpSelect').val('');
				$(this).parent().parent().next().find('.resSubSelect').val('');
				$(this).parent().parent().next().find('.resNameInput').removeClass('hidden');
				$(this).parent().parent().next().addClass('has-warning');
				break;
			default:
				break;
		}
	});

	$('.resEmpSelect, .resSubSelect').on('change', function () {
		var name = $('option:selected', this).text();
		$(this).parent().find('.resNameInput').val(name);
	});

	$('#SaveNewResource').click(function () {
		$.ajax({
			url: '../AddResource',
			type: 'POST',
			data: {
				id: $('#Project_Id').val(),
				resName: $('#NewProjectResource_ResourceName').val(),
				resEmp: $('#NewProjectResource_ResourceEmployee').val(),
				resSub: $('#NewProjectResource_ResourceSubcontractor').val(),
				task: $('#NewProjectResource_Task').val(),
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

	$('.calcFin').on('change', function () {
		var frm = $('#editForm').serialize();

		$.ajax({
			url: '../CalculateFinancials',
			type: 'POST',
			data: {
				peVM: frm
			},
			dataType: "json",
			success: function (data) {
				console.log(data);
				// TODO write everything back
			},
			error: function (XMLHttpRequest, textStatus, errorThrown) {
				// console.log(XMLHttpRequest, textStatus, errorThrown);
			}
		});
	});
});