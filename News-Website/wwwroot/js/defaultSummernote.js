function defaultSummerNote() {
    var def = $(".summernote").val();
    $("#DraftContentEncoded").val(encodeURIComponent(def));
    $(".summernote").summernote({
        height: 500,
        toolbar: [
            ['style', ['style']],
            ['font', ['bold', 'italic', 'underline', 'color', 'clear', 'strikethrough', 'superscript', 'subscript']],
            ['fontsize', ['fontsize']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['height', ['height']],
            ['insert', ['link', 'table']],
            ['view', ['fullscreen', 'codeview', 'help']],
            ['options', ['undo', 'redo']]
        ],
        callbacks: {
            onChange: function (contents, $editable) {
                $("#DraftContentEncoded").val(encodeURIComponent(contents));
            }
        }
    });
}

$(function () {
    
    
    defaultSummerNote();
    //$(".summernote").summernote("code", def );
})