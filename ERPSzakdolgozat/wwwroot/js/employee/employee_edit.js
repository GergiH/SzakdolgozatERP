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

function copyAddress() {
	$('#MailCountry').val($('#HomeCountry').val());
	$('#MailZIP').val($('#HomeZIP').val());
	$('#MailCity').val($('#HomeCity').val());
	$('#MailStreet').val($('#HomeStreet').val());
}