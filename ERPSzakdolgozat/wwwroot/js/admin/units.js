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
