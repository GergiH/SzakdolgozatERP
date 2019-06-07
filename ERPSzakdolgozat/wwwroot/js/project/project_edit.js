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

	$('#CloseNewResource').click(function () {
		$('#NewProjectResource_ResourceName').val('');
		$('#NewProjectResource_ResourceEmployee').val('');
		$('#NewProjectResource_ResourceSubcontractor').val('');
		$('#NewProjectResource_ResourceTask').val('');
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

	$('.calcFin').on('change', function () {
		var frm = $('#editForm').serialize();

		$.ajax({
			url: '../CalculateFinancials',
			type: 'POST',
			data: "kiskutya=" + frm,
			dataType: "json",
			success: function (data) {
				for (var i = 0; i < data.project.resources.length; i++) {
					$('#Project_Resources_' + i + '__Revenue').val(data.project.resources[i].revenue);
				}

				$('#Project_HoursDone').val(data.project.hoursDone);
				$('#Project_HoursRemaining').val(data.project.hoursRemaining);
				$('#Project_HoursAll').val(data.project.hoursAll);
				$('#Project_OvertimeDone').val(data.project.overtimeDone);
				$('#Project_OvertimeRemaining').val(data.project.overtimeRemaining);
				$('#Project_OvertimeAll').val(data.project.overtimeAll);
				$('#Project_ResourcesCostSpent').val(data.project.resourcesCostSpent);
				$('#Project_ResourcesCostRemaining').val(data.project.resourcesCostRemaining);
				$('#Project_ResourcesCost').val(data.project.resourcesCost);
				$('#Project_ResourcesRevenueGained').val(data.project.resourcesRevenueGained);
				$('#Project_RiskCostSpent').val(data.project.riskCostSpent);
				$('#Project_RiskCostRemaining').val(data.project.riskCostRemaining);
				$('#Project_RiskCost').val(data.project.riskCost);
				$('#Project_RiskRevenue').val(data.project.riskRevenue);
				$('#Project_TotalCostSpent').val(data.project.totalCostSpent);
				$('#Project_TotalCostRemaining').val(data.project.totalCostRemaining);
				$('#Project_TotalCost').val(data.project.totalCost);
				$('#Project_TotalRevenue').val(data.project.totalRevenue);
			},
			error: function (XMLHttpRequest, textStatus, errorThrown) {
				// console.log(XMLHttpRequest, textStatus, errorThrown);
			}
		});
	});

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