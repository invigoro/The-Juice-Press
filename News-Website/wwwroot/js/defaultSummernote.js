$(function () {
    var def = $(".summernote").val();
    $("#DescriptionEncoded").val(encodeURIComponent(def));
    $(".summernote").summernote({
        height: 180,
        toolbar: [
            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['font', ['strikethrough', 'superscript', 'subscript']],
            ['fontsize', ['fontsize']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['height', ['height']]
        ],
        callbacks: {
            onChange: function (contents, $editable) {
                $("#DescriptionEncoded").val(encodeURIComponent(contents));
            }
        }
    });
    //$(".summernote").summernote("code", def );
})