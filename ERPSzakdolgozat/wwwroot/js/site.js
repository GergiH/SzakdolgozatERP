$(document).ready(function () {
	// Tooltips
	$('[data-toggle="tooltip"]').tooltip(); 

	// Toastr
	$(document).ready(function () {
		toastr.options = {
			"closeButton": true,
			"debug": false,
			"newestOnTop": true,
			"progressBar": true,
			"positionClass": "toast-bottom-right",
			"preventDuplicates": false,
			"onclick": null,
			"showDuration": "300",
			"hideDuration": "2000",
			"timeOut": "5000",
			"extendedTimeOut": "2000",
			"showEasing": "swing",
			"hideEasing": "linear",
			"showMethod": "fadeIn",
			"hideMethod": "fadeOut"
		};

		switch (toastTemp) {
			case "created-success":
				toastr.success('Successfully created.');
				break;
			case "saved-success":
				console.log("ja");
				toastr.success('Successfully saved.');
				break;
			case "deleted-success":
				toastr.success('Successfully deleted.');
				break;
			case "created-fail":
				toastr.error('Failed to create.');
				break;
			case "saved-fail":
				toastr.error('Failed to save.');
				break;
			case "deleted-fail":
				toastr.error('Failed to delete.');
				break;
			default:
				break;
		}
	});
});