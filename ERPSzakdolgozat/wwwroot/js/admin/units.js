$('.edit-record').click(function () {
	var recordId = $(this).data('id');
	$('#saveEdit').data('id', recordId);

	$.ajax({
		url: './GetUnit',
		type: 'POST',
		data: {
			id: recordId
		},
		dataType: "json",
		success: function (data) {
			$('#editTitle').text('Edit - ' + data.name);

			$('#editCode').val(data.code);
			$('#editName').val(data.name);
			$('#editActive').prop('checked', data.active);
		},
		error: function (XMLHttpRequest, textStatus, errorThrown) {
		}
	});
});

$('#saveEdit').click(function () {
	var recordId = $('#saveEdit').data('id');
	var code = $('#editCode').val();
	var name = $('#editName').val();
	var active = $('#editActive').prop('checked');

	if (recordId) {
		$.ajax({
			url: './SaveUnit',
			type: 'POST',
			data: {
				id: recordId,
				code: code,
				name: name,
				active: active
			},
			dataType: "json",
			success: function (data) {
				window.location.reload();
			},
			error: function (XMLHttpRequest, textStatus, errorThrown) {
			}
		});
	}
});

//$('#SaveNewFinancial').click(function () {
//	$.ajax({
//		url: '../AddFinancial',
//		type: 'POST',
//		data: {
//			currencyId: $('#nF_Currency').val(),
//			workHours: $('#nF_WorkHours').val(),
//			grossSalary: $('#nF_GrossSalary').val(),
//			cafeteria: $('#nF_Cafeteria').val(),
//			bonus: $('#nF_Bonus').val(),
//			employeeId: $('#EmployeeId').val()
//		},
//		dataType: "json",
//		success: function (data) {
//			if (data === 1) {
//				$('#NewFinancialAlert').removeClass('alert-success');
//				$('#NewFinancialAlert').addClass('alert-danger');
//				$('#NewFinancialAlert').text('errorvanbaszki');
//			} else {
//				$('#NewFinancialAlert').removeClass('alert-danger');
//				$('#NewFinancialAlert').addClass('alert-success');
//				$('#NewFinancialAlert').text('nincserrorjee');
//			}

//			$('#NewFinancialAlert').show();

//			window.location.reload();
//		},
//		error: function (XMLHttpRequest, textStatus, errorThrown) {
//			// console.log(XMLHttpRequest, textStatus, errorThrown);
//		}
//	});
//});