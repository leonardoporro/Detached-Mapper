(function () {
    $(function () {

        var options = {
            mode: 'tree'
        };
        var editor = new JSONEditor($('#jsoneditor')[0], options);

        $.getJSON('api/company/1')
         .done(function (data) {
             editor.set(data);
             editor.expandAll();
         });

        $('#saveButton').click(function () {
            $.ajax('api/company', {
                data: editor.getText(),
                contentType: 'application/json',
                type: 'POST'
            })
            .done(function () {
                location.reload();
            })
            .fail(function (error) {
                $('#errormsg').text(error.responseJSON.message).toggle();
            });
        });
    });
})();