(function ($) {
    $.validator.unobtrusive.adapters.add('filetype', ['validtypes'], function (options) {
        options.rules['filetype'] = { validtypes: options.params.validtypes.split(',') };
        options.messages['filetype'] = options.message;
    });

    $.validator.addMethod("filetype", function (value, element, param) {
        for (var i = 0; i < element.files.length; i++) {
            var extension = getFileExtension(element.files[i].name);
            if ($.inArray(extension, param.validtypes) === -1) {
                return false;
            }
        }
        return true;
    });

    function getFileExtension(fileName) {
        if (/[.]/.exec(fileName)) {
            return /[^.]+$/.exec(fileName)[0].toLowerCase();
        }
        return null;
    }
}(jQuery));
