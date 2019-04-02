$(document).ready(function () {
	if ($('#isDetails').val() === "true") {
		// TODO add hiddens to view if disabled values are needed
		$('input, option').prop('readonly', true);
		$(':checkbox').prop('disabled', true);
		$('select').prop('disabled', true);

		if ($('#SameAddress').is(':checked')) {
			$('.mailAddress').prop('readonly', true);
		}
	}
});

$('#SameAddress').change(function () {
	if ($(this).is(':checked')) {
		copyAddress();
		$('.mailAddress').prop('readonly', true);
	}
	else {
		$('.mailAddress').prop('readonly', false);
	}
});

$('#SaveNewFinancial').click(function () {
	$.ajax({
		url: '../AddFinancial',
		type: 'POST',
		data: {
			currencyId: $('#nF_Currency').val(),
			workHours: $('#nF_WorkHours').val(),
			grossSalary: $('#nF_GrossSalary').val(),
			cafeteria: $('#nF_Cafeteria').val(),
			bonus: $('#nF_Bonus').val(),
			employeeId: $('#EmployeeId').val()
		},
		dataType: "json",
		success: function (data) {
			if (data === 1) {
				$('#NewFinancialAlert').removeClass('alert-success');
				$('#NewFinancialAlert').addClass('alert-danger');
				$('#NewFinancialAlert').text('errorvanbaszki');
			} else {
				$('#NewFinancialAlert').removeClass('alert-danger');
				$('#NewFinancialAlert').addClass('alert-success');
				$('#NewFinancialAlert').text('nincserrorjee');
			}

			$('#NewFinancialAlert').show();

			window.location.reload();
		},
		error: function (XMLHttpRequest, textStatus, errorThrown) {
			// console.log(XMLHttpRequest, textStatus, errorThrown);
		}
	});
});

function copyAddress() {
	$('#MailCountry').val($('#HomeCountry').val());
	$('#MailZIP').val($('#HomeZIP').val());
	$('#MailCity').val($('#HomeCity').val());
	$('#MailStreet').val($('#HomeStreet').val());
}

// To prevent accidental edits of the Name field
$('#name-unlock').click(function () {
	var buttonText = $(this).text();
	if (buttonText === 'Unlock') {
		$('#EmployeeName').prop('readonly', false);
		$(this).text('Lock');
	} else {
		$('#EmployeeName').prop('readonly', true);
		$(this).text('Unlock');
	}
});