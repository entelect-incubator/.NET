(function ($) {
	$(document).ready(function () {
		$('body').append(`
            <div class="toast-container" data-example-id="">
                <div class="alert alert-primary alert-with-icon">
                    <button type="button" aria-hidden="true" class="close" data-dismiss="alert" aria-label="Close">
                        <i class="fa fa-times"></i>
                    </button>
                    <div class="alert-icon"></div>
                    <span class="alert-message"></span>
                </div>

                <div class="alert alert-info alert-with-icon">
                    <button type="button" aria-hidden="true" class="close" data-dismiss="alert" aria-label="Close">
                        <i class="fa fa-times"></i>
                    </button>
                    <div class="alert-icon"></div>
                    <span class="alert-message"></span>
                </div>

                <div class="alert alert-warning alert-with-icon">
                    <button type="button" aria-hidden="true" class="close" data-dismiss="alert" aria-label="Close">
                        <i class="fa fa-times"></i>
                    </button>
                    <div class="alert-icon"></div>
                    <span class="alert-message"></span>
                </div>

                <div class="alert alert-danger alert-with-icon">
                    <button type="button" aria-hidden="true" class="close" data-dismiss="alert" aria-label="Close">
                        <i class="fa fa-times"></i>
                    </button>
                    <div class="alert-icon"></div>
                    <span class="alert-message"></span>
                </div>
            </div>
        `);
	});

	$.alert = function (message, callback) {
		alert(message, 'primary');

		if (callback) {
			callback();
		}
	};

	$.alertInfo = function (message, callback) {
		alert(message, 'info');

		if (callback) {
			callback();
		}
	};

	$.alertWarning = function (message, callback) {
		alert(message, 'warning');

		if (callback) {
			callback();
		}
	};

	$.alertDanger = function (message, callback) {
		alert(message, 'danger');

		if (callback) {
			callback();
		}
	};

	function alert(message, type) {
		var alert = $('.alert-' + type);
		alert.show();
		alert.find('.alert-message').html(message);
	}

}(jQuery))