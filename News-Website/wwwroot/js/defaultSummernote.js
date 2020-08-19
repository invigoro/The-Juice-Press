function defaultSummerNote() {
    var def = $(".summernote").val();
    $("#DraftContentEncoded").val(encodeURIComponent(def));
    $(".summernote").summernote({
        height: 500,
        fontSizes: ['8', '9', '10', '11', '12', '13', '14', '16', '18', '24', '36', '48'],
        toolbar: [
            ['style', ['style']],
            ['fontoptions', ['fontname', 'fontsize']],
            ['font', ['bold', 'italic', 'underline']]
            ['fontstyle', ['color', 'clear', 'strikethrough', 'superscript', 'subscript']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['height', ['height']],
            ['insert', ['link', 'table']],
            ['view', ['fullscreen', 'codeview', 'help']],
            ['options', ['undo', 'redo']]
        ],
        placeholder: "Write your article content here",
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