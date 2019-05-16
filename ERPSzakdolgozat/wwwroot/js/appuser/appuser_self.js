$(document).ready(function () {
	$('#profile').bind('change', function () {
		var image, file;
		file = this.files[0];

		if (file) {
			image = new Image();
			image.src = window.URL.createObjectURL(file);

			image.onload = function () {
				if (this.width !== 256 && this.height !== 256) {
					$('#profileError').show();
					$('#save-button').prop('disabled', true);
				} else {
					$('#profileError').hide();
					$('#save-button').prop('disabled', false);
				}
			}
		} else {
			$('#profileError').hide();
			$('#save-button').prop('disabled', false);
		}
	});
});